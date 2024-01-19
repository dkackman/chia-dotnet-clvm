using System.Numerics;
using System.Text;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public class Program
{
    public static readonly BigInteger cost = 11000000000;
    public static readonly Program True = FromBytes([1]);
    public static readonly Program False = FromBytes([]);
    public static readonly Program Nil = False;

    public Program(Cons value)
    {
        Value = value;
    }
    public Program(byte[] value)
    {
        Value = value;
    }

    public object Value { get; }

    public bool IsAtom => Value is byte[];
    public bool IsCons => Value is Cons;

    public bool IsNull => IsAtom && Atom.Length == 0;

    public byte[] Atom => Value as byte[] ?? throw new InvalidOperationException("Program is not an atom");
    public Cons Cons => Value as Cons ?? throw new InvalidOperationException("Program is not a cons");

    public Program First => Cons.Item1;
    public Program Rest => Cons.Item2;

    public static Program FromCons(Program program1, Program program2) => new(new Cons(program1, program2));
    public static Program FromBytes(byte[] value) => new(value);
    public static Program FromJacobianPoint(JacobianPoint value) => new(value.ToBytes());
    public static Program FromPrivateKey(PrivateKey value) => new(value.ToBytes());
    public static Program FromHex(string value) => new(value.HexStringToByteArray());
    public static Program FromBool(bool value) => value ? True : False;
    public static Program FromInt(long value) => new(value.EncodeInt());
    public static Program FromBigInt(BigInteger value) => new(ByteUtils.EncodeBigInt(value));
    public static Program FromText(string value) => new(value.ToBytes());

    public static Program FromSource(string source)
    {
        var stream = Parser.TokenStream(source);
        var tokens = stream.ToList();
        if (tokens.Count > 0)
        {
            return Parser.TokenizeExpr(source, tokens);
        }

        throw new ParseError("Unexpected end of source.");
    }

    public static Program FromList(Program[] value)
    {
        throw new NotImplementedException();
    }
    public static Program FromList(IList<Program> value) => FromList(value.Cast<Program>().ToArray());

    public string PositionSuffix => Position is not null ? $" at {Position}" : "";
    public Position? Position { get; private set; }

    public Program At(Position position)
    {
        Position = position;
        return this;
    }

    public Program Curry(IList<Program> args)
    {
        return Program.FromSource(
            "(a (q #a 4 (c 2 (c 5 (c 7 0)))) (c (q (c (q . 2) (c (c (q . 1) 5) (c (a 6 (c 2 (c 11 (q 1)))) 0))) #a (i 5 (q 4 (q . 4) (c (c (q . 1) 9) (c (a 6 (c 2 (c 13 (c 11 0)))) 0))) (q . 11)) 1) 1))"
        ).Run(Program.FromCons(this, Program.FromList(args.ToArray()
        ))).Value;
    }

    public Tuple<Program, List<Program>> Uncurry()
    {
        var uncurryPatternFunction = Program.FromSource("(a (q . (: . function)) (: . core))");
        var uncurryPatternCore = Program.FromSource("(c (q . (: . parm)) (: . core))");

        var result = Bindings.Match(uncurryPatternFunction, this);
        if (result == null) return null;

        var fn = result["function"];
        var core = result["core"];

        var args = new List<Program>();

        while (true)
        {
            result = Bindings.Match(uncurryPatternCore, core);
            if (result == null)
            {
                break;
            }

            args.Add(result["parm"]);
            core = result["core"];
        }

        if (core.IsAtom && core.ToBigInt() == 1)
        {
            return new Tuple<Program, List<Program>>(fn, args);
        }
        return null;
    }

    public byte[] Hash()
    {
        return IsAtom
            ? Hmac.Hash256(new byte[] { 1 }.Concat(Atom).ToArray())
            : Hmac.Hash256(new byte[] { 2 }.Concat(First.Hash()).Concat(Rest.Hash()).ToArray());
    }

    public string HashHex() => Hash().ToHex();

    public Program Define(Program program)
    {
        var result = this;
        if (IsAtom || First.IsCons || First.ToText() != "mod")
        {
            result = FromList(new List<Program> { FromText("mod"), Nil, this }.ToArray());
        }
        var items = result.ToList();
        items.Insert(2, program);
        return FromList(items.ToArray());
    }

    public Program DefineAll(IList<Program> programs)
    {
        var result = this;
        foreach (var program in programs.AsEnumerable().Reverse())
        {
            result = result.Define(program);
        }
        return result;
    }

    public ProgramOutput Compile(CompileOptions options = null)
    {
        var fullOptions = options ?? new CompileOptions
        {
            Strict = false,
            Operators = DefaultOperators.MakeDefaultOperators(),
            IncludeFilePaths = new Dictionary<string, IDictionary<string, string>>()
        };

        if (fullOptions.Strict)
        {
            fullOptions.Operators.Unknown = (_operator, args) =>
            {
                throw new Exception($"Unimplemented operator {args.PositionSuffix}.");
            };
        }

        ProgramOutput DoFullPathForName(Program args)
        {
            var fileName = args.First.ToText();
            foreach (var entry in fullOptions.IncludeFilePaths)
            {
                if (entry.Value.ContainsKey(fileName))
                {
                    return new ProgramOutput
                    {
                        Value = FromText($"{entry.Key}/{fileName}"),
                        Cost = 1
                    };
                }
            }

            throw new Exception($"Can't open {fileName}{args.PositionSuffix}.");
        }

        ProgramOutput DoRead(Program args)
        {
            var fileName = args.First.ToText();
            string source = null;
            foreach (var entry in fullOptions.IncludeFilePaths)
            {
                foreach (var file in entry.Value)
                {
                    if (fileName == $"{entry.Key}/{file.Key}")
                    {
                        source = file.Value;
                    }
                }
            }
            if (source == null)
            {
                throw new Exception($"Can't open {fileName}{args.PositionSuffix}.");
            }
            return new ProgramOutput { Value = FromSource(source), Cost = 1 };
        }

        ProgramOutput DoWrite(Program _args)
        {
            return new ProgramOutput { Value = Nil, Cost = 1 };
        }

        ProgramOutput RunProgram(Program program, Program args)
        {
            return program.Run(args, fullOptions);
        }

        var bindings = new Dictionary<string, Operator>
        {
            { "com", MakeDoCom(RunProgram) },
            { "opt", MakeDoOpt(RunProgram) },
            { "_full_path_for_name", DoFullPathForName },
            { "_read", DoRead },
            { "_write", DoWrite }
        };

        foreach (var binding in bindings)
        {
            fullOptions.Operators.Operators[binding.Key] = binding.Value;
        }

        return RunProgram(
            FromSource("(a (opt (com 2)) 3)"),
            FromList(new List<Program> { this }.ToArray())
        );
    }

    public ProgramOutput Run(Program environment, RunOptions options = null)
    {
        throw new NotImplementedException();
    }
    public IList<Program> ToList(bool strict = false)
    {
        throw new NotImplementedException();
    }

    public BigInteger ToBigInt()
    {
        throw new NotImplementedException();
    }
    public string ToText()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to text.");

        return Encoding.UTF8.GetString(Atom);
    }

    public string ToHex()
    {
        throw new NotImplementedException();
    }

    public int ToInt()
    {
        throw new NotImplementedException();
    }
    public string ToSource(bool showKeywords = true)
    {
        throw new NotImplementedException();
    }

    public byte[] Serialize()
    {
        if (IsAtom)
        {
            if (IsNull)
            {
                return new byte[] { 0x80 };
            }
            else if (Atom.Length == 1 && Atom[0] <= 0x7f)
            {
                return Atom;
            }
            else
            {
                var size = Atom.Length;
                var result = new List<byte>();
                if (size < 0x40)
                {
                    result.Add((byte)(0x80 | size));
                }
                else if (size < 0x2000)
                {
                    result.Add((byte)(0xc0 | (size >> 8)));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                else if (size < 0x100000)
                {
                    result.Add((byte)(0xe0 | (size >> 16)));
                    result.Add((byte)((size >> 8) & 0xff));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                else if (size < 0x8000000)
                {
                    result.Add((byte)(0xf0 | (size >> 24)));
                    result.Add((byte)((size >> 16) & 0xff));
                    result.Add((byte)((size >> 8) & 0xff));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                else if (size < 0x400000000)
                {
                    result.Add((byte)(0xf8 | (size >> 32)));
                    result.Add((byte)((size >> 24) & 0xff));
                    result.Add((byte)((size >> 16) & 0xff));
                    result.Add((byte)((size >> 8) & 0xff));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        $"Cannot serialize {ToString()} as it is 17,179,869,184 or more bytes in size{PositionSuffix}."
                    );
                }
                result.AddRange(Atom);
                return result.ToArray();
            }
        }
        else
        {
            var result = new List<byte> { 0xff };
            result.AddRange(First.Serialize());
            result.AddRange(Rest.Serialize());
            return result.ToArray();
        }
    }

    public string SerializeHex() => Serialize().ToHex();
}


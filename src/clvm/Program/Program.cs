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

    public static Program FromList(Program[] programs)
    {
        Program result = Nil;
        foreach (Program program in programs.Reverse())
        {
            result = FromCons(program, result);
        }

        return result;
    }

    public static Program FromList(IList<Program> value) => FromList(value.Cast<Program>().ToArray());

    public static Program Deserialize(byte[] bytes)
    {
        if (bytes.Length == 0)
            throw new ParseError("Unexpected end of source.");

        return Serialization.Deserialize([.. bytes]);
    }

    public static Program DeserializeHex(string hex) => Deserialize(hex.FromHex());

    public Program(Cons value) => Value = value;
    public Program(byte[] value) => Value = value;

    public object Value { get; }

    public bool IsAtom => Value is byte[];
    public bool IsCons => Value is Cons;
    public bool IsNull => IsAtom && Atom.Length == 0;

    public byte[] Atom => Value as byte[] ?? throw new InvalidOperationException("Program is not an atom");
    public Cons Cons => Value as Cons ?? throw new InvalidOperationException("Program is not a cons");

    public Program First => Cons.Item1;
    public Program Rest => Cons.Item2;

    public string PositionSuffix => Position is not null ? $" at {Position}" : "";
    public Position? Position { get; private set; }

    public Program At(Position position)
    {
        Position = position;
        return this;
    }

    public Program Curry(IList<Program> args)
    {
        return FromSource(
            "(a (q #a 4 (c 2 (c 5 (c 7 0)))) (c (q (c (q . 2) (c (c (q . 1) 5) (c (a 6 (c 2 (c 11 (q 1)))) 0))) #a (i 5 (q 4 (q . 4) (c (c (q . 1) 9) (c (a 6 (c 2 (c 13 (c 11 0)))) 0))) (q . 11)) 1) 1))"
        ).Run(FromCons(this, FromList([.. args]
        ))).Value;
    }

    public Tuple<Program, List<Program>>? Uncurry()
    {
        var uncurryPatternFunction = FromSource("(a (q . (: . function)) (: . core))");
        var uncurryPatternCore = FromSource("(c (q . (: . parm)) (: . core))");

        var result = Bindings.Match(uncurryPatternFunction, this);
        if (result == null)
        {
            return null;
        }

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

    public byte[] Hash() => IsAtom
            ? Hmac.Hash256([1, .. Atom])
            : Hmac.Hash256([2, .. First.Hash(), .. Rest.Hash()]);

    public string HashHex() => Hash().ToHex();

    public Program Define(Program program)
    {
        var result = this;
        if (IsAtom || First.IsCons || First.ToText() != "mod")
        {
            result = FromList([FromText("mod"), Nil, this]);
        }

        var items = result.ToList();
        items.Insert(2, program);

        return FromList([.. items]);
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

    public ProgramOutput Compile(CompileOptions? options = null)
    {
        var fullOptions = Bindings.Merge(new CompileOptions(), options);

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
            string? source = null;
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

        ProgramOutput DoWrite(Program _)
        {
            return new ProgramOutput { Value = Nil, Cost = 1 };
        }

        ProgramOutput RunProgram(Program program, Program args)
        {
            return program.Run(args, fullOptions);
        }

        fullOptions.Operators.Operators["com"] = clvm.Compile.MakeDoCom(RunProgram);
        fullOptions.Operators.Operators["opt"] = Optimize.MakeDoOpt(RunProgram);
        fullOptions.Operators.Operators["_full_path_for_name"] = DoFullPathForName;
        fullOptions.Operators.Operators["_read"] = DoRead;
        fullOptions.Operators.Operators["_write"] = DoWrite;

        return RunProgram(
            FromSource("(a (opt (com 2)) 3)"),
            FromList([this])
        );
    }

    public ProgramOutput Run(Program environment, RunOptions? options = null)
    {
        var fullOptions = Bindings.Merge(new CompileOptions(), options);

        if (fullOptions.Strict)
        {
            fullOptions.Operators.Unknown = (_operator, args) =>
            {
                throw new Exception($"Unimplemented operator{args.PositionSuffix}.");
            };
        }

        var instructionStack = new Stack<Instruction>([InstructionsClass.Instructions["eval"]]);
        var stack = new Stack<Program>([FromCons(this, environment)]);
        BigInteger cost = 0;
        while (instructionStack.Count > 0)
        {
            var instruction = instructionStack.Pop();
            cost += instruction(instructionStack, stack, fullOptions);
            if (fullOptions.MaxCost.HasValue && cost > fullOptions.MaxCost.Value)
                throw new Exception($"Exceeded cost of {fullOptions.MaxCost.Value}{stack.Peek().PositionSuffix}.");
        }

        return new ProgramOutput
        {
            Value = stack.Peek(),
            Cost = cost
        };
    }

    public byte[] ToBytes()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to hex{PositionSuffix}.");

        return Atom;
    }

    public JacobianPoint ToJacobianPoint()
    {
        if (IsCons || (Atom.Length != 48 && Atom.Length != 96))
            throw new Exception($"Cannot convert {ToString()} to JacobianPoint{PositionSuffix}.");

        return Atom.Length == 48
            ? JacobianPoint.FromBytesG1(Atom)
            : JacobianPoint.FromBytesG2(Atom);
    }

    public PrivateKey ToPrivateKey()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to private key{PositionSuffix}.");

        return PrivateKey.FromBytes(Atom);
    }

    public string ToHex()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to hex{PositionSuffix}.");

        return Atom.ToHex();
    }

    public bool ToBool()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to bool{PositionSuffix}.");

        return !IsNull;
    }

    public long ToInt()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to int{PositionSuffix}.");

        return Atom.BytesToInt(Endian.Big, true);
    }

    public BigInteger ToBigInt()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to bigint{PositionSuffix}.");

        return Atom.BytesToBigInt(Endian.Big, true);
    }

    public string ToText()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to text.");

        return Encoding.UTF8.GetString(Atom);
    }

    public string ToSource(bool showKeywords = true)
    {
        if (IsAtom)
        {
            if (IsNull)
            {
                return "()";
            }

            if (Atom.Length > 2)
            {
                try
                {
                    var str = ToText();
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (!Constants.Printable.Contains(str[i]))
                        {
                            return $"0x{ToHex()}";
                        }
                    }

                    if (str.Contains('"') && str.Contains('\''))
                    {
                        return $"0x{ToHex()}";
                    }

                    string quote = str.Contains('"') ? "'" : "\"";
                    return quote + str + quote;
                }
                catch
                {
                    return $"0x{ToHex()}";
                }
            }
            else if (ByteUtils.BytesEqual(ByteUtils.EncodeInt(Atom.BytesToInt(Endian.Big, true)), Atom))
            {
                return Atom.BytesToInt(Endian.Big, true).ToString();
            }
            else
            {
                return $"0x{ToHex()}";
            }
        }
        else
        {
            var result = "(";
            if (showKeywords && First.IsAtom)
            {
                var value = First.ToBigInt();
                var keyword = KeywordConstants.Keywords.FirstOrDefault(kvp => kvp.Value == value).Key;
                result += keyword ?? First.ToSource(showKeywords);
            }
            else
            {
                result += First.ToSource(showKeywords);
            }

            var current = Cons.Item2;
            while (current.IsCons)
            {
                result += $" {current.First.ToSource(showKeywords)}";
                current = current.Cons.Item2;
            }
            result += (current.IsNull ? "" : $" . {current.ToSource(showKeywords)}") + ")";

            return result;
        }
    }

    public IList<Program> ToList(bool strict = false)
    {
        List<Program> result = [];
        var current = this;
        while (current.IsCons)
        {
            Program item = current.First;
            result.Add(item);
            current = current.Rest;
        }

        if (!current.IsNull && strict)
            throw new Exception($"Expected strict list{PositionSuffix}.");

        return result;
    }

    public byte[] Serialize() => Serialization.Serialize(this);

    public string SerializeHex() => Serialize().ToHex();

    public bool Equals(Program value)
        => IsAtom == value.IsAtom && (IsAtom ? ByteUtils.BytesEqual(Atom, value.Atom) : First.Equals(value.First) && Rest.Equals(value.Rest));

    public override string ToString() => ToSource();
}

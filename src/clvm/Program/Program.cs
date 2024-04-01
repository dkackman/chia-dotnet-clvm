using System.Numerics;
using System.Text;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

/// <summary>
/// Represents a CLVM program.
/// </summary>
public class Program
{
    /// <summary>
    /// Represents the True program.
    /// </summary>
    public static readonly Program True = FromBytes([1]);

    /// <summary>
    /// Represents the False program.
    /// </summary>
    public static readonly Program False = FromBytes([]);

    /// <summary>
    /// Represents the Nil program.
    /// </summary>
    public static readonly Program Nil = False;

    /// <summary>
    /// Creates a program from two cons cells.
    /// </summary>
    /// <param name="program1">The first program.</param>
    /// <param name="program2">The second program.</param>
    /// <returns>The created program.</returns>
    public static Program FromCons(Program program1, Program program2) => new(new Cons(program1, program2));

    /// <summary>
    /// Creates a program from a byte array.
    /// </summary>
    /// <param name="value">The byte array.</param>
    /// <returns>The created program.</returns>
    public static Program FromBytes(byte[] value) => new(value);

    /// <summary>
    /// Creates a new Program from a JacobianPoint.
    /// </summary>
    /// <param name="value">The JacobianPoint to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromJacobianPoint(JacobianPoint value) => new(value.ToBytes());

    /// <summary>
    /// Creates a new Program from a PrivateKey.
    /// </summary>
    /// <param name="value">The PrivateKey to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromPrivateKey(PrivateKey value) => new(value.ToBytes());

    /// <summary>
    /// Creates a new Program from a hexadecimal string.
    /// </summary>
    /// <param name="value">The hexadecimal string to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromHex(string value) => new(value.HexStringToByteArray());

    /// <summary>
    /// Creates a new Program from a boolean value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromBool(bool value) => value ? True : False;

    /// <summary>
    /// Creates a new Program from a long integer.
    /// </summary>
    /// <param name="value">The long integer to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromInt(long value) => new(value.Encode());

    /// <summary>
    /// Creates a new Program from a BigInteger.
    /// </summary>
    /// <param name="value">The BigInteger to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromBigInt(BigInteger value) => new(value.Encode());

    /// <summary>
    /// Creates a new Program from a text string.
    /// </summary>
    /// <param name="value">The text string to convert.</param>
    /// <returns>A new Program.</returns>
    public static Program FromText(string value) => new(value.ToBytes());

    /// <summary>
    /// Creates a new Program from a source string.
    /// </summary>
    /// <param name="source">The source string to parse.</param>
    /// <returns>A new Program parsed from the source string.</returns>
    /// <exception cref="ParseError">Thrown when the source string is unexpectedly empty.</exception>
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

    /// <summary>
    /// Creates a new Program from a list of Programs.
    /// </summary>
    /// <param name="programs">The list of Programs to convert.</param>
    /// <returns>A new Program created from the list of Programs.</returns>
    public static Program FromList(IEnumerable<Program> programs)
    {
        Program result = Nil;
        foreach (Program program in programs.Reverse())
        {
            result = FromCons(program, result);
        }

        return result;
    }

    /// <summary>
    /// Deserializes a byte array into a Program.
    /// </summary>
    /// <param name="bytes">The byte array to deserialize.</param>
    /// <returns>A new Program deserialized from the byte array.</returns>
    /// <exception cref="ParseError">Thrown when the byte array is unexpectedly empty.</exception>
    public static Program Deserialize(byte[] bytes)
    {
        if (bytes.Length == 0)
            throw new ParseError("Unexpected end of source.");

        return Serialization.Deserialize([.. bytes]);
    }

    /// <summary>
    /// Deserializes a hexadecimal string into a Program.
    /// </summary>
    /// <param name="hex">The hexadecimal string to deserialize.</param>
    /// <returns>A new Program deserialized from the hexadecimal string.</returns>
    public static Program DeserializeHex(string hex) => Deserialize(hex.ToHexBytes());

    /// <summary>
    /// Initializes a new instance of the Program class with a Cons value.
    /// </summary>
    /// <param name="value">The Cons value to initialize with.</param>
    public Program(Cons value) => Value = value;

    /// <summary>
    /// Initializes a new instance of the Program class with a byte array value.
    /// </summary>
    /// <param name="value">The byte array value to initialize with.</param>
    public Program(byte[] value) => Value = value;

    /// <summary>
    /// Initializes a new instance of the Program class with an object.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the value isn't a Cons or byte array</exception>
    /// <param name="value">The byte array or Cons value to initialize with.</param>
    protected Program(object value)
    {
        Value = value;

        if (!IsCons && !IsAtom)
        {
            throw new InvalidOperationException("The value argument must be a Cons or byte array");
        }
    }

    /// <summary>
    /// Gets the value of the Program.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Gets a value indicating whether the Program is an atom.
    /// </summary>
    public bool IsAtom => Value is byte[];

    /// <summary>
    /// Gets a value indicating whether the Program is a Cons.
    /// </summary>
    public bool IsCons => Value is Cons;

    /// <summary>
    /// Gets a value indicating whether the Program is null.
    /// </summary>
    public bool IsNull => IsAtom && Atom.Length == 0;

    /// <summary>
    /// Gets the atom value of the Program.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the Program is not an atom.</exception>
    public byte[] Atom => Value as byte[] ?? throw new InvalidOperationException("Program is not an atom");

    /// <summary>
    /// Gets the Cons value of the Program.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the Program is not a Cons.</exception>
    public Cons Cons => Value as Cons ?? throw new InvalidOperationException("Program is not a cons");

    /// <summary>
    /// Gets the first Program in the Cons.
    /// </summary>
    public Program First => Cons.Item1;

    /// <summary>
    /// Gets the rest of the Programs in the Cons.
    /// </summary>
    public Program Rest => Cons.Item2;

    /// <summary>
    /// Gets the position suffix of the Program.
    /// </summary>
    public string PositionSuffix => Position is not null ? $" at {Position}" : "";

    /// <summary>
    /// Gets or privately sets the position of the Program.
    /// </summary>
    public Position? Position { get; private set; }

    /// <summary>
    /// Sets the position of the Program and returns the Program.
    /// </summary>
    /// <param name="position">The position to set.</param>
    /// <returns>The Program with the set position.</returns>
    public Program At(Position position)
    {
        Position = position;
        return this;
    }

    /// <summary>
    /// Curries the Program with a list of arguments.
    /// </summary>
    /// <param name="args">The list of arguments to curry the Program with.</param>
    /// <returns>A new Program that is the result of currying the original Program with the arguments.</returns>
    public Program Curry(IEnumerable<Program> args)
    {
        return FromSource(
            "(a (q #a 4 (c 2 (c 5 (c 7 0)))) (c (q (c (q . 2) (c (c (q . 1) 5) (c (a 6 (c 2 (c 11 (q 1)))) 0))) #a (i 5 (q 4 (q . 4) (c (c (q . 1) 9) (c (a 6 (c 2 (c 13 (c 11 0)))) 0))) (q . 11)) 1) 1))"
        ).Run(FromCons(this, FromList([.. args]
        ))).Value;
    }

    /// <summary>
    /// Uncurries the Program into a tuple containing the original Program and a list of arguments.
    /// </summary>
    /// <returns>A tuple containing the original Program and a list of arguments, or null if the Program cannot be uncurried.</returns>
    public Tuple<Program, IEnumerable<Program>>? Uncurry()
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
            return new Tuple<Program, IEnumerable<Program>>(fn, args);
        }

        return null;
    }

    /// <summary>
    /// Computes the hash of the Program.
    /// </summary>
    /// <returns>A byte array representing the hash of the Program.</returns>
    public byte[] Hash() => IsAtom
            ? Hmac.Hash256([1, .. Atom])
            : Hmac.Hash256([2, .. First.Hash(), .. Rest.Hash()]);

    /// <summary>
    /// Computes the hash of the Program and returns it as a hexadecimal string.
    /// </summary>
    /// <returns>A string representing the hash of the Program in hexadecimal.</returns>
    public string HashHex() => Hash().ToHex();

    /// <summary>
    /// Defines a new Program within the current Program.
    /// </summary>
    /// <param name="program">The Program to define within the current Program.</param>
    /// <returns>A new Program with the defined Program inserted.</returns>
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

    /// <summary>
    /// Defines multiple Programs within the current Program.
    /// </summary>
    /// <param name="programs">The Programs to define within the current Program.</param>
    /// <returns>A new Program with all the defined Programs inserted.</returns>
    public Program DefineAll(IEnumerable<Program> programs)
    {
        var result = this;
        foreach (var program in programs.AsEnumerable().Reverse())
        {
            result = result.Define(program);
        }

        return result;
    }

    /// <summary>
    /// Compiles the Program with the given options.
    /// </summary>
    /// <param name="options">The options to use when compiling the Program. If null, default options are used.</param>
    /// <returns>A ProgramOutput representing the result of the compilation.</returns>
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

    /// <summary>
    /// Runs the Program with the given environment and options.
    /// </summary>
    /// <param name="environment">The environment to use when running the Program.</param>
    /// <param name="options">The options to use when running the Program. If null, default options are used.</param>
    /// <returns>A ProgramOutput representing the result of the run.</returns>
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

    /// <summary>
    /// Converts the Program to a byte array.
    /// </summary>
    /// <returns>A byte array representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons.</exception>
    public byte[] ToBytes()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to hex{PositionSuffix}.");

        return Atom;
    }

    /// <summary>
    /// Converts the Program to a JacobianPoint.
    /// </summary>
    /// <returns>A JacobianPoint representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons or the Atom length is not 48 or 96.</exception>
    public JacobianPoint ToJacobianPoint()
    {
        if (IsCons || (Atom.Length != 48 && Atom.Length != 96))
            throw new Exception($"Cannot convert {ToString()} to JacobianPoint{PositionSuffix}.");

        return JacobianPoint.FromBytes(Atom);
    }

    /// <summary>
    /// Converts the Program to a PrivateKey.
    /// </summary>
    /// <returns>A PrivateKey representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons.</exception>
    public PrivateKey ToPrivateKey()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to private key{PositionSuffix}.");

        return PrivateKey.FromBytes(Atom);
    }

    /// <summary>
    /// Converts the Program to a hexadecimal string.
    /// </summary>
    /// <returns>A string representing the Program in hexadecimal.</returns>
    public string ToHex()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to hex{PositionSuffix}.");

        return Atom.ToHex();
    }

    /// <summary>
    /// Converts the Program to a boolean.
    /// </summary>
    /// <returns>A boolean representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons.</exception>
    public bool ToBool()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to bool{PositionSuffix}.");

        return !IsNull;
    }

    /// <summary>
    /// Converts the Program to a hint string.
    /// </summary>
    /// <returns>The hint</returns>
    public string ToHint() => FromBigInt(ToBigInt() + 1).ToHex().PadLeft(64, '0')[..64];

    /// <summary>
    /// Converts the Program to a long integer.
    /// </summary>
    /// <returns>A long integer representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons.</exception>
    public long ToInt()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to int{PositionSuffix}.");

        return Atom.DecodeInt();
    }

    /// <summary>
    /// Converts the Program to a BigInteger.
    /// </summary>
    /// <returns>A BigInteger representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons.</exception>
    public BigInteger ToBigInt()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to bigint{PositionSuffix}.");

        return Atom.DecodeBigInt();
    }

    /// <summary>
    /// Converts the Program to a text string.
    /// </summary>
    /// <returns>A string representing the Program in text.</returns>
    /// <exception cref="Exception">Thrown when the Program is a Cons.</exception>
    public string ToText()
    {
        if (IsCons)
            throw new Exception($"Cannot convert {ToString()} to text.");

        return Encoding.UTF8.GetString(Atom);
    }

    /// <summary>
    /// Converts the Program to its source code representation.
    /// </summary>
    /// <param name="showKeywords">A boolean indicating whether to include keywords in the source code representation. Defaults to true.</param>
    /// <returns>A string representing the source code of the Program.</returns>
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
            else if (ByteUtils.BytesEqual(Atom.DecodeInt().Encode(), Atom))
            {
                return Atom.DecodeInt().ToString();
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

    /// <summary>
    /// Converts the Program to a list of Programs.
    /// </summary>
    /// <param name="strict">A boolean indicating whether to enforce strict list conversion. Defaults to false.</param>
    /// <returns>A list of Programs representing the Program.</returns>
    /// <exception cref="Exception">Thrown when the Program is not a strict list and strict is true.</exception>
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

    /// <summary>
    /// Serializes the Program to a byte array.
    /// </summary>
    /// <returns>A byte array representing the serialized Program.</returns>
    public byte[] Serialize() => Serialization.Serialize(this);

    /// <summary>
    /// Serializes the Program to a hexadecimal string.
    /// </summary>
    /// <returns>A string representing the serialized Program in hexadecimal.</returns>
    public string SerializeHex() => Serialize().ToHex();

    /// <summary>
    /// Determines whether the specified Program is equal to the current Program.
    /// </summary>
    /// <param name="value">The Program to compare with the current Program.</param>
    /// <returns>true if the specified Program is equal to the current Program; otherwise, false.</returns>
    public bool Equals(Program value)
        => IsAtom == value.IsAtom && (IsAtom ? ByteUtils.BytesEqual(Atom, value.Atom) : First.Equals(value.First) && Rest.Equals(value.Rest));

    /// <summary>
    /// Returns a string that represents the current Program.
    /// </summary>
    /// <returns>A string that represents the current Program.</returns>
    public override string ToString() => ToSource();
}

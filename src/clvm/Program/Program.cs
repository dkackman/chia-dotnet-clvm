using System.Numerics;
using System.Runtime.InteropServices;
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

    public string PositionSuffix => Position is not null ? $" at {Position}" : "";
    public Position? Position { get; private set; }

    public Program At(Position position)
    {
        Position = position;
        return this;
    }

    public ProgramOutput Compile(CompileOptions options = null)
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
}


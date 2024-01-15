using System.Numerics;
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
    }
    public Program(byte[] value)
    {
    }

    public byte[] Atom { get; set; }
    public Cons Cons { get; set; }

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

    public string PositionSuffix { get; set; }

    public bool IsAtom { get; set; }
    public bool IsCons { get; set; }
    public bool IsNull { get; set; }
    public Program First { get; set; }
    public Program Rest { get; set; }

    public static Program FromHex(string value)
    {
        throw new NotImplementedException();
    }
    public static Program FromInt(long value)
    {
        throw new NotImplementedException();
    }
    public static Program FromBigInt(BigInteger value)
    {
        throw new NotImplementedException();
    }
    public static Program FromBool(bool value)
    {
        throw new NotImplementedException();
    }
    public static Program FromText(string value)
    {
        throw new NotImplementedException();
    }
    public static Program FromSource(string value)
    {
        throw new NotImplementedException();
    }
    public static Program FromBytes(byte[] value)
    {
        throw new NotImplementedException();
    }
    public static Program FromJacobianPoint(JacobianPoint value)
    {
        throw new NotImplementedException();
    }
    public static Program FromPrivateKey(PrivateKey value)
    {
        throw new NotImplementedException();
    }
    public static Program FromList(Program[] value)
    {
        throw new NotImplementedException();
    }
    public static Program FromCons(Program program1, Program program2)
    {
        throw new NotImplementedException();
    }
}
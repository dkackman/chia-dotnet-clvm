using System.Numerics;

namespace chia.dotnet.clvm;

internal static class Costs
{
    public static readonly  BigInteger If = 33;
    public static readonly  BigInteger Cons = 50;
    public static readonly  BigInteger First = 30;
    public static readonly  BigInteger Rest = 30;
    public static readonly  BigInteger Listp = 19;
    public static readonly  BigInteger MallocPerByte = 10;
    public static readonly  BigInteger ArithBase = 99;
    public static readonly  BigInteger ArithPerByte = 3;
    public static readonly  BigInteger ArithPerArg = 320;
    public static readonly  BigInteger LogBase = 100;
    public static readonly  BigInteger LogPerByte = 3;
    public static readonly  BigInteger LogPerArg = 264;
    public static readonly  BigInteger GrsBase = 117;
    public static readonly  BigInteger GrsPerByte = 1;
    public static readonly  BigInteger EqBase = 117;
    public static readonly  BigInteger EqPerByte = 1;
    public static readonly  BigInteger GrBase = 498;
    public static readonly  BigInteger GrPerByte = 2;
    public static readonly  BigInteger DivmodBase = 1116;
    public static readonly  BigInteger DivmodPerByte = 6;
    public static readonly  BigInteger DivBase = 988;
    public static readonly  BigInteger DivPerByte = 4;
    public static readonly  BigInteger Sha256Base = 87;
    public static readonly  BigInteger Sha256PerByte = 2;
    public static readonly  BigInteger Sha256PerArg = 134;
    public static readonly  BigInteger PointAddBase = 101094;
    public static readonly  BigInteger PointAddPerArg = 1343980;
    public static readonly  BigInteger PubkeyBase = 1325730;
    public static readonly  BigInteger PubkeyPerByte = 38;
    public static readonly  BigInteger MulBase = 92;
    public static readonly  BigInteger MulPerOp = 885;
    public static readonly  BigInteger MulLinearPerByte = 6;
    public static readonly  BigInteger MulSquarePerByteDivider = 128;
    public static readonly  BigInteger StrlenBase = 173;
    public static readonly  BigInteger StrlenPerByte = 1;
    public static readonly  BigInteger PathLookupBase = 40;
    public static readonly  BigInteger PathLookupPerLeg = 4;
    public static readonly  BigInteger PathLookupPerZeroByte = 4;
    public static readonly  BigInteger ConcatBase = 142;
    public static readonly  BigInteger ConcatPerByte = 3;
    public static readonly  BigInteger ConcatPerArg = 135;
    public static readonly  BigInteger BoolBase = 200;
    public static readonly  BigInteger BoolPerArg = 300;
    public static readonly  BigInteger AshiftBase = 596;
    public static readonly  BigInteger AshiftPerByte = 3;
    public static readonly  BigInteger LshiftBase = 277;
    public static readonly  BigInteger LshiftPerByte = 3;
    public static readonly  BigInteger LognotBase = 331;
    public static readonly  BigInteger LognotPerByte = 3;
    public static readonly  BigInteger Apply = 90;
    public static readonly  BigInteger Quote = 20;
}

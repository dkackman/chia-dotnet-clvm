using System.Numerics;
using System.Globalization;

namespace chia.dotnet.clvm; 

public static class Constants
{
    public const string Printable = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ \t\n\v\f";

    public static readonly BigInteger N = BigInteger.Parse("073eda753299d7d483339d80809a1d80553bda402fffe5bfeffffffff00000001", NumberStyles.AllowHexSpecifier);
}
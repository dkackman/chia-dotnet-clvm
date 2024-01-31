using chia.dotnet.bls;

namespace chia.dotnet.clvm;

internal static class Atoms
{
    public static readonly byte[] QuoteAtom = ByteUtils.EncodeBigInt(KeywordConstants.Keywords["q"]);
    public static readonly byte[] ApplyAtom = ByteUtils.EncodeBigInt(KeywordConstants.Keywords["a"]);
    public static readonly byte[] FirstAtom = ByteUtils.EncodeBigInt(KeywordConstants.Keywords["f"]);
    public static readonly byte[] RestAtom = ByteUtils.EncodeBigInt(KeywordConstants.Keywords["r"]);
    public static readonly byte[] ConsAtom = ByteUtils.EncodeBigInt(KeywordConstants.Keywords["c"]);
    public static readonly byte[] RaiseAtom = ByteUtils.EncodeBigInt(KeywordConstants.Keywords["x"]);
}
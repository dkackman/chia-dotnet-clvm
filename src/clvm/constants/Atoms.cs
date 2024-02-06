using chia.dotnet.bls;

namespace chia.dotnet.clvm;

internal static class Atoms
{
    public static readonly byte[] QuoteAtom = KeywordConstants.Keywords["q"].EncodeBigInt();
    public static readonly byte[] ApplyAtom = KeywordConstants.Keywords["a"].EncodeBigInt();
    public static readonly byte[] FirstAtom = KeywordConstants.Keywords["f"].EncodeBigInt();
    public static readonly byte[] RestAtom = KeywordConstants.Keywords["r"].EncodeBigInt();
    public static readonly byte[] ConsAtom = KeywordConstants.Keywords["c"].EncodeBigInt();
    public static readonly byte[] RaiseAtom = KeywordConstants.Keywords["x"].EncodeBigInt();
}

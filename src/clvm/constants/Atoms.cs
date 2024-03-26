using chia.dotnet.bls;

namespace chia.dotnet.clvm;

internal static class Atoms
{
    public static readonly byte[] QuoteAtom = KeywordConstants.Keywords["q"].Encode();
    public static readonly byte[] ApplyAtom = KeywordConstants.Keywords["a"].Encode();
    public static readonly byte[] FirstAtom = KeywordConstants.Keywords["f"].Encode();
    public static readonly byte[] RestAtom = KeywordConstants.Keywords["r"].Encode();
    public static readonly byte[] ConsAtom = KeywordConstants.Keywords["c"].Encode();
    public static readonly byte[] RaiseAtom = KeywordConstants.Keywords["x"].Encode();
}

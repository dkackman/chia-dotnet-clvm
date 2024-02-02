using System.Numerics;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

internal class NodePath
{
    public static readonly NodePath Top = new(1);
    public static readonly NodePath Left = Top.First();
    public static readonly NodePath Right = Top.Rest();

    private BigInteger index;

    public NodePath(BigInteger index)
    {
        if (index < 0)
        {
            var byteCount = (index.GetBitLength() + 7) >> 3;
            var blob = index.BigIntToBytes((int)byteCount, Endian.Big, true);
            index = ByteUtils.BytesToBigInt([0, .. blob]);
        }
        this.index = index;
    }

    public byte[] AsPath()
    {
        var byteCount = (index.GetBitLength() + 7) >> 3;
        return index.BigIntToBytes((int)byteCount);
    }

    public NodePath Add(NodePath other) => new(ComposePaths(index, other.index));

    public NodePath First() => new(index * 2);

    public NodePath Rest() => new(index * 2 + 1);

    public override string ToString() => $"NodePath: {index}";

    public static BigInteger ComposePaths(BigInteger left, BigInteger right)
    {
        BigInteger mask = 1;
        var tempPath = left;
        while (tempPath > 1)
        {
            right <<= 1;
            mask <<= 1;
            tempPath >>= 1;
        }
        mask -= 1;

        return right | (left & mask);
    }
}

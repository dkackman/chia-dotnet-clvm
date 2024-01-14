using System.Numerics;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public class NodePath
{
    public static readonly NodePath Top = new(1);
    public static readonly NodePath Left = Top.First();
    public static readonly NodePath Right = Top.Rest();

    private BigInteger index;

    public NodePath(BigInteger index)
    {
        if (index < 0)
        {
            // You need to implement bigIntBitLength, bigIntToBytes, and bytesToBigInt methods
            var byteCount = (ByteUtils.BigIntBitLength(index) + 7) >> 3;
            var blob = ByteUtils.BigIntToBytes(index, (int)byteCount, Endian.Big, true);
            index = ByteUtils.BytesToBigInt([0, .. blob]);
        }
        this.index = index;
    }

    public byte[] AsPath()
    {
        // You need to implement bigIntBitLength and bigIntToBytes methods
        var byteCount = (ByteUtils.BigIntBitLength(index) + 7) >> 3;
        return ByteUtils.BigIntToBytes(index, (int)byteCount);
    }

    public NodePath Add(NodePath other) => new(ComposePaths(index, other.index));

    public NodePath First()
    {
        return new NodePath(index * 2);
    }

    public NodePath Rest()
    {
        return new NodePath(index * 2 + 1);
    }

    public override string ToString()
    {
        return $"NodePath: {index}";
    }

    public static BigInteger ComposePaths(BigInteger left, BigInteger right)
    {
        BigInteger mask = new(1);
        BigInteger tempPath = left;
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
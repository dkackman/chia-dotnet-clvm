using System.Numerics;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public class NodePath
{
    public static int IntBitLength(long value)
    {
        if (value == 0)
        {
            return 0;
        }
        
        value = Math.Abs(value);
        int bits = 0;
        while (value > 0)
        {
            bits++;
            value >>= 1;
        }
        return bits;
    }

    /// <summary>
    /// Calculates the number of bits required to represent a BigInteger.
    /// </summary>
    /// <param name="value">The BigInteger value.</param>
    /// <returns>The number of bits required to represent the BigInteger.</returns>
    public static long BigIntBitLength(BigInteger value) => value == 0 ? 0 : value.GetBitLength();

    public static readonly NodePath Top = new(1);
    public static readonly NodePath Left = Top.First();
    public static readonly NodePath Right = Top.Rest();

    private BigInteger index;

    public NodePath(BigInteger index)
    {
        if (index < 0)
        {
            // You need to implement bigIntBitLength, bigIntToBytes, and bytesToBigInt methods
            var byteCount = (BigIntBitLength(index) + 7) >> 3;
            var blob = ByteUtils.BigIntToBytes(index, (int)byteCount, Endian.Big, true);
            index = ByteUtils.BytesToBigInt([0, .. blob]);
        }
        this.index = index;
    }

    public byte[] AsPath()
    {
        // You need to implement bigIntBitLength and bigIntToBytes methods
        var byteCount = (BigIntBitLength(index) + 7) >> 3;
        return ByteUtils.BigIntToBytes(index, (int)byteCount);
    }

    public NodePath Add(NodePath other)
    {
        // You need to implement composePaths method
        return new NodePath(ComposePaths(index, other.index));
    }

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
        BigInteger mask = new BigInteger(1);
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
using chia.dotnet.bls;

namespace chia.dotnet.clvm;


internal static class Serialization
{
    public static byte[] Serialize(Program program)
    {
        if (program.IsAtom)
        {
            if (program.IsNull)
            {
                return new byte[] { 0x80 };
            }
            else if (program.Atom.Length == 1 && program.Atom[0] <= 0x7f)
            {
                return program.Atom;
            }
            else
            {
                var size = program.Atom.Length;
                var result = new List<byte>();
                if (size < 0x40)
                {
                    result.Add((byte)(0x80 | size));
                }
                else if (size < 0x2000)
                {
                    result.Add((byte)(0xc0 | (size >> 8)));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                else if (size < 0x100000)
                {
                    result.Add((byte)(0xe0 | (size >> 16)));
                    result.Add((byte)((size >> 8) & 0xff));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                else if (size < 0x8000000)
                {
                    result.Add((byte)(0xf0 | (size >> 24)));
                    result.Add((byte)((size >> 16) & 0xff));
                    result.Add((byte)((size >> 8) & 0xff));
                    result.Add((byte)((size >> 0) & 0xff));
                }
                // else if (size < 0x400000000)
                // {
                //     result.Add((byte)(0xf8 | (size >> 32)));
                //     result.Add((byte)((size >> 24) & 0xff));
                //     result.Add((byte)((size >> 16) & 0xff));
                //     result.Add((byte)((size >> 8) & 0xff));
                //     result.Add((byte)((size >> 0) & 0xff));
                // }
                else
                {
                    throw new ArgumentOutOfRangeException(
                        $"Cannot serialize {program.ToString()} as it is 17,179,869,184 or more bytes in size{program.PositionSuffix}."
                    );
                }
                result.AddRange(program.Atom);
                return [.. result];
            }
        }
        else
        {
            var result = new List<byte> { 0xff };
            result.AddRange(program.First.Serialize());
            result.AddRange(program.Rest.Serialize());
            return result.ToArray();
        }
    }

    public static Program Deserialize(List<byte> program)
    {
        var sizeBytes = new List<byte>();
        if (program[0] <= 0x7f)
        {
            return Program.FromBytes(new byte[] { (byte)program[0] });
        }

        if (program[0] <= 0xbf)
        {
            sizeBytes.Add((byte)(program[0] & 0x3f));
        }
        else if (program[0] <= 0xdf)
        {
            sizeBytes.Add((byte)(program[0] & 0x1f));
            program.RemoveAt(0);
            if (!program.Any())
                throw new ParseError("Expected next byte in source.");
            sizeBytes.Add(program[0]);
        }
        else if (program[0] <= 0xef)
        {
            sizeBytes.Add((byte)(program[0] & 0x0f));
            for (int i = 0; i < 2; i++)
            {
                program.RemoveAt(0);
                if (!program.Any())
                    throw new ParseError("Expected next byte in source.");
                sizeBytes.Add(program[0]);
            }
        }
        else if (program[0] <= 0xf7)
        {
            sizeBytes.Add((byte)(program[0] & 0x07));
            for (int i = 0; i < 3; i++)
            {
                program.RemoveAt(0);
                if (!program.Any())
                    throw new ParseError("Expected next byte in source.");
                sizeBytes.Add(program[0]);
            }
        }
        else if (program[0] <= 0xfb)
        {
            sizeBytes.Add((byte)(program[0] & 0x03));
            for (int i = 0; i < 4; i++)
            {
                program.RemoveAt(0);
                if (!program.Any())
                    throw new ParseError("Expected next byte in source.");
                sizeBytes.Add(program[0]);
            }
        }
        else if (program[0] == 0xff)
        {
            program.RemoveAt(0);
            if (!program.Any())
                throw new ParseError("Expected next byte in source.");
            Program first = Deserialize(program);
            program.RemoveAt(0);
            if (!program.Any())
                throw new ParseError("Expected next byte in source.");
            Program rest = Deserialize(program);
            return Program.FromCons(first, rest);
        }
        else
        {
            throw new ParseError("Invalid encoding.");
        }

        int size = (int)ByteUtils.BytesToInt(sizeBytes.ToArray(), Endian.Big, true);// DecodeInt(sizeInts.ToArray());
        List<byte> bytes = new List<byte>();
        for (int i = 0; i < size; i++)
        {
            program.RemoveAt(0);
            if (!program.Any())
                throw new ParseError("Expected next byte in atom.");
            bytes.Add((byte)program[0]);
        }
        return Program.FromBytes(bytes.ToArray());
    }
}
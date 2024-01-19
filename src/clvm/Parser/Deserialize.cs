using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public static class Serialization
{
    public static Program Deserialize(List<int> program)
    {
        List<int> sizeInts = new List<int>();
        if (program[0] <= 0x7f)
            return Program.FromBytes(new byte[] { (byte)program[0] });
        else if (program[0] <= 0xbf) sizeInts.Add(program[0] & 0x3f);
        else if (program[0] <= 0xdf)
        {
            sizeInts.Add(program[0] & 0x1f);
            program.RemoveAt(0);
            if (!program.Any())
                throw new ParseError("Expected next byte in source.");
            sizeInts.Add(program[0]);
        }
        else if (program[0] <= 0xef)
        {
            sizeInts.Add(program[0] & 0x0f);
            for (int i = 0; i < 2; i++)
            {
                program.RemoveAt(0);
                if (!program.Any())
                    throw new ParseError("Expected next byte in source.");
                sizeInts.Add(program[0]);
            }
        }
        else if (program[0] <= 0xf7)
        {
            sizeInts.Add(program[0] & 0x07);
            for (int i = 0; i < 3; i++)
            {
                program.RemoveAt(0);
                if (!program.Any())
                    throw new ParseError("Expected next byte in source.");
                sizeInts.Add(program[0]);
            }
        }
        else if (program[0] <= 0xfb)
        {
            sizeInts.Add(program[0] & 0x03);
            for (int i = 0; i < 4; i++)
            {
                program.RemoveAt(0);
                if (!program.Any())
                    throw new ParseError("Expected next byte in source.");
                sizeInts.Add(program[0]);
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
        
        var sizeBytes = sizeInts.Select(i => (byte)i).ToArray();
        int size = (int)ByteUtils.BytesToInt(sizeBytes, Endian.Big, true);// DecodeInt(sizeInts.ToArray());
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
using System.Numerics;

namespace chia.dotnet.clvm;

public record ProgramOutput
{
    public Program Value { get; init; } = new([]);
    public BigInteger Cost { get; init; }
}

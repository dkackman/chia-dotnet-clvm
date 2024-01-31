using System.Numerics;

namespace chia.dotnet.clvm;

/// <summary>
/// Represents the output of a CLVM program execution.
/// </summary>
public record ProgramOutput
{
    /// <summary>
    /// Gets or initializes the value produced by the CLVM program.
    /// </summary>
    public Program Value { get; init; } = new([]);

    /// <summary>
    /// Gets or initializes the cost of executing the CLVM program.
    /// </summary>
    public BigInteger Cost { get; init; }
}

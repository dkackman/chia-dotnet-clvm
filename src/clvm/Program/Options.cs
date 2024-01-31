using System.Numerics;

namespace chia.dotnet.clvm;

/// <summary>
/// Represents the options for running a CLVM program.
/// </summary>
public record RunOptions
{
    /// <summary>
    /// Gets or sets the maximum cost allowed for executing the program.
    /// </summary>
    public BigInteger? MaxCost { get; init; }

    /// <summary>
    /// Gets or sets the type of operators to be used in the program.
    /// </summary>
    public OperatorsType Operators { get; init; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether strict mode is enabled.
    /// </summary>
    public bool Strict { get; init; }
}

/// <summary>
/// Represents the options for compiling a CLVM program.
/// </summary>
public record CompileOptions : RunOptions
{
    /// <summary>
    /// Gets or sets the include file paths used during compilation.
    /// </summary>
    public IDictionary<string, IDictionary<string, string>> IncludeFilePaths { get; init; } = new Dictionary<string, IDictionary<string, string>>();
}
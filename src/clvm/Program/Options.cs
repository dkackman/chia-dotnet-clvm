using System.Numerics;

namespace chia.dotnet.clvm;

public record RunOptions
{
    public BigInteger? MaxCost { get; init; }
    public OperatorsType Operators { get; init; } = new OperatorsType();
    public bool Strict { get; init; }
}

public record CompileOptions : RunOptions
{
    public IDictionary<string, IDictionary<string, string>> IncludeFilePaths { get; init; } = new Dictionary<string, IDictionary<string, string>>();
}
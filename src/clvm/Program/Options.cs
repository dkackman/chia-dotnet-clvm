using System.Numerics;

namespace chia.dotnet.clvm;

public record RunOptions
{
    public BigInteger? MaxCost { get; init; }
    public object Operators { get; init; }
    public bool Strict { get; init; }
}

public record CompileOptions : RunOptions
{
    public IDictionary<string, IDictionary<string, string>> IncludeFilePaths { get; init; }
}
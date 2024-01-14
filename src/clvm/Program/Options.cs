using System.Numerics;

namespace chia.dotnet.clvm;

public record RunOptions
{
    public RunOptions(BigInteger maxCost, object operators, bool strict = false)
    {
        MaxCost = maxCost;
        Operators = operators;
        Strict = strict;
    }

    public BigInteger MaxCost { get; init; }
    public object Operators { get; init; }
    public bool Strict { get; init; }
}

public record CompileOptions : RunOptions
{
    public CompileOptions(BigInteger maxCost, object operators, bool strict = false, IDictionary<string, IDictionary<string, string>>? includeFilePaths = null)
        : base(maxCost, operators, strict)
    {
        IncludeFilePaths = includeFilePaths ?? new Dictionary<string, IDictionary<string, string>>();
    }

    public IDictionary<string, IDictionary<string, string>> IncludeFilePaths { get; init; }
}
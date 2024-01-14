namespace chia.dotnet.clvm;

public delegate ProgramOutput Operator(Program args);

public record OperatorsType
{
    public IDictionary<string, Operator> Operators { get; init; } = new Dictionary<string, Operator>();
    public Func<Program, Program, ProgramOutput> Unknown { get; init; } = (a, b) => new ProgramOutput(new Program(), 0);
    public string Quote { get; init; } = string.Empty;
    public string Apply { get; init; } = string.Empty;
}
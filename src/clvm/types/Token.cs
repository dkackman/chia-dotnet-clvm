namespace chia.dotnet.clvm;

public record Token{
    public string Text { get; init; } = "";
    public int Index { get; init; }
}
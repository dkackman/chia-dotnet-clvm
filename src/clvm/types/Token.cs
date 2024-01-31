namespace chia.dotnet.clvm;

internal record Token{
    public string Text { get; init; } = "";
    public int Index { get; init; }
}
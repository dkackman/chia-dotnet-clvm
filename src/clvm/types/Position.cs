namespace chia.dotnet.clvm;

public class Position
{
    public int Line { get; init; }
    public int Column { get; init; }

    public Position(string source, int index)
    {
        source = source.Replace("\r\n", "\n");
        int line = 1;
        int column = 1;
        for (int i = 0; i < index; i++)
        {
            if (CheckForChar(source, index, '\n'))
            {
                line++;
                column = 1;
            }
            else
            {
                column++;
            }
        }
        Line = line;
        Column = column;
    }

    public override string ToString() => $"{Line}:{Column}";

    private static bool CheckForChar(string source, int index, char expected)
    {
        if (source.Length <= index)
        {
            return false;
        }
        return source[index] == expected;
    }
}

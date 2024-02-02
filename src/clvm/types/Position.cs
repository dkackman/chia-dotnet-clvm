namespace chia.dotnet.clvm;

/// <summary>
/// Represents a position in a source code file, specified by line and column numbers.
/// </summary>
public class Position
{
    /// <summary>
    /// Gets the line number of the position.
    /// </summary>
    public int Line { get; init; }

    /// <summary>
    /// Gets the column number of the position.
    /// </summary>
    public int Column { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class with the specified source code and index.
    /// </summary>
    /// <param name="source">The source code.</param>
    /// <param name="index">The index of the position in the source code.</param>
    public Position(string source, int index)
    {
        source = source.Replace("\r\n", "\n");
        var line = 1;
        var column = 1;
        for (var i = 0; i < index; i++)
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

    /// <summary>
    /// Returns a string that represents the current position in the format "line:column".
    /// </summary>
    /// <returns>A string representation of the position.</returns>
    public override string ToString() => $"{Line}:{Column}";

    private static bool CheckForChar(string source, int index, char expected) => source.Length > index && source[index] == expected;
}

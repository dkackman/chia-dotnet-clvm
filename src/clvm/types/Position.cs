using System;

namespace chia.dotnet.clvm;
public class Position
{
    public int Line { get; set; }
    public int Column { get; set; }

    public Position(string source, int index)
    {
        source = source.Replace("\r\n", "\n");
        int line = 1;
        int column = 1;
        for (int i = 0; i < index; i++)
        {
            if (source[i] == '\n')
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
}

using System.Numerics;
using System.Runtime.InteropServices;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public static class Parser
{
    public static Token? Next(IList<Token> tokens)
    {
        tokens.RemoveAt(0);
        return tokens.Count > 0 ? tokens[0] : null;
    }

    public static void Expect(string source, IList<Token> tokens)
    {
        Token token = tokens[0];
        if (Next(tokens) is null)
            throw new ParseError($"Unexpected end of source at {new Position(source, token.Index)}.");
    }
}
using System.Numerics;
using System.Text.RegularExpressions;

namespace chia.dotnet.clvm;

public static partial class Parser
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

    public static bool IsSpace(char c) => SpaceRegex().IsMatch(c.ToString());

    [GeneratedRegex(@"^[\u0020\u202F\u205F\u2028\u2029\u3000\u0085\u1680\u00A0\u2000-\u200A\u0009-\u000D\u001C-\u001F]$")]
    private static partial Regex SpaceRegex();

    public static int ConsumeWhitespace(string text, int index)
    {
        while (true)
        {
            while (index < text.Length && IsSpace(text[index]))
            {
                index++;
            }
            if (index >= text.Length || text[index] != ';')
            {
                break;
            }
            while (index < text.Length && !"\n\r".Contains(text[index]))
            {
                index++;
            }
        }
        return index;
    }

    public static Token ConsumeUntilWhitespace(string text, int index)
    {
        int start = index;
        while (index < text.Length && !IsSpace(text[index]) && text[index] != ')')
            index++;
        return new Token { Text = text.Substring(start, index - start), Index = start };
    }

    public static Program TokenizeCons(string source, List<Token> tokens)
    {
        Token token = tokens[0];
        if (token.Text == ")")
            return Program.FromBytes(new byte[0]).At(new Position(source, token.Index));

        int consStart = token.Index;
        Program first = TokenizeExpr(source, tokens);
        Expect(source, tokens);
        token = tokens[0];

        Program rest;
        if (token.Text == ".")
        {
            int dotStart = token.Index;
            Expect(source, tokens);
            token = tokens[0];
            rest = TokenizeExpr(source, tokens);
            Expect(source, tokens);
            token = tokens[0];
            if (token.Text != ")")
                throw new ParseError($"Illegal dot expression at {new Position(source, dotStart)}.");
        }
        else
        {
            rest = TokenizeCons(source, tokens);
        }

        return Program.FromCons(first, rest).At(new Position(source, consStart));
    }

    public static Program TokenizeInt(string source, Token token)
    {
        if (MyRegex().IsMatch(token.Text))
        {
            return Program.FromBigInt(BigInteger.Parse(token.Text.Replace("_", ""))).At(new Position(source, token.Index));
        }
        else
        {
            return null;
        }
    }

    public static Program TokenizeHex(string source, Token token)
    {
        if (token.Text.Length >= 2 && token.Text.Substring(0, 2).ToLower() == "0x")
        {
            string hex = token.Text.Substring(2);
            if (hex.Length % 2 == 1) hex = $"0{hex}";
            try
            {
                return Program.FromHex(hex).At(new Position(source, token.Index));
            }
            catch (Exception)
            {
                throw new ParseError($"Invalid hex {token.Text} at {new Position(source, token.Index)}.");
            }
        }
        else
        {
            return null;
        }
    }

    [GeneratedRegex(@"^[+\-]?[0-9]+(?:_[0-9]+)*$")]
    private static partial Regex MyRegex();

    public static Program TokenizeQuotes(string source, Token token)
    {
        if (token.Text.Length < 2) return null;
        char quote = token.Text[0];
        if (!"\"'".Contains(quote)) return null;
        if (token.Text[token.Text.Length - 1] != quote)
            throw new ParseError($"Unterminated string {token.Text} at {new Position(source, token.Index)}.");
        return Program.FromText(token.Text.Substring(1, token.Text.Length - 2)).At(new Position(source, token.Index));
    }

    public static Program TokenizeSymbol(string source, Token token)
    {
        string text = token.Text;
        if (text.StartsWith('#')) text = text.Substring(1);
        BigInteger? keyword = KeywordConstants.Keywords.GetValueOrDefault(text);
        return (keyword == null ? Program.FromText(text) : Program.FromBigInt(keyword.Value)).At(new Position(source, token.Index));
    }

    public static Program TokenizeExpr(string source, List<Token> tokens)
    {
        Token token = tokens[0];
        if (token.Text == "(")
        {
            Expect(source, tokens);
            return TokenizeCons(source, tokens);
        }

        Program result = TokenizeInt(source, token) ??
                         TokenizeHex(source, token) ??
                         TokenizeQuotes(source, token) ??
                         TokenizeSymbol(source, token);

        if (result == null)
            throw new ParseError($"Invalid expression {token.Text} at {new Position(source, token.Index)}.");

        return result;
    }

    public static IEnumerable<Token> TokenStream(string source)
    {
        int index = 0;
        while (index < source.Length)
        {
            index = ConsumeWhitespace(source, index);
            if (index >= source.Length) break;
            char charAtIndex = source[index];
            if ("(.)".Contains(charAtIndex))
            {
                yield return new Token { Text = charAtIndex.ToString(), Index = index };
                index++;
                continue;
            }
            if ("\"'".Contains(charAtIndex))
            {
                int start = index;
                char quote = source[index];
                index++;
                while (index < source.Length && source[index] != quote) index++;
                if (index < source.Length)
                {
                    yield return new Token { Text = source.Substring(start, index - start + 1), Index = start };
                    index++;
                    continue;
                }
                else
                    throw new ParseError($"Unterminated string at {new Position(source, index)}.");
            }
            Token token = ConsumeUntilWhitespace(source, index);
            yield return new Token { Text = token.Text, Index = token.Index };
            index = token.Index;
        }
    }
}
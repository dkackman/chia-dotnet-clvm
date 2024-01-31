namespace chia.dotnet.clvm;

/// <summary>
/// Represents a cons cell in a program.
/// </summary>
/// <remarks>https://en.wikipedia.org/wiki/Cons/</remarks>
public class Cons(Program item1, Program item2) : Tuple<Program, Program>(item1, item2)
{
}

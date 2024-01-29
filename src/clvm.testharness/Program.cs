using chia.dotnet.clvm;
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var puzzleProgram = chia.dotnet.clvm.Program.FromSource("()");
            var result = puzzleProgram.Compile(new CompileOptions { MaxCost = null, Strict = false });

            Console.WriteLine(result.Value.ToSource());
            Console.WriteLine(result.Value.HashHex());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
using chia.dotnet.clvm;
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var puzzleProgram = chia.dotnet.clvm.Program.FromSource("(q . \"hello world\")");
            var result = puzzleProgram.Compile();

            Console.WriteLine(result.Value);
            Console.WriteLine(result.Cost);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

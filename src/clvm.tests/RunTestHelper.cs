using chia.dotnet.clvm;
using System.Numerics;

static class RunTestHelper
{
    public static void Run(object[] input, object[] output)
    {
        var puzzle = input[0] is not null ? input[0].ToString()! : throw new Exception("Invalid input");
        var solution = input.Length > 1 && input[1] is not null ? input[1].ToString()! : "()";
        BigInteger? maxCost = input.Length > 2 && input[2] is not null ? new((long)input[2]) : null;
        var strict = input.Length > 3 && input[3] is not null && (bool)input[3];

        if (output == null || output[0] == null)
        {
            
            Assert.ThrowsAny<Exception>(() =>
            {
                var puzzleProgram = Program.FromSource(puzzle);
                var solutionProgram = Program.FromSource(solution);
                puzzleProgram.Run(solutionProgram, new RunOptions { MaxCost = maxCost, Strict = strict });
            });
        }
        else
        {
            var puzzleProgram = Program.FromSource(puzzle);
            var solutionProgram = Program.FromSource(solution);
            var result = puzzleProgram.Run(solutionProgram, new RunOptions { MaxCost = maxCost, Strict = strict });

            string text = "";
            if (output.Length > 2 && output[2] != null)
            {
                text = result.Value.SerializeHex();
            }
            else
            {
                var arg = output.Length > 3 && output[3] != null && Convert.ToBoolean(output[3]);
                text = result.Value.ToSource(arg);
            }

            var expectedResult = output[0] is not null ? output[0].ToString()! : null; ;
            Assert.Equal(expectedResult, text);

            if (output.Length > 1 && output[1] != null)
            {
                BigInteger expectedCost = new((long)output[1]);
                Assert.Equal(expectedCost - 9, result.Cost);
            }
        }
    }
}
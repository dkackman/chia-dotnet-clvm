using chia.dotnet.clvm;
using System.Numerics;

static class RunTestHelper
{
    public static void Run(object[] input, object[] output)
    {
        var puzzle = input[0] is not null ? input[0].ToString()! : throw new Exception("Invalid input");
        var solution = input.Length > 1 && input[1] is not null ? input[1].ToString()! : "()";
        BigInteger? maxCost = input.Length > 2 && input[2] is not null ? new(Convert.ToInt16(input[2])) : null;
        var strict = input.Length > 3 && input[3] is not null && Convert.ToBoolean(input[3]);

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
            //(>s (q . 0x00) (q))
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
                if (output.Length > 3 && output[3] != null)
                {
                    text = result.Value.ToSource(Convert.ToBoolean(output[3]));
                }
                else
                {
                    text = result.Value.ToSource();
                }
            }

            var expectedResult = output[0] is not null ? output[0].ToString()! : null;
            Assert.Equal(expectedResult, text);

            if (output.Length > 1 && output[1] != null)
            {
                BigInteger expectedCost = new(Convert.ToInt64(output[1]));
                Assert.Equal(expectedCost - 9, result.Cost);
            }
        }
    }
}

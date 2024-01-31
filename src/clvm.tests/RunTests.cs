using System.Numerics;
using chia.dotnet.clvm;

namespace clvm.tests;

public class RunTests
{
    public static IEnumerable<object[]> TestData =>
        [
            [new object[] {"(+)"}, new object[] { "()", 109 }],
        ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        var puzzle = input[0].ToString()!;
        var solution = input.Length > 1 ? input[0].ToString()! : "()";
        BigInteger? maxCost = input.Length > 2 ? new((int)input[2]) : null;
        bool strict = input.Length > 3 && (bool)input[2];

        if (output == null)
        {
            Assert.Throws<Exception>(() =>
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
                var arg = output.Length > 3 && output[3] != null;
                text = result.Value.ToSource(arg);
            }

            var expectedResult = output[0].ToString()!;
            Assert.Equal(expectedResult, text);

            if (output.Length > 1)
            {
                BigInteger expectedCost = new((int)output[1]);
                Assert.Equal(result.Cost, expectedCost - 9);
            }
        }
    }
}
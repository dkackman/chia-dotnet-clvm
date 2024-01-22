using System.Numerics;
using chia.dotnet.clvm;

namespace clvm.tests;

public class RunTests
{
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { "(+ (q . 7) (q . 1))", new object[] { "8", 805 } },
            new object[] { "(+ (q . 1))", new object[] { "1", 462 } },
            new object[] { "(+ ())", new object[] { "()", 473 } },
            new object[] { "(+)", new object[] { "()", 109 } },
            new object[] { "(+ (q . 0x0000000000000000000000000000000000000000000000000000000000000000000000000000000007) (q . 0x000000000000000000000000000000000000000000000000000000000000000000000001))", new object[] { "8", 1030 } },
            new object[] { "(0x0bf (q . 1) (q . 2) (q . 3))", new object[] { "()", 1962 } },
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(string puzzle, object[] output)
    {
        string solution = output[0].ToString();
        BigInteger cost = new BigInteger((int)output[1]);
        bool strict = output.Length > 2 ? (bool)output[2] : false;

        if (output == null)
        {
            Assert.Throws<Exception>(() =>
            {
                var puzzleProgram = Program.FromSource(puzzle);
                var solutionProgram = Program.FromSource(solution);
                puzzleProgram.Run(solutionProgram, new RunOptions { MaxCost = cost, Strict = strict });
            });
        }
        else
        {
            var puzzleProgram = Program.FromSource(puzzle);
            var solutionProgram = Program.FromSource(solution);
            var result = puzzleProgram.Run(solutionProgram, new RunOptions { MaxCost = cost, Strict = strict });
            string text = output.Length > 2 && (bool)output[2] ? result.Value.SerializeHex() : result.Value.ToSource(output.Length > 3 && Convert.ToBoolean(output[3].ToString()));
            Assert.Equal(text, output[0].ToString());
            if (output.Length > 1)
                Assert.Equal(result.Cost, (BigInteger)output[1] - 9);
        }
    }
}
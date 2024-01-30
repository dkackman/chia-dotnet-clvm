using System.Numerics;
using chia.dotnet.clvm;

namespace clvm.tests;

public class CompileTests
{
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { "()", "()" },
            new object[] { "(list 100 200 300)", "(100 200 300)" },
            new object[] { "(if 1 100 200)", "100" },
            new object[] { "(/ 5 2)", "2" },
            new object[] { "(mod args (f args))", "2" },
            new object[] { "(mod () (defun factorial (number) (if (> number 2) (* number (factorial (- number 1))) number)) (factorial 5))", "(q . 120)" },
            new object[] { "(mod () (defconstant something \"Hello\") something)", "(q . \"Hello\")" },
            new object[] { "(mod () (defun-inline mul (left right) (* left right)) (mul 5 10))", "(q . 50)" },
            new object[] { "(mod () (defmacro mul (left right) (* left right)) (mul 5 10))", "(q . 50)", new BigInteger(29) }
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestCompile(string puzzle, string expectedOutput, BigInteger? expectedCost = null)
    {
        var puzzleProgram = Program.FromSource(puzzle);
        var t = puzzleProgram.ToSource();
        var result = puzzleProgram.Compile(new CompileOptions { MaxCost = expectedCost });
        if (expectedOutput == null)
        {
            Assert.Throws<Exception>(() => result.Value);
        }
        else
        {
            var text = result.Value.ToSource();
            Assert.Equal(expectedOutput, text);
            if (expectedCost != null)
                Assert.Equal(expectedCost + 182, result.Cost);
        }
    }
}
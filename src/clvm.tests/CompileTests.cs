using System.Numerics;
using chia.dotnet.clvm;

namespace clvm.tests;

public class CompileTests
{
    public static IEnumerable<object[]> TestData =>
        [
            ["()", "()"],
            ["(list 100 200 300)", "(100 200 300)"],
            ["(if 1 100 200)", "100"],
            ["(/ 5 2)", "2"],
            ["(mod args (f args))", "2"],
            ["(mod () (defun factorial (number) (if (> number 2) (* number (factorial (- number 1))) number)) (factorial 5))", "(q . 120)"],
            ["(mod () (defconstant something \"Hello\") something)", "(q . \"Hello\")"],
            ["(mod () (defun-inline mul (left right) (* left right)) (mul 5 10))", "(q . 50)"],
            ["(mod () (defmacro mul (left right) (* left right)) (mul 5 10))", "(q . 50)", new BigInteger(29)]
        ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestCompile(string puzzle, string expectedOutput, BigInteger? expectedCost = null)
    {
        var puzzleProgram = Program.FromSource(puzzle);
        var result = puzzleProgram.Compile(new CompileOptions { MaxCost = null });

        var text = result.Value.ToSource();
        Assert.Equal(expectedOutput, text);
        if (expectedCost != null)
            Assert.Equal(expectedCost + 182, result.Cost);
    }

    [Fact]
    public void EnforceMaxCost()
    {
        var puzzleProgram = Program.FromSource("(mod () (defmacro mul (left right) (* left right)) (mul 5 10))");

        Assert.Throws<Exception>(() => puzzleProgram.Compile(new CompileOptions { MaxCost = 2 }));
    }
}
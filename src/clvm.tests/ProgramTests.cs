using chia.dotnet.clvm;
using Xunit.Abstractions;

namespace clvm.tests;

public class ProgramTests
{
    private readonly ITestOutputHelper _output;

    public ProgramTests(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    void EmptyProgram()
    {
        var program = Program.FromSource("()");
        Assert.Equal("()", program.ToSource());
        Assert.Equal("4bf5122f344554c53bde2ebb8cd2b7e3d1600ad631c385a5d7cce23c7785459a", program.HashHex());
    }

    [Fact]
    void SimpleFromSource()
    {
        var program = Program.FromSource("(a (opt (com 2)) 3)");
        Assert.Equal("(a (\"opt\" (\"com\" 2)) 3)", program.ToSource());
        Assert.Equal("577115c0e8d757c021cd050ced14f1d8d2861ddd01fd8e14769aa65191989545", program.HashHex());
    }

    [Fact]
    public void SimpleCompile()
    {
        var consoleOutput = new StringWriter();
        try
        {
            Console.SetOut(consoleOutput);

            var puzzleProgram = Program.FromSource("()");
            var result = puzzleProgram.Compile(new CompileOptions { MaxCost = null, Strict = false });

            Assert.Equal("()", result.Value.ToSource());
            Assert.Equal("4bf5122f344554c53bde2ebb8cd2b7e3d1600ad631c385a5d7cce23c7785459a", result.Value.HashHex());

        }
        finally
        {
            var output = consoleOutput.ToString();
            _output.WriteLine(output); // _output is an instance of ITestOutputHelper

            // Reset console output
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }

    }
}


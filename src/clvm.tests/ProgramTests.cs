
using chia.dotnet.clvm;

namespace clvm.tests;

public class ProgramTests
{
    [Fact]
    void EmptyProgram()
    {
        var program = Program.FromSource("()");
        Assert.Equal("()", program.ToSource());
        Assert.Equal("4bf5122f344554c53bde2ebb8cd2b7e3d1600ad631c385a5d7cce23c7785459a", program.HashHex());
    }

    [Fact]
    void RunProgram()
    {
        var program = Program.FromSource("(a (opt (com 2)) 3)");
        Assert.Equal("(a (opt (com 2)) 3)", program.ToSource());
        Assert.Equal("577115c0e8d757c021cd050ced14f1d8d2861ddd01fd8e14769aa65191989545", program.HashHex());
    }
}


using chia.dotnet.clvm;

namespace clvm.tests;

public class SerializeTests
{
    public static IEnumerable<object[]> TestData()
    {
        var data = new Dictionary<string, string>
        {
            { "()", "80" },
            { "(q . 1)", "ff0101" },
            { "(q . (q . ()))", "ff01ff0180" },
            { "1", "01" },
            { "0xffffabcdef", "85ffffabcdef" },
            { "\"abcdef\"", "86616263646566" },
            {
                "(f (c (q . 20) (q . 30)))",
                "ff05ffff04ffff0114ffff011e8080"
            },
            {
                "(+ 100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000 100000000000000000000000000000000000))",
                "ff10ffbd200888489af9569930925255368b25e27749a2c3a5d54a31d90d45629b2e29348d5be73f72bf489e71df64000000000000000000000000000000000000ff8f13426172c74d822b878fe80000000080"
            },
            {
                "4738294723897492387408293747389479823749238749832748932748923745987326478623874623784679283747823649832756782374732864823764872364873264832764738264873648273648723648273649273687",
                "c04a4ad5c0c0203e6553d723e4e9c6861ec58934a33f237330d166d7e5b490595f999c5ae6a01836e022ecbe7b489f0584841ef8c7bb88ec9b6c63d8d9d4459c142a42632ae01a6022f08b57"
            },
            { "((((()))))", "ffffffff8080808080" }
        };

        foreach (var kvp in data)
        {
            yield return new object[] { kvp.Key, kvp.Value
};
        }
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void SerializeTest(string input, string expectedOutput)
    {
        var puzzleProgram = Program.FromSource(input);
        var actualOutput = puzzleProgram.SerializeHex();

        Assert.Equal(expectedOutput, actualOutput, ignoreCase: true);
    }

    [Fact]
    public void Serialize_ShouldThrowException()
    {
        // This test checks for exceptions. Adjust it according to your actual exception logic.
        var input = "Your input that should cause an exception";

        var exception = Record.Exception(() =>
        {
            var puzzleProgram = Program.FromSource(input);
            puzzleProgram.Serialize();
        });

        Assert.NotNull(exception);
        // Optionally, you can assert the type of exception or its message
        // Assert.IsType<YourExpectedExceptionType>(exception);
        // Assert.Equal("Expected exception message", exception.Message);
    }
}

namespace clvm.tests;

public class RunTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
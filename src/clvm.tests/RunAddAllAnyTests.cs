namespace clvm.tests;

public class RunAddAllAnyTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // add-1
        [new object[] { "(+ (q . 7) (q . 1))" }, new object[] { "8", 805L }],
        // add-2
        [new object[] { "(+ (q . 1))" }, new object[] { "1", 462L }],
        // add-3
        [new object[] { "(+ ())" }, new object[] { "()", 473L }],
        // add-4
        [new object[] { "(+)" }, new object[] { "()", 109L }],
        // add-5
        [new object[] { "(+ (q . 0x0000000000000000000000000000000000000000000000000000000000000000000000000000000007) (q . 0x000000000000000000000000000000000000000000000000000000000000000000000001))" }, new object[] { "8", 1030L }],
        // all-1
        [new object[] { "(all (q . 1))" }, new object[] { "1", 530L }],
        // all-2
        [new object[] { "(all (q . 1) (q . (foo)))" }, new object[] { "1", 850L }],
        // all-3
        [new object[] { "(all (q . 1) (q . 1) (q . 0))" }, new object[] { "()", 1170L }],
        // any-1
        [new object[] { "(any (q . 1))" }, new object[] { "1", 530L }],
        // any-2
        [new object[] { "(any (q . 0) (q . (foo)))" }, new object[] { "1", 850L }],
        // any-3
        [new object[] { "(any (q . 0) (q . 0))" }, new object[] { "()", 850L }],
        // apply-00
        [new object[] { "(a (q . 2) (q . (3 4 5)))" }, new object[] { "3", 188L }],
        // apply-01
        [new object[] { "(a (q . 5) (q . (3 4 5)))" }, new object[] { "4", 192L }],
        // apply-02
        [new object[] { "(a (q . 11) 1)", "(5 6 7 8 9)" }, new object[] { "7", 220L }],
        // apply-03
        [new object[] { "(a)" }, new object[] { null }],
        // apply-04
        [new object[] { "(a (q . +))" }, new object[] { null }],
        // apply-05
        [new object[] { "(a (q . +) (q . (1 2 3)) (q . foo))" }, new object[] { null }],
        // apply-06
        [new object[] { "(a (q . (+ 2 5)) (q . (20 30)))" }, new object[] { "50", 996L }],
        // apply-07
        [new object[] { "(a (q . q) (q . (2 3 4)))", null, null, true }, new object[] { "(2 3 4)", 184L, null, false }],
    ];
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
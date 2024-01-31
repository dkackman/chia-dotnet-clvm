namespace clvm.tests;

public class RunDivModEqTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

    public static IEnumerable<object[]> TestData =>
    [
        // divmod-1
        [new object[] { "(divmod 2 5)", "(80001 73)" }, new object[] { "(1095 . 66)", 1280L }],
        // divmod-10
        [new object[] { "(divmod 2 5)", "(80000 -10)" }, new object[] { "(-8000)", 1270L }],
        // divmod-11
        [new object[] { "(divmod (q . 0x0000000000000000000000000000000000000000000000000000000000000013881) (q . 0x0000000000000000000000000000000000000000000000000000049))" }, new object[] { "(1095 . 66)", 1568L }],
        // divmod-12
        [new object[] { "(divmod (q . -10) (q . -7))" }, new object[] { "(1 . -3)", 1198L, null, false }],
        // divmod-13
        [new object[] { "(divmod (q . -10) (q . 7))" }, new object[] { "(-2 . 4)", 1198L, null, false }],
        // divmod-14
        [new object[] { "(divmod (q . 10) (q . -7))" }, new object[] { "(-2 . -4)", 1198L, null, false }],
        // divmod-15
        [new object[] { "(divmod (q . 10) (q . 7))" }, new object[] { "(1 . 3)", 1198L, null, false }],
        // divmod-16
        [new object[] { "(divmod (q . -10) (q . -70))" }, new object[] { "(() . -10)", 1188L, null, false }],
        // divmod-17
        [new object[] { "(divmod (q . -10) (q . 70))" }, new object[] { "(-1 . 60)", 1198L, null, false }],
        // divmod-18
        [new object[] { "(divmod (q . 10) (q . -70))" }, new object[] { "(-1 . -60)", 1198L, null, false }],
        // divmod-19
        [new object[] { "(divmod (q . 10) (q . 70))" }, new object[] { "(() . 10)", 1188L, null, false }],
        // divmod-2
        [new object[] { "(divmod 2 5)", "(-80001 73)" }, new object[] { "(-1096 . 7)", 1280L }],
        // divmod-20
        [new object[] { "(divmod (q . -100) (q . -7))" }, new object[] { "(14 . -2)", 1198L, null, false }],
        // divmod-21
        [new object[] { "(divmod (q . -100) (q . 7))" }, new object[] { "(-15 . 5)", 1198L, null, false }],
        // divmod-22
        [new object[] { "(divmod (q . 100) (q . -7))" }, new object[] { "(-15 . -5)", 1198L, null, false }],
        // divmod-23
        [new object[] { "(divmod (q . 100) (q . 7))" }, new object[] { "(14 . 2)", 1198L, null, false }],
        // divmod-24
        [new object[] { "(divmod (q . -100) (q . -70))" }, new object[] { "(1 . -30)", 1198L, null, false }],
        // divmod-25
        [new object[] { "(divmod (q . -100) (q . 70))" }, new object[] { "(-2 . 40)", 1198L, null, false }],
        // divmod-26
        [new object[] { "(divmod (q . 100) (q . -70))" }, new object[] { "(-2 . -40)", 1198L, null, false }],
        // divmod-27
        [new object[] { "(divmod (q . 100) (q . 70))" }, new object[] { "(1 . 30)", 1198L, null, false }],
        // divmod-3
        [new object[] { "(divmod 2 5)", "(80001 -73)" }, new object[] { "(-1096 . -7)", 1280L }],
        // divmod-4
        [new object[] { "(divmod 2 5)", "(-80001 -73)" }, new object[] { "(1095 . -66)", 1280L }],
        // divmod-5
        [new object[] { "(divmod 2 5)", "((200 80001) 73)" }, new object[] { null }],
        // divmod-6
        [new object[] { "(divmod 2 5)", "(80001 (200 73))" }, new object[] { null }],
        // divmod-7
        [new object[] { "(divmod 2 5)", "(80001 0)" }, new object[] { null }],
        // divmod-8
        [new object[] { "(divmod 2 5)", "(80000 10)" }, new object[] { "(8000)", 1270L }],
        // divmod-9
        [new object[] { "(divmod 2 5)", "(-80000 10)" }, new object[] { "(-8000)", 1270L }],
        // divmod-01
        [new object[] { "(divmod)" }, new object[] { null }],

        // divmod-02
        [new object[] { "(divmod (q . 1))" }, new object[] { null }],

        // divmod-03
        [new object[] { "(divmod (q . 1) (q . 1))" }, new object[] { "(1)", null, null, false }],

        // divmod-04
        [new object[] { "(divmod (q . 1) (q . 1) (q . 1))" }, new object[] { null }],

        // divmod-05
        [new object[] { "(divmod () ())" }, new object[] { null }],

        // divmod-06
        [new object[] { "(divmod (q . (1 2)) (q . (1 2)))" }, new object[] { null }],

        // divmod-07
        [new object[] { "(divmod (q . 0xffff) (q . 0xffff))" }, new object[] { "(1)", null, null, false }],

        // divmod-08
        [new object[] { "(divmod (q . 128) (q . 128))" }, new object[] { "(1)", null, null, false }],

        // divmod-09
        [new object[] { "(divmod (q . -1) (q . -1))" }, new object[] { "(1)", null, null, false }],

        // eq-01
        [new object[] { "(=)" }, new object[] { null }],

        // eq-02
        [new object[] { "(= (q . 1))" }, new object[] { null }],

        // eq-03
        [new object[] { "(= (q . 1) (q . 1))" }, new object[] { "1" }],

        // eq-04
        [new object[] { "(= (q . 1) (q . 1) (q . 1))" }, new object[] { null }],

        // eq-05
        [new object[] { "(= () ())" }, new object[] { "1" }],

        // eq-06
        [new object[] { "(= (q . (1 2)) (q . (1 2)))" }, new object[] { null }],

        // eq-07
        [new object[] { "(= (q . 0xffff) (q . 0xffff))" }, new object[] { "1" }],

        // eq-08
        [new object[] { "(= (q . 128) (q . 128))" }, new object[] { "1" }],

        // eq-09
        [new object[] { "(= (q . -1) (q . -1))" }, new object[] { "1" }],

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
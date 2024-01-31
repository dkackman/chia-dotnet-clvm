namespace clvm.tests;

public class RunConcatConsDivTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // concat-1
        [new object[] { "(concat)" }, new object[] { "()", 152L }],
        // concat-2
        [new object[] { "(concat (q . foo))" }, new object[] { "\"foo\"", 346L }],
        // concat-3
        [new object[] { "(concat (q . foo) (q . bar))" }, new object[] { "\"foobar\"", 540L }],
        // concat-4
        [new object[] { "(concat (q . foo) (q . (bar)))" }, new object[] { null }],
        // cons-1
        [new object[] { "(c)" }, new object[] { null }],
        // cons-2
        [new object[] { "(c (q . 100) (q . ()))" }, new object[] { "(100)", 100L }],
        // cons-3
        [new object[] { "(c (q . 100) (q . (200 300 400)))" }, new object[] { "(100 200 300 400)", 100L }],
        // cons-4
        [new object[] { "(c (q . 100) (q . ((500 (200 300 400)))))" }, new object[] { "(100 (500 (200 300 400)))", 100L }],
        // cons-5
        [new object[] { "(c (q . 100))" }, new object[] { null }],
        // cons-as-op-1
        [new object[] { "((c (q . (+ (q . 50) 1)) (q . 500)))" }, new object[] { null }],
        // div-1
        [new object[] { "(/ 2 5)", "(80001 73)" }, new object[] { "1095", 1134L }],
        // div-2
        [new object[] { "(/ 2 5)", "(-80001 73)" }, new object[] { "-1096", 1134L }],
        // div-3
        [new object[] { "(/ 2 5)", "(80001 -73)" }, new object[] { "-1096", 1134L }],
        // div-4
        [new object[] { "(/ 2 5)", "(80001 0)" }, new object[] { null }],
        // div-5
        [new object[] { "(/ (q . 0x00000000000000000000000000000000000000000000000000000000a) (q . 0x000000000000000000000000000000000000000000000000000000000000000000000005))" }, new object[] { "2", 1308L }],
        // div-6
        [new object[] { "(/ (q . 3) (q . 10))" }, new object[] { "()", 1046L }],
        // div-7
        [new object[] { "(/ (q . -3) (q . 10))" }, new object[] { "()", 1046L }],
        // div-8
        [new object[] { "(/ (q . 3) (q . -10))" }, new object[] { "()", 1046L }],
        // div-9
        [new object[] { "(/ (q . -3) (q . -10))" }, new object[] { "()", 1046L }],
        // concat-01
        [new object[] { "(concat)" }, new object[] { "()" }],

        // concat-02
        [new object[] { "(concat (q . 1))" }, new object[] { "1" }],

        // concat-03
        [new object[] { "(concat (q . 1) (q . 1))" }, new object[] { "257" }],

        // concat-04
        [new object[] { "(concat () ())" }, new object[] { "()" }],

        // concat-05
        [new object[] { "(concat (q . (1 2)) (q . (1 2)))" }, new object[] { null }],

        // concat-06
        [new object[] { "(concat (q . 0xffff) (q . 0xffff))" }, new object[] { "0xffffffff" }],

        // concat-07
        [new object[] { "(concat (q . 128) (q . 128))" }, new object[] { "0x00800080" }],

        // concat-08
        [new object[] { "(concat (q . -1) (q . -1))" }, new object[] { "0xffff" }],

        // concat-09
        [new object[] { "(concat () (q . -1))" }, new object[] { "-1" }],

        // concat-10
        [new object[] { "(concat (q . 1) ())" }, new object[] { "1" }],

        // concat-11
        [new object[] { "(concat (q . (1 2)) (q . 1))" }, new object[] { null }],

        // concat-12
        [new object[] { "(concat (q . 1) (q . (1 2)))" }, new object[] { null }],

        // cons-01
        [new object[] { "(c)" }, new object[] { null }],

        // cons-02
        [new object[] { "(c (q . 1))" }, new object[] { null }],

        // cons-03
        [new object[] { "(c (q . 1) (q . 1))" }, new object[] { "(1 . 1)", null, null, false }],

        // cons-04
        [new object[] { "(c (q . 1) (q . 1) (q . 1))" }, new object[] { null }],

        // cons-05
        [new object[] { "(c () ())" }, new object[] { "(())" }],

        // cons-06
        [new object[] { "(c (q . (1 2)) (q . (1 2)))" }, new object[] { "((1 2) 1 2)", null, null, false }],

        // cons-07
        [new object[] { "(c (q . 0xffff) (q . 0xffff))" }, new object[] { "(0xffff . 0xffff)" }],

        // cons-08
        [new object[] { "(c (q . 128) (q . 128))" }, new object[] { "(128 . 128)" }],

        // cons-09
        [new object[] { "(c (q . -1) (q . -1))" }, new object[] { "(-1 . -1)" }],

        // div-01
        [new object[] { "(/)" }, new object[] { null }],

        // div-02
        [new object[] { "(/ (q . 1))" }, new object[] { null }],

        // div-03
        [new object[] { "(/ (q . 1) (q . 1))" }, new object[] { "1" }],

        // div-04
        [new object[] { "(/ (q . 1) (q . 1) (q . 1))" }, new object[] { null }],

        // div-05
        [new object[] { "(/ () ())" }, new object[] { null }],

        // div-06
        [new object[] { "(/ (q . (1 2)) (q . (1 2)))" }, new object[] { null }],

        // div-07
        [new object[] { "(/ (q . 0xffff) (q . 0xffff))" }, new object[] { "1" }],

        // div-08
        [new object[] { "(/ (q . 128) (q . 128))" }, new object[] { "1" }],

        // div-09
        [new object[] { "(/ (q . -1) (q . -1))" }, new object[] { "1" }],

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
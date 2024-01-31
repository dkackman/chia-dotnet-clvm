namespace clvm.tests;

public class RunFirstGreaterHexIfTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // first-1
        [new object[] { "(f (q . (100)))" }, new object[] { "100", 60L }],
        // first-2
        [new object[] { "(f (q . (1 2 3)))" }, new object[] { "1", 60L }],
        // first-3
        [new object[] { "(f (q . ()))" }, new object[] { null }],
        // first-4
        [new object[] { "(f (f (q . ((100 200 300) 400 500))))" }, new object[] { "100", 91L }],
        // gr-s-1
        [new object[] { "(>s (q . 0x00) (q . \"\"))" }, new object[] { "1", 168L }],
        // gr-s-10
        [new object[] { "(>s (q . 0x001004) (q . (100 200)))" }, new object[] { null }],
        // gr-s-2
        [new object[] { "(>s (q . 0x01) (q . 0x00))" }, new object[] { "1", 169L }],
        // gr-s-3
        [new object[] { "(>s (q . 0x00) (q . 0x01))" }, new object[] { "()", 169L }],
        // gr-s-4
        [new object[] { "(>s (q . 0x1000) (q . 0x1001))" }, new object[] { "()", 171L }],
        // gr-s-5
        [new object[] { "(>s (q . 0x1000) (q . 0x01))" }, new object[] { "1", 170L }],
        // gr-s-6
        [new object[] { "(>s (q . 0x1000) (q . 0x1000))" }, new object[] { "()", 171L }],
        // gr-s-7
        [new object[] { "(>s (q . 0x001004) (q . 0x1005))" }, new object[] { "()", 172L }],
        // gr-s-8
        [new object[] { "(>s (q . 0x1005) (q . 0x001004))" }, new object[] { "1", 172L }],
        // gr-s-9
        [new object[] { "(>s (q . (100 200)) (q . 0x001004))" }, new object[] { null }],
        // greater-1
        [new object[] { "(> (q . 10))" }, new object[] { null }],
        // greater-10
        [new object[] { "(> (q . 0x000000000000000000000000000000000000000000000000000000000000000000493e0) (q . 0x00000000000000000000000000000000000000000000000000000000000005a))" }, new object[] { "1", 684L }],
        // greater-2
        [new object[] { "(> (q . 11) 1)", "10" }, new object[] { "1", 576L }],
        // greater-3
        [new object[] { "(> (q . 9) 1)", "10" }, new object[] { "()", 576L }],
        // greater-4
        [new object[] { "(> (q . 0) (q . 0))" }, new object[] { "()", 548L }],
        // greater-5
        [new object[] { "(> (q . (0)) (q . 0))" }, new object[] { null }],
        // greater-6
        [new object[] { "(> 3 3)" }, new object[] { null }],
        // greater-7
        [new object[] { "(> (q . 3) (q . 300))" }, new object[] { "()", 554L }],
        // greater-8
        [new object[] { "(> (q . 0x5a) (q . 0x493e0))" }, new object[] { "()", 556L }],
        // greater-9
        [new object[] { "(> (q . 0x493e0) (q . 0x5a))" }, new object[] { "1", 556L }],
        // hex-1
        [new object[] { "(q . 0x00)" }, new object[] { "0x00", 29L }],
        // hex-2
        [new object[] { "(q . 0x007eff)" }, new object[] { "0x007eff", 29L }],
        // if_1
        [new object[] { "(i (q . 100) (q . 200) (q . 300))" }, new object[] { "200", 103L }],
        // if_2
        [new object[] { "(i (q . ()) (q . 200) (q . 300))" }, new object[] { "300", 103L }],
        // if_3
        [new object[] { "(i (q . 1) (q . 200) (q . 300))" }, new object[] { "200", 103L }],
        // if_4
        [new object[] { "(i (f (r (r 1))) (f 1) (f (r 1)))", "(200 300 400)" }, new object[] { "200", 361L }],
        // if_5
        [new object[] { "(i (f (r (r 1))) (f 1) (f (r 1)))", "(200 300 1)" }, new object[] { "200", 361L }],
        // first-01
        [new object[] { "(f)" }, new object[] { null }],

        // first-02
        [new object[] { "(f (q . 1))" }, new object[] { null }],

        // first-03
        [new object[] { "(f (q . 1) (q . 1))" }, new object[] { null }],

        // first-04
        [new object[] { "(f ())" }, new object[] { null }],

        // first-05
        [new object[] { "(f (q . (1 2)))" }, new object[] { "1" }],

        // first-06
        [new object[] { "(f (q . 0xffff))" }, new object[] { null }],

        // first-07
        [new object[] { "(f (q . 128))" }, new object[] { null }],

        // first-08
        [new object[] { "(f (q . -1))" }, new object[] { null }],

        // gr-01
        [new object[] { "(>)" }, new object[] { null }],

        // gr-02
        [new object[] { "(> (q . 1))" }, new object[] { null }],

        // gr-03
        [new object[] { "(> (q . 1) (q . 1))" }, new object[] { "()" }],

        // gr-04
        [new object[] { "(> (q . 1) (q . 1) (q . 1))" }, new object[] { null }],

        // gr-05
        [new object[] { "(> () ())" }, new object[] { "()" }],

        // gr-06
        [new object[] { "(> (q . (1 2)) (q . (1 2)))" }, new object[] { null }],

        // gr-07
        [new object[] { "(> (q . 0xffff) (q . 0xffff))" }, new object[] { "()" }],

        // gr-08
        [new object[] { "(> (q . 128) (q . 128))" }, new object[] { "()" }],

        // gr-09
        [new object[] { "(> (q . -1) (q . -1))" }, new object[] { "()" }],

        // grss-01
        [new object[] { "(>s)" }, new object[] { null }],

        // grss-02
        [new object[] { "(>s (q . 1))" }, new object[] { null }],

        // grss-03
        [new object[] { "(>s (q . 1) (q . 1))" }, new object[] { "()" }],

        // grss-04
        [new object[] { "(>s (q . 1) (q . 1) (q . 1))" }, new object[] { null }],

        // grss-05
        [new object[] { "(>s () ())" }, new object[] { "()" }],

        // grss-06
        [new object[] { "(>s (q . (1 2)) (q . (1 2)))" }, new object[] { null }],

        // grss-07
        [new object[] { "(>s (q . 0xffff) (q . 0xffff))" }, new object[] { "()" }],

        // grss-08
        [new object[] { "(>s (q . 128) (q . 128))" }, new object[] { "()" }],

        // grss-09
        [new object[] { "(>s (q . -1) (q . -1))" }, new object[] { "()" }],

        // if-01
        [new object[] { "(i)" }, new object[] { null }],

        // if-02
        [new object[] { "(i (q . 1))" }, new object[] { null }],

        // if-03
        [new object[] { "(i (q . 1) (q . 1))" }, new object[] { null }],

        // if-04
        [new object[] { "(i (q . 1) (q . 1) (q . 1))" }, new object[] { "1" }],

        // if-05
        [new object[] { "(i (q . 1) (q . 1) (q . 1) (q . 1))" }, null],

        // if-06
        [new object[] { "(i () () ())" }, new object[] { "()" }],

        // if-07
        [
            new object[] { "(i (q . (1 2)) (q . (1 3)) (q . (1 4)))" },
            new object[] { "(1 3)", null, null, false }
        ],

        // if-08
        [new object[] { "(i (q . 0xffff) (q . 0xffff) (q . 0xffff))" }, new object[] { "0xffff" }],

        // if-09
        [new object[] { "(i (q . 128) (q . 128) (q . 128))" }, new object[] { "128" }],

        // if-10
        [new object[] { "(i (q . -1) (q . -1) (q . -1))" }, new object[] { "-1" }]
    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
namespace clvm.tests;

public class RunIllegalIntListLogTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // illegal-dot-expression
        [new object[] { "(q . . 0 1)" }, new object[] { null }],

        // int-0
        [new object[] { "(q . 127)" }, new object[] { "7f", null, true }],

        // int-1
        [new object[] { "(q . 128)" }, new object[] { "820080", null, true }],

        // int-2
        [new object[] { "(q . -127)" }, new object[] { "8181", null, true }],

        // int-3
        [new object[] { "(q . -128)" }, new object[] { "8180", null, true }],

        // int-4
        [new object[] { "(q . 32767)" }, new object[] { "827fff", null, true }],

        // int-5
        [new object[] { "(q . 32768)" }, new object[] { "83008000", null, true }],

        // int-6
        [new object[] { "(q . -32767)" }, new object[] { "828001", null, true }],

        // int-7
        [new object[] { "(q . -32768)" }, new object[] { "828000", null, true }],

        // list-1
        [new object[] { "(l (q . 100))" }, new object[] { "()", 49L }],

        // list-2
        [new object[] { "(l (q . (100)))" }, new object[] { "1", 49L }],

        // list-3
        [new object[] { "(l)" }, new object[] { null }],

        // list-4
        [new object[] { "(l (q . 100) (q . 200))" }, new object[] { null }],

        // list-5
        [new object[] { "(l 2)", "(50)" }, new object[] { "()", 77L }],

        // logand-1
        [new object[] { "(logand (q . 0xfffe) (q . 93))" }, new object[] { "92", 697L }],

        // logand-2
        [new object[] { "(logand (q . 13) (q . 12))" }, new object[] { "12", 694L }],

        // logand-3
        [new object[] { "(logand (q . 13) (q . 12) (q . 4))" }, new object[] { "4", 981L }],

        // logand-4
        [new object[] { "(logand)" }, new object[] { "-1", 120L }],

        // logand-5
        [new object[] { "(logand (q . 0x000000000000000000000000000000000000000000000000000000000000fffe) (q . 0x00000000000000000000000000000000000000000000000000000000000005D))" }, new object[] { "92", 880L }],

        // logand-6
        [new object[] { "(logand (q . -128) (q . 0x7ffff))" }, new object[] { "0x07ff80", 720L }],

        // logior-1
        [new object[] { "(logior (q . 12) (q . 5))" }, new object[] { "13", 694L }],

        // logior-2
        [new object[] { "(logior (q . 12) (q . 5) (q . 7))" }, new object[] { "15", 981L }],

        // logior-3
        [new object[] { "(logior (q . 0x0000000000000000000000000000000000000000000000000000000000000000000000000000c) (q . 0x00005) (q . 0x00000000000000000000000000000000000000000000000000000000000000007))" }, new object[] { "15", 1197L }],

        // logior-4
        [new object[] { "(logior (q . -128) (q . 0x7ffff))" }, new object[] { "-1", 700L }],

        // lognot-1
        [new object[] { "(lognot (q . 12))" }, new object[] { "-13", 374L }],

        // lognot-2
        [new object[] { "(lognot (q . -1))" }, new object[] { "()", 364L }],

        // lognot-3
        [new object[] { "(lognot (q . 0))" }, new object[] { "-1", 371L }],

        // lognot-4
        [new object[] { "(lognot (q . 734671943749191))" }, new object[] { "0xfd63d1dbc431b8", 452L }],

        // lognot-5
        [new object[] { "(lognot)" }, new object[] { null }],

        // lognot-6
        [new object[] { "(lognot (q . (foo)))" }, new object[] { null }],

        // lognot-7
        [new object[] { "(lognot (q . 1) (q . 2))" }, new object[] { null }],

        // lognot-8
        [
            new object[] { "(lognot (q . 0x000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001))" },
            new object[] { "-2", 14903L }
        ],

        // logxor-1
        [new object[] { "(logxor (q . 12) (q . 5))" }, new object[] { "9", 694L }],

        // logxor-2
        [new object[] { "(logxor (q . 12) (q . 5) (q . 7))" }, new object[] { "14", 981L }],

        // logxor-3
        [new object[] { "(logxor (q . 0x0000000000000000000000000000000000000000000000000000000000000000000000000c) (q . 0x00005) (q . 0x0000000000000000000000000000000000000000000000000000000000000000007))" }, new object[] { "14", 1194L }],

        // logxor-4
        [new object[] { "(logxor (q . -128) (q . 0x7ffff))" }, new object[] { "0xf8007f", 720L }],

        // list-01
        [new object[] { "(l)" }, null],

        // list-02
        [new object[] { "(l (q . 1))" }, new object[] { "()" }],

        // list-03
        [new object[] { "(l (q . 1) (q . 1))" }, null],

        // list-04
        [new object[] { "(l ())" }, new object[] { "()" }],

        // list-05
        [new object[] { "(l (q . (1 2)))" }, new object[] { "1" }],

        // list-06
        [new object[] { "(l (q . 0xffff))" }, new object[] { "()" }],

        // list-07
        [new object[] { "(l (q . 128))" }, new object[] { "()" }],

        // list-08
        [new object[] { "(l (q . -1))" }, new object[] { "()" }],

        // logand-01
        [new object[] { "(logand)" }, new object[] { "-1" }],

        // logand-02
        [new object[] { "(logand (q . 1))" }, new object[] { "1" }],

        // logand-03
        [new object[] { "(logand (q . 1) (q . 1))" }, new object[] { "1" }],

        // logand-04
        [new object[] { "(logand () ())" }, new object[] { "()" }],

        // logand-05
        [new object[] { "(logand (q . (1 2)) (q . (1 2)))" }, null],

        // logand-06
        [new object[] { "(logand (q . 0xffff) (q . 0xffff))" }, new object[] { "-1" }],

        // logand-07
        [new object[] { "(logand (q . 128) (q . 128))" }, new object[] { "128" }],

        // logand-08
        [new object[] { "(logand (q . -1) (q . -1))" }, new object[] { "-1" }],

        // logior-01
        [new object[] { "(logior)" }, new object[] { "()" }],

        // logior-02
        [new object[] { "(logior (q . 1))" }, new object[] { "1" }],

        // logior-03
        [new object[] { "(logior (q . 1) (q . 1))" }, new object[] { "1" }],

        // logior-04
        [new object[] { "(logior () ())" }, new object[] { "()" }],

        // logior-05
        [new object[] { "(logior (q . (1 2)) (q . (1 2)))" }, null],

        // logior-06
        [new object[] { "(logior (q . 0xffff) (q . 0xffff))" }, new object[] { "-1" }],

        // logior-07
        [new object[] { "(logior (q . 128) (q . 128))" }, new object[] { "128" }],

        // logior-08
        [new object[] { "(logior (q . -1) (q . -1))" }, new object[] { "-1" }],

        // lognot-01
        [new object[] { "(lognot)" }, null],

        // lognot-02
        [new object[] { "(lognot (q . 1))" }, new object[] { "-2" }],

        // lognot-03
        [new object[] { "(lognot (q . 1) (q . 1))" }, null],

        // lognot-04
        [new object[] { "(lognot ())" }, new object[] { "-1" }],

        // lognot-05
        [new object[] { "(lognot (q . (1 2)))" }, null],

        // lognot-06
        [new object[] { "(lognot (q . 0xffff))" }, new object[] { "()" }],

        // lognot-07
        [new object[] { "(lognot (q . 128))" }, new object[] { "-129" }],

        // lognot-08
        [new object[] { "(lognot (q . -1))" }, new object[] { "()" }],

        // logxor-01
        [new object[] { "(logxor)" }, new object[] { "()" }],

        // logxor-02
        [new object[] { "(logxor (q . 1))" }, new object[] { "1" }],

        // logxor-03
        [new object[] { "(logxor (q . 1) (q . 1))" }, new object[] { "()" }],

        // logxor-04
        [new object[] { "(logxor () ())" }, new object[] { "()" }],

        // logxor-05
        [new object[] { "(logxor (q . (1 2)) (q . (1 2)))" }, null],

        // logxor-06
        [new object[] { "(logxor (q . 0xffff) (q . 0xffff))" }, new object[] { "()" }],

        // logxor-07
        [new object[] { "(logxor (q . 128) (q . 128))" }, new object[] { "()" }],

        // logxor-08
        [new object[] { "(logxor (q . -1) (q . -1))" }, new object[] { "()" }]

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
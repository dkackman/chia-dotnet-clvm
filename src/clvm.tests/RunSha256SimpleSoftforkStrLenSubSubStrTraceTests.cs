namespace clvm.tests;

public class RunSha256SimpleSoftforkStrLenSubSubStrTraceTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // sha256-1
        [new object[] { "(sha256 (f 1))", "(\"hello.there.my.dear.friend\")" }, new object[] { "0x5272821c151fdd49f19cc58cf8833da5781c7478a36d500e8dc2364be39f8216", 678L }],

        // sha256-2
        [new object[] { "(sha256 (q . \"hel\") (q . \"lo.there.my.dear.friend\"))" }, new object[] { "0x5272821c151fdd49f19cc58cf8833da5781c7478a36d500e8dc2364be39f8216", 777L }],

        // sha256-3
        [new object[] { "(sha256 (f 1) (f (r 1)))", "(\"hel\" \"lo.there.my.dear.friend\")" }, new object[] { "0x5272821c151fdd49f19cc58cf8833da5781c7478a36d500e8dc2364be39f8216", 918L }],

        // sha256-4
        [new object[] { "(sha256 1)", "(hello)" }, new object[] { null }],

        // simple_add
        [new object[] { "(+ (q . 10) (q . 20))" }, new object[] { "30", 805L }],

        // softfork-1
        [new object[] { "(softfork (q . 50))" }, new object[] { "()", 80L }],

        // softfork-2
        [new object[] { "(softfork (q . 51) (q . (+ 60 50)))" }, new object[] { "()", 101L }],

        // softfork-3
        [new object[] { "(softfork (q . 3121) (q . (+ 60 50)))" }, new object[] { "()", 3171L }],

        // softfork-4
        [new object[] { "(softfork (q . 0) (q . (+ 60 50)))" }, new object[] { null }],

        // softfork-5
        [new object[] { "(softfork (q . 0x0000000000000000000000000000000000000000000000000000000000000000000000000000000000000050))" }, new object[] { "()", 110L }],

        // strlen-1
        [new object[] { "(strlen 1)", "foo-bar" }, new object[] { "7", 244L }],

        // strlen-2
        [new object[] { "(strlen 1)", "(foo-bar)" }, new object[] { null }],

        // strlen-3
        [new object[] { "(strlen 1)", "()" }, new object[] { "()", 227L }],

        // strlen-4
        [new object[] { "(strlen 1)", "\"the quick brown fox jumps over the lazy dogs\"" }, new object[] { "44", 281L }],

        // sub-1
        [new object[] { "(- (q . 7) (q . 1))" }, new object[] { "6", 805L }],

        // sub-2
        [new object[] { "(- (q . 1))" }, new object[] { "1", 462L }],

        // sub-3
        [new object[] { "(- ())" }, new object[] { "()", 473L }],

        // sub-4
        [new object[] { "(-)" }, new object[] { "()", 109L }],

        // sub-5
        [new object[] { "(- (q . 0x0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007) (q . 0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001))" }, new object[] { "6", 1144L }],

        // substr-00
        [new object[] { "(substr (q . \"abcdefghijkl\") (q . 14))" }, new object[] { null }],

        // substr-01
        [new object[] { "(substr (q . \"abcdefghijkl\") (q . 0))" }, new object[] { "\"abcdefghijkl\"", 51L }],

        // substr-02
        [new object[] { "(substr (q . \"abcdefghijkl\") (q . -1))" }, new object[] { null }],

        // substr-03
        [new object[] { "(substr (q . \"abcdefghijkl\") (q . 12))" }, new object[] { "()", 51L }],

        // substr-04
        [new object[] { "(substr (q . \"abcdefghijkl\") (q . 11))" }, new object[] { "108", 51L }],

        // substr-05
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(0 4)" }, new object[] { "\"abcd\"", 131L }],

        // substr-06
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(0 12)" }, new object[] { "\"abcdefghijkl\"", 131L }],

        // substr-07
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(-1 12)" }, new object[] { null }],

        // substr-08
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(0 13)" }, new object[] { null }],

        // substr-09
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(10 10)" }, new object[] { "()", 131L }],

        // substr-10
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(10 9)" }, new object[] { null }],

        // substr-11
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(1 4)" }, new object[] { "\"bcd\"", 131L }],

        // substr-12
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(8 12)" }, new object[] { "\"ijkl\"", 131L }],

        // substr-13
        [new object[] { "(substr (q . (\"abcdefghijkl\")) 2 5)", "(0 4)" }, new object[] { null }],

        // substr-14
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "((0) 4)" }, new object[] { null }],

        // substr-15
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(0 (4))" }, new object[] { null }],

        // substr-16
        [new object[] { "(substr (q . \"abcdefghijkl\") (q . 0x000000000000000000000000000000000000000000000000000000000000000002) (q . 0x0000000000000000000000000000000000000000000000000000000000000005))" }, new object[] { null }],

        // substr-17
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(0 -1)" }, new object[] { null }],

        // substr-18
        [new object[] { "(substr (q . \"abcdefghijkl\") 2 5)", "(4294967297 3)" }, new object[] { null }],

        // trace-1
        [new object[] { "(+ (q . 10) (f 1))", "(51)" }, new object[] { "61", 860L }],

        // trace-2
        [new object[] { "(x)" }, new object[] { null }],

        // sha256-01
        [ new object[] { "(sha256)" }, new object[] { "0xe3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855" } ],

        // sha256-02
        [ new object[] { "(sha256 (q . 1))" }, new object[] { "0x4bf5122f344554c53bde2ebb8cd2b7e3d1600ad631c385a5d7cce23c7785459a" } ],

        // sha256-03
        [ new object[] { "(sha256 (q . 1) (q . 1))" },
            new object[] { "0x9dcf97a184f32623d11a73124ceb99a5709b083721e878a16d78f596718ba7b2" } ],

        // sha256-04
        [ new object[] { "(sha256 () ())" }, new object[] { "0xe3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855" } ],

        // sha256-05
        [new object[] { "(sha256 (q . (1 2)) (q . (1 2)))" }, null],

        // sha256-06
        [ new object[] { "(sha256 (q . 0xffff) (q . 0xffff))" }, new object[] { "0xad95131bc0b799c0b1af477fb14fcf26a6a9f76079e48bf090acb7e8367bfd0e" } ],

        // sha256-07
        [ new object[] { "(sha256 (q . 128) (q . 128))" }, new object[] { "0xda60b92bc70e999c07a6ded180a16c1e801e89a5722b565ea242d6aff2f507d8" } ],

        // sha256-08
        [ new object[] { "(sha256 (q . -1) (q . -1))" }, new object[] { "0xca2fd00fa001190744c15c317643ab092e7048ce086a243e2be9437c898de1bb" } ],

        // softfork-01
        [new object[] { "(softfork)" }, null],

        // softfork-02
        [ new object[] { "(softfork (q . 1))" }, new object[] { "()", 31 } ],

        // softfork-03
        [ new object[] { "(softfork (q . 1) (q . 1))" }, new object[] { "()", 51 } ],

        // softfork-04
        [new object[] { "(softfork ())" }, null],

        // softfork-05
        [new object[] { "(softfork (q . (1 2)))" }, null],

        // softfork-06
        [new object[] { "(softfork (q . 0xffff))" }, null],

        // softfork-07
        [ new object[] { "(softfork (q . 128))" },
            new object[] { "()", 158 } ],

        // softfork-08
        [new object[] { "(softfork (q . -1))" }, null],

        // strlen-01
        [new object[] { "(strlen)" }, null],

        // strlen-02
        [ new object[] { "(strlen (q . 1))" },
            new object[] { "1" } ],

        // strlen-03
        [new object[] { "(strlen (q . 1) (q . 1))" }, null],

        // strlen-04
        [ new object[] { "(strlen ())" },            new object[] { "()" } ],

        // strlen-05
        [new object[] { "(strlen (q . (1 2)))" }, null],

        // strlen-06
        [ new object[] { "(strlen (q . 0xffff))" }, new object[] { "2" } ],

        // strlen-07
        [ new object[] { "(strlen (q . 128))" }, new object[] { "2" } ],

        // strlen-08
        [ new object[] { "(strlen (q . -1))" }, new object[] { "1" } ],

        // sub-01
        [ new object[] { "(-)" }, new object[] { "()" } ],

        // sub-02
        [ new object[] { "(- (q . 1))" }, new object[] { "1" } ],

        // sub-03
        [ new object[] { "(- (q . 1) (q . 1))" }, new object[] { "()" } ],

        // sub-04
        [ new object[] { "(- () ())" }, new object[] { "()" } ],

        // sub-05
        [new object[] { "(- (q . (1 2)) (q . (1 2)))" }, null],

        // sub-06
        [ new object[] { "(- (q . 0xffff) (q . 0xffff))" }, new object[] { "()" } ],

        // sub-07
        [ new object[] { "(- (q . 128) (q . 128))" }, new object[] { "()" } ],

        // sub-08
        [ new object[] { "(- (q . -1) (q . -1))" }, new object[] { "()" } ],

        // substr-01
        [new object[] { "(substr)" }, null],

        // substr-02
        [new object[] { "(substr (q . 1))" }, null],

        // substr-03
        [ new object[] { "(substr (q . 1) (q . 1))" }, new object[] { "()", null, null, false } ],

        // substr-04
        [ new object[] { "(substr (q . 1) (q . 1) (q . 1))" }, new object[] { "()" } ],

        // substr-05
        [new object[] { "(substr (q . 1) (q . 1) (q . 1) (q . 1))" }, null],

        // substr-06
        [ new object[] { "(substr () () ())" }, new object[] { "()" } ],

        // substr-07
        [new object[] { "(substr (q . (1 2)) (q . (1 2)) (q . (1 2)))" }, null],

        // substr-08
        [new object[] { "(substr (q . 0xffff) (q . 0xffff) (q . 0xffff))" }, null],

        // substr-09
        [new object[] { "(substr (q . 128) (q . 128) (q . 128))" }, null],

        // substr-10
        [new object[] { "(substr (q . -1) (q . -1) (q . -1))" }, null]

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
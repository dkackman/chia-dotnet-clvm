namespace clvm.tests;

public class RunPubkeyQuoteRaiseRestTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // pubkey-for-exp-1
        [new object[] { "(pubkey_for_exp (q . 1))" }, new object[] { "0x97f1d3a73197d7942695638c4fa9ac0fc3688c4f9774b905a14e3a3f171bac586c55e83ff97a1aeffb3af00adb22c6bb", 1326278L }],

        // pubkey-for-exp-2
        [new object[] { "(pubkey_for_exp (q . 2))" }, new object[] { "0xa572cbea904d67468808c8eb50a9450c9721db309128012543902d0ac358a62ae28f75bb8f1c7c42c39a8c5529bf0f4e", 1326278L }],

        // pubkey-for-exp-3
        [new object[] { "(pubkey_for_exp (q . 3))" }, new object[] { "0x89ece308f9d1f0131765212deca99697b112d61f9be9a5f1f3780a51335b3ff981747a0b2ca2179b96d2c0c9024e5224", 1326278L }],

        // pubkey-for-exp-4
        [new object[] { "(pubkey_for_exp (q . -2))" }, new object[] { "0x8572cbea904d67468808c8eb50a9450c9721db309128012543902d0ac358a62ae28f75bb8f1c7c42c39a8c5529bf0f4e", 1326278L }],

        // pubkey-for-exp-5
        [new object[] { "(pubkey_for_exp (q . 5))" }, new object[] { "0xb0e7791fb972fe014159aa33a98622da3cdc98ff707965e536d8636b5fcc5ac7a91a8c46e59a00dca575af0f18fb13dc", 1326278L }],

        // quote-1
        [new object[] { "(q . 0)" }, new object[] { "()", 29L }],

        // quote-2
        [new object[] { "(q . 0 1)" }, new object[] { null }],

        // quote-3
        [new object[] { "(q . 0)" }, new object[] { "()", 29L }],

        // quote-4
        [new object[] { "(q . (0 1 (f (a)) (q . 15 20)))" }, new object[] { null }],

        // quote-5
        [new object[] { "(q)" }, new object[] { "()", 29L }],

        // quote-6
        [new object[] { "(q . 0 1)" }, new object[] { null }],

        // quote-7
        [new object[] { "(q 0 1)" }, new object[] { "(() 1)", 29L }],

        // quote-8
        [new object[] { "(q . )" }, new object[] { null }],

        // quote-explicit-list
        [new object[] { "(q . (\"A\" \"B\"))" }, new object[] { "(65 66)" }],

        // quote-implicit-list
        [new object[] { "(q \"A\" \"B\")" }, new object[] { "(65 66)" }],

        // raise-1
        [new object[] { "(x (q . 2000))" }, new object[] { null }],

        // raise-2
        [new object[] { "(x (q . 2000))" }, new object[] { null }],

        // raise-3
        [new object[] { "(x (q . (100)) (q . (200)) (q . (300)))" }, new object[] { null }],

        // rest-1
        [new object[] { "(r (q . (100)))" }, new object[] { "()", 60L }],

        // rest-2
        [new object[] { "(r (q . (100 200 300)))" }, new object[] { "(200 300)", 60L }],

        // rest-3
        [new object[] { "(r (q . ()))" }, new object[] { null }],

        // rest-4
        [new object[] { "(r (r (q . ((100 200 300) 400 500))))" }, new object[] { "(500)", 91L }],

        // pubkey_for_exp-01
        [new object[] { "(pubkey_for_exp)" }, null],

        // pubkey_for_exp-02
        [ new object[] { "(pubkey_for_exp (q . 1))" },
            new object[] { "0x97f1d3a73197d7942695638c4fa9ac0fc3688c4f9774b905a14e3a3f171bac586c55e83ff97a1aeffb3af00adb22c6bb" } ],

        // pubkey_for_exp-03
        [new object[] { "(pubkey_for_exp (q . 1) (q . 1))" }, null],

        // pubkey_for_exp-04
        [ new object[] { "(pubkey_for_exp ())" },
            new object[] { "0xc00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" } ],

        // pubkey_for_exp-05
        [new object[] { "(pubkey_for_exp (q . (1 2)))" }, null],

        // pubkey_for_exp-06
        [ new object[] { "(pubkey_for_exp (q . 0xffff))" },
            new object[] { "0xb7f1d3a73197d7942695638c4fa9ac0fc3688c4f9774b905a14e3a3f171bac586c55e83ff97a1aeffb3af00adb22c6bb" } ],

        // pubkey_for_exp-07
        [ new object[] { "(pubkey_for_exp (q . 128))" },
            new object[] { "0x8b737f47d5b2794819b5dc01236895e684f1406f8b9f0d9aa06b5fb36dba6c185efec755b77d9424d09b848468127559" } ],

        // pubkey_for_exp-08
        [ new object[] { "(pubkey_for_exp (q . -1))" },
            new object[] { "0xb7f1d3a73197d7942695638c4fa9ac0fc3688c4f9774b905a14e3a3f171bac586c55e83ff97a1aeffb3af00adb22c6bb" } ],

        // quote-01
        [new object[] { "(q)" }, new object[] { "()" }],

        // quote-02
        [new object[] { "(q . (q . 1))" }, new object[] { "(q . 1)" }],

        // quote-03
        [new object[] { "(q . (q . 1) (q . 1))" }, null],

        // quote-04
        [new object[] { "(q . ())" }, new object[] { "()" }],

        // quote-05
        [new object[] { "(q . (q . (1 2)))" }, new object[] { "(q 1 2)" }],

        // quote-06
        [ new object[] { "(q . (q . 0xffff))" }, new object[] { "(1 . 0xffff)", null, null, false } ],

        // quote-07
        [new object[] { "(q . (q . 128))" }, new object[] { "(1 . 128)", null, null, false }],

        // quote-08
        [new object[] { "(q . (q . -1))" }, new object[] { "(1 . -1)", null, null, false }],

        // raise-01
        [new object[] { "(x)" }, null],

        // raise-02
        [new object[] { "(x (q . 1))" }, null],

        // raise-03
        [new object[] { "(x (q . 1) (q . 1))" }, null],

        // raise-04
        [new object[] { "(x ())" }, null],

        // raise-05
        [new object[] { "(x (q . (1 2)) (q . (1 2)))" }, null],

        // raise-06
        [new object[] { "(x (q . 0xffff) (q . 0xffff))" }, null],

        // raise-07
        [new object[] { "(x (q . 128) (q . 128))" }, null],

        // raise-08
        [new object[] { "(x (q . -1) (q . -1))" }, null],

        // rest-01
        [new object[] { "(r)" }, null],

        // rest-02
        [new object[] { "(r (q . 1))" }, null],

        // rest-03
        [new object[] { "(r (q . 1) (q . 1))" }, null],

        // rest-04
        [new object[] { "(r ())" }, null],

        // rest-05
        [new object[] { "(r (q . (1 2)))" }, new object[] { "(2)", null, null, false }],

        // rest-06
        [new object[] { "(r (q . 0xffff))" }, null],

        // rest-07
        [new object[] { "(r (q . 128))" }, null],

        // rest-08
        [new object[] { "(r (q . -1))" }, null]

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
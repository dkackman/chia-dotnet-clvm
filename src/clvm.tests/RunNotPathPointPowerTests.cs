namespace clvm.tests;

public class RunNotPathPointPowerTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // not-1
        [new object[] { "(not (q . 0))" }, new object[] { "1", 230L }],

        // not-2
        [new object[] { "(not (q . 1))" }, new object[] { "()", 230L }],

        // not-3
        [new object[] { "(not (q . (foo bar)))" }, new object[] { "()", 230L }],

        // path-1
        [new object[] { "1", "1" }, new object[] { "1", 53L }],

        // path-10
        [new object[] { "0xD7", "(0x1337 . (0x1337 . (0x1337 . ((0x1337 . ((0x1337 . 42) . 0x1337)) . 0x1337))))" }, new object[] { "42", 81L, null, false }],

        // path-11
        [new object[] { "0x000000000000000000000000" }, new object[] { "()", 101L, null, false }],

        // path-2
        [new object[] { "44", "(((0x1337 . (0x1337 . (42 . 0x1337))) . 0x1337) . 0x1337)" }, new object[] { "42", 73L, null, false }],

        // path-3
        [new object[] { "7708975405620101644641102810267383005", "(0x1337 . ((0x1337 . (0x1337 . (0x1337 . ((0x1337 . (0x1337 . (((0x1337 . ((0x1337 . (0x1337 . (0x1337 . (0x1337 . (0x1337 . ((0x1337 . (0x1337 . ((0x1337 . (((0x1337 . (0x1337 . (0x1337 . ((0x1337 . (((0x1337 . (((0x1337 . (0x1337 . (0x1337 . (0x1337 . ((0x1337 . ((0x1337 . (((((0x1337 . ((0x1337 . ((0x1337 . (0x1337 . (0x1337 . (((0x1337 . (0x1337 . ((0x1337 . (0x1337 . ((((0x1337 . (0x1337 . (0x1337 . (0x1337 . (((((0x1337 . (0x1337 . (0x1337 . (0x1337 . (0x1337 . (((((0x1337 . (((((0x1337 . ((0x1337 . (0x1337 . ((((0x1337 . ((((0x1337 . ((0x1337 . ((0x1337 . ((0x1337 . (0x1337 . (0x1337 . ((((0x1337 . (0x1337 . ((0x1337 . (((0x1337 . (0x1337 . (((0x1337 . (0x1337 . (0x1337 . (42 . 0x1337)))) . 0x1337) . 0x1337))) . 0x1337) . 0x1337)) . 0x1337))) . 0x1337) . 0x1337) . 0x1337)))) . 0x1337)) . 0x1337)) . 0x1337)) . 0x1337) . 0x1337) . 0x1337)) . 0x1337) . 0x1337) . 0x1337))) . 0x1337)) . 0x1337) . 0x1337) . 0x1337) . 0x1337)) . 0x1337) . 0x1337) . 0x1337) . 0x1337)))))) . 0x1337) . 0x1337) . 0x1337) . 0x1337))))) . 0x1337) . 0x1337) . 0x1337))) . 0x1337))) . 0x1337) . 0x1337)))) . 0x1337)) . 0x1337)) . 0x1337) . 0x1337) . 0x1337) . 0x1337)) . 0x1337)) . 0x1337))))) . 0x1337) . 0x1337)) . 0x1337) . 0x1337)) . 0x1337)))) . 0x1337) . 0x1337)) . 0x1337))) . 0x1337)))))) . 0x1337)) . 0x1337) . 0x1337))) . 0x1337)))) . 0x1337))" }, new object[] { "42", 541L, null, false }],

        // path-4
        [new object[] { "1", "(((0x1337 . (0x1337 . (42 . 0x1337))) . 0x1337) . 0x1337)" }, new object[] { "(((4919 4919 42 . 4919) . 4919) . 4919)", 53L, null, false }],

        // path-5
        [new object[] { "0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001", "(((0x1337 . (0x1337 . (42 . 0x1337))) . 0x1337) . 0x1337)" }, new object[] { "(((4919 4919 42 . 4919) . 4919) . 4919)", 545L, null, false }],

        // path-6
        [new object[] { "56800", "((((((0x1337 . (0x1337 . (0x1337 . (0x1337 . ((0x1337 . (0x1337 . (0x1337 . ((0x1337 . 42) . 0x1337)))) . 0x1337))))) . 0x1337) . 0x1337) . 0x1337) . 0x1337) . 0x1337)" }, new object[] { "42", 117L, null, false }],

        // path-7
        [new object[] { "4012287723", "(0x1337 . (0x1337 . ((0x1337 . ((0x1337 . (0x1337 . (0x1337 . ((0x1337 . (0x1337 . (((0x1337 . ((0x1337 . ((0x1337 . (0x1337 . (((0x1337 . (((0x1337 . (0x1337 . (0x1337 . (0x1337 . ((0x1337 . (0x1337 . 42)) . 0x1337))))) . 0x1337) . 0x1337)) . 0x1337) . 0x1337))) . 0x1337)) . 0x1337)) . 0x1337) . 0x1337))) . 0x1337)))) . 0x1337)) . 0x1337)))" }, new object[] { "42", 181L, null, false }],

        // path-8
        [new object[] { "0x0000C8C141AB3121E776", "((0x1337 . (0x1337 . ((0x1337 . (0x1337 . (0x1337 . ((0x1337 . (0x1337 . (0x1337 . (((0x1337 . (0x1337 . (0x1337 . (0x1337 . (((((0x1337 . (((0x1337 . ((((0x1337 . (0x1337 . (((0x1337 . (0x1337 . ((0x1337 . ((0x1337 . ((0x1337 . (0x1337 . ((((((0x1337 . ((0x1337 . ((((((0x1337 . (0x1337 . ((((0x1337 . (((0x1337 . 42) . 0x1337) . 0x1337)) . 0x1337) . 0x1337) . 0x1337))) . 0x1337) . 0x1337) . 0x1337) . 0x1337) . 0x1337)) . 0x1337)) . 0x1337) . 0x1337) . 0x1337) . 0x1337) . 0x1337))) . 0x1337)) . 0x1337)) . 0x1337))) . 0x1337) . 0x1337))) . 0x1337) . 0x1337) . 0x1337)) . 0x1337) . 0x1337)) . 0x1337) . 0x1337) . 0x1337) . 0x1337))))) . 0x1337) . 0x1337)))) . 0x1337)))) . 0x1337))) . 0x1337)" }, new object[] { "42", 313L, null, false }],

        // path-9
        [new object[] { "0x00D7", "(0x1337 . (0x1337 . (0x1337 . ((0x1337 . ((0x1337 . 42) . 0x1337)) . 0x1337))))" }, new object[] { "42", 85L, null, false }],

        // point-add-1
        [new object[] { "(point_add (pubkey_for_exp (q . 1)) (pubkey_for_exp (q . 2)))" }, new object[] { "0x89ece308f9d1f0131765212deca99697b112d61f9be9a5f1f3780a51335b3ff981747a0b2ca2179b96d2c0c9024e5224", 5442082L }],

        // point-add-2
        [new object[] { "(= (point_add (pubkey_for_exp (q . 2)) (pubkey_for_exp (q . 3))) (pubkey_for_exp (q . 5)))" }, new object[] { "1", 6768565L }],

        // point-add-3
        [new object[] { "(= (point_add (pubkey_for_exp (q . -2)) (pubkey_for_exp (q . 5))) (pubkey_for_exp (q . 3)))" }, new object[] { "1", 6768565L }],

        // power-1
        [
            new object[] { "(a (q . (a 2 (c 2 (c 5 (c 11 (q . ())))))) (c (q . (a (i (= 11 (q . ())) (q . (q . 1)) (q . (* 5 (a 2 (c 2 (c 5 (c (- 11 (q . 1)) (q . ())))))))) 1)) 1))", "(5033 1000)" }, 
            new object[] { "0x024d4f505f1f813ca5e0ae8805bad8707347e65c5f7595da4852be5074288431d1df11a0c326d249f1f52ee051579403d1d0c23a7a1e9af18b7d7dc4c63c73542863c434ae9dfa80141a30cf4acee0d6c896aa2e64ea748404427a3bdaa1b97e4e09b8f5e4f8e9c568a4fc219532dbbad5ec54476d19b7408f8e7e7df16b830c20a1e83d90cc0620b0677b7606307f725539ef223561cdb276baf8e92156ee6492d97159c8f64768349ea7e219fd07fa818a59d81d0563b140396402f0ff758840da19808440e0a57c94c48ef84b4ab7ca8c5f010b69b8f443b12b50bd91bdcf2a96208ddac283fa294d6a99f369d57ab41d03eab5bb4809223c141ad94378516e6766a5054e22e997e260978af68a86893890d612f081b40d54fd1e940af35c0d7900c9a917e2458a61ef8a83f7211f519b2c5f015dfa7c2949ef8bedd02d3bad64ca9b2963dc2bb79f24092331133a7a299872079b9d0422b8fc0eeba4e12c7667ac7282cc6ff98a7c670614c9fce5a061b8d5cd4dd3c6d62d245688b62f9713dc2604bdd5bbc85c070c51f784a9ebac0e0eaa2e29e82d93e570887aa7e1a9d25baf0b2c55a4615f35ec0dbe9baa921569700f95e10cd2d4f6ba152a2ac288c37b60980df33dadfa920fd43dbbf55a0b333b88a3237d954e33d80ed6582019faf51db5f1b52e392559323f8bdd945e7fc6cb8f97f2b8417cfc184d7bfbfa5314d4114f95b725847523f1848d13c28ad96662298ee4e2d87af23e7cb4e58d7a20a5c57ae6833b4a37dcafccca0245a0d6ef28f83200d74db390281e03dd3a8b782970895764c3fcef31c5ed6d0b6e4e796a62ad5654691eea0d9db351cc4fee63248405b24c98bd5e68e4a5e0ab11e90e3c7de270c594d3a35639d931853b7010c8c896f6b28b2af719e53da65da89d44b926b6f06123c9217a43be35d751516bd02c18c4f868a2eae78ae3c6deab1115086c8ce58414db4561865d17ab95c7b3d4e1bfc6d0a4d3fbf5f20a0a7d77a9270e4da354c588da55b0063aec76654019ffb310e1503d99a7bc81ccdf5f8b15c8638156038624cf35988d8420bfdb59184c4b86bf5448df65c44aedc2e98eead7f1ba4be8f402baf12d41076b8f0991cfc778e04ba2c05d1440c70488ffaeefde537064035037f729b683e8ff1b3d0b4aa26a2b30bcaa9379f7fcc7072ff9a2c3e801c5979b0ab3e7acf89373de642d596f26514b9fa213ca217181a8429ad69d14445a822b16818c2509480576dc0ff7bac48c557e6d1883039f4daf873fa4f9a4d849130e2e4336049cfaf9e69a7664f0202b901cf07c7065c4dc93c46f98c5ea5c9c9d911b733093490da3bf1c95f43cd18b7be3798535a55ac6da3442946a268b74bde1349ca9807c41d90c7ec218a17efd2c21d5fcd720501f8a488f1dfba0a423dfdb2a877707b77930e80d734ceabcdb24513fad8f2e2470604d041df083bf184edd0e9720dd2b608b1ee1df951d7ce8ec671317b4f5a3946aa75280658b4ef77b3f504ce73e7ecac84eec3c2b45fb62f6fbd5ab78c744abd3bf5d0ab37d7b19124d2470d53db09ddc1f9dd9654b0e6a3a44c95d0a5a5e061bd24813508d3d1c901544dc3e6b84ca38dd2fde5ea60a57cbc12428848c4e3f6fd4941ebd23d709a717a090dd01830436659f7c20fd2d70c916427e9f3f12ac479128c2783f02a9824aa4e31de133c2704e049a50160f656e28aa0a2615b32bd48bb5d5d13d363a487324c1e9b8703be938bc545654465c9282ad5420978263b3e3ba1bb45e1a382554ac68e5a154b896c9c4c2c3853fbbfc877c4fb7dc164cc420f835c413839481b1d2913a68d206e711fb19b284a7bb2bd2033531647cf135833a0f3026b0c1dc0c184120d30ef4865985fdacdfb848ab963d2ae26a784b7b6a64fdb8feacf94febed72dcd0a41dc12be26ed79af88f1d9cba36ed1f95f2da8e6194800469091d2dfc7b04cfe93ab7a7a888b2695bca45a76a1458d08c3b6176ab89e7edc56c7e01142adfff944641b89cd5703a911145ac4ec42164d90b6fcd78b39602398edcd1f935485894fb8a1f416e031624806f02fbd07f398dbfdd48b86dfacf2045f85ecfe5bb1f01fae758dcdb4ae3b1e2aac6f0878f700d1f430b8ca47c9d8254059bd5c006042c4605f33ca98b41", 15025126L }
        ],

        // not-01
        [new object[] { "(not)" }, null],

        // not-02
        [new object[] { "(not (q . 1))" }, new object[] { "()" }],

        // not-03
        [new object[] { "(not (q . 1) (q . 1))" }, null],

        // not-04
        [new object[] { "(not ())" }, new object[] { "1" }],

        // not-05
        [new object[] { "(not (q . (1 2)))" }, new object[] { "()" }],

        // not-06
        [new object[] { "(not (q . 0xffff))" }, new object[] { "()" }],

        // not-07
        [new object[] { "(not (q . 128))" }, new object[] { "()" }],

        // not-08
        [new object[] { "(not (q . -1))" }, new object[] { "()" }],

        // point_add-01
        [
            new object[] { "(point_add)" },
            new object[] { "0xc00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" }
        ],

        // point_add-02
        [new object[] { "(point_add (q . 1))" }, null],

        // point_add-03
        [new object[] { "(point_add (q . 1) (q . 1))" }, null],

        // point_add-04
        [new object[] { "(point_add () ())" }, null],

        // point_add-05
        [new object[] { "(point_add (q . (1 2)) (q . (1 2)))" }, null],

        // point_add-06
        [new object[] { "(point_add (q . 0xffff) (q . 0xffff))" }, null],

        // point_add-07
        [new object[] { "(point_add (q . 128) (q . 128))" }, null],

        // point_add-08
        [new object[] { "(point_add (q . -1) (q . -1))" }, null]

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}
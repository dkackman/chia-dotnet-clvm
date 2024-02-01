namespace clvm.tests;

public class RunUnknownTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public static IEnumerable<object[]> TestData =>
    [
        // unknown-0
        [new object[] { "(a (q 0x00ffffffffffffffffffff00) (q ()))" }, new object[] { null }],
        // unknown-1
        [new object[] { "(a (q 0xfffffffff) (q ()))" }, new object[] { null }],
        // unknown-2
        [new object[] { "(a (q 0xff) (q 1))", null, null, true }, new object[] { null }],
        // unknown-00
        [new object[] { "(0x000 )", null, null, true }, null],
        // unknown-01
        [ new object[] { "(0x000 )" }, new object[] { "()", 11L } ],
        // unknown-02
        [new object[] { "(0x03f )" }, null],
        // unknown-03
        [ new object[] { "(0x03f )" },
            new object[] { "()", 11L } ],
        // unknown-04
        [new object[] { "(0x100 )" }, null],
        // unknown-05
        [ new object[] { "(0x100 )" },
            new object[] { "()", 12L } ],
        // unknown-06
        [new object[] { "(0x13f )" }, null],
        // unknown-07
        [ new object[] { "(0x13f )" },
            new object[] { "()", 12L } ],
        // unknown-08
        [new object[] { "(0xfffeffff00 )" }, null],
        // unknown-09
        [ new object[] { "(0xfffeffff00 )" },
            new object[] { "()", 4294901770L } ],
        // unknown-10
        [new object[] { "(0xfffeffff3f )" }, null],
        // unknown-100
        [new object[] { "(0x180 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-101
        [ new object[] { "(0x180 (q . 1) (q . 2) (q . 3))" },
            new object[] { "()", 3854L } ],
        // unknown-102
        [new object[] { "(0x1bf (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-103
        [ new object[] { "(0x1bf (q . 1) (q . 2) (q . 3))" },
            new object[] { "()", 3854L } ],
        // unknown-104
        [new object[] { "(0xfffeffff80 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-105
        [new object[] { "(0xfffeffff80 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-106
        [new object[] { "(0xfffeffffbf (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-107
        [new object[] { "(0xfffeffffbf (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-108
        [new object[] { "(0x7ffffffff80 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-109
        [new object[] { "(0x7ffffffff80 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-11
        [ new object[] { "(0xfffeffff3f )" },
            new object[] { "()", 4294901770L } ],
        // unknown-110
        [new object[] { "(0x7ffffffffbf (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-111
        [new object[] { "(0x7ffffffffbf (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-112
        [new object[] { "(0x0c0 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-113
        [ new object[] { "(0x0c0 (q . 1) (q . 2) (q . 3))" },
            new object[] { "()", 626L } ],
        // unknown-114
        [new object[] { "(0x0ff (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-115
        [ new object[] { "(0x0ff (q . 1) (q . 2) (q . 3))" },
            new object[] { "()", 626L } ],
        // unknown-116
        [new object[] { "(0x1c0 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-117
        [ new object[] { "(0x1c0 (q . 1) (q . 2) (q . 3))" },
            new object[] { "()", 1182L } ],
        // unknown-118
        [new object[] { "(0x1ff (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-119
        [ new object[] { "(0x1ff (q . 1) (q . 2) (q . 3))" },
            new object[] { "()", 1182L } ],
        // unknown-12
        [new object[] { "(0x7ffffffff00 )" }, null],
        // unknown-120
        [new object[] { "(0xfffeffffc0 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-121
        [new object[] { "(0xfffeffffc0 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-122
        [new object[] { "(0xfffeffffff (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-123
        [new object[] { "(0xfffeffffff (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-124
        [new object[] { "(0x7ffffffffc0 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-125
        [new object[] { "(0x7ffffffffc0 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-126
        [new object[] { "(0x7ffffffffff (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-127
        [new object[] { "(0x7ffffffffff (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-128
        [new object[] { "(0x000 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-129
        [ new object[] { "(0x000 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" },
            new object[] { "()", 51 } ],
        // unknown-13
        [new object[] { "(0x7ffffffff00 )" }, null],
        // unknown-130
        [new object[] { "(0x03f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-131
        [ new object[] { "(0x03f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" },
            new object[] { "()", 51 } ],
        // unknown-132
        [new object[] { "(0x100 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-133
        [ new object[] { "(0x100 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" },
            new object[] { "()", 52 } ],
        // unknown-134 (continued)
        [new object[] { "(0x13f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-135
        [ new object[] { "(0x13f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" },
            new object[] { "()", 52 } ],
        // unknown-136
        [new object[] { "(0xfffeffff00 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-137 (continued)
        [ new object[] { "(0xfffeffff00 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" },
            new object[] { "()", 4294901810L } ],
        // unknown-138
        [new object[] { "(0xfffeffff3f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-139
        [ new object[] { "(0xfffeffff3f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" },
            new object[] { "()", 4294901810L } ],
        // unknown-14
        [new object[] { "(0x7ffffffff3f )", null, null, true }, null],
        // unknown-140
        [new object[] { "(0x7ffffffff00 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-141
        [new object[] { "(0x7ffffffff00 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-142
        [new object[] { "(0x7ffffffff3f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-143
        [new object[] { "(0x7ffffffff3f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-144
        [new object[] { "(0x040 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-145
        [new object[] { "(0x040 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 1065L }],
        // unknown-146
        [new object[] { "(0x07f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-147
        [new object[] { "(0x07f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 1065L }],
        // unknown-148
        [new object[] { "(0x140 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-149
        [new object[] { "(0x140 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 2080L }],
        // unknown-15
        [new object[] { "(0x7ffffffff3f )" }, null],
        // unknown-150
        [new object[] { "(0x17f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-151
        [new object[] { "(0x17f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 2080L }],
        // unknown-152
        [new object[] { "(0xfffeffff40 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-153
        [new object[] { "(0xfffeffff40 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-154
        [new object[] { "(0xfffeffff7f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-155
        [new object[] { "(0xfffeffff7f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-156
        [new object[] { "(0x7ffffffff40 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-157
        [new object[] { "(0x7ffffffff40 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-158
        [new object[] { "(0x7ffffffff7f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-159
        [new object[] { "(0x7ffffffff7f (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-16
        [new object[] { "(0x040 )", null, null, true }, null],
        // unknown-160
        [new object[] { "(0x080 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-161
        [new object[] { "(0x080 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 1582L }],
        // unknown-162
        [new object[] { "(0x0bf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-163
        [new object[] { "(0x0bf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 1582L }],
        // unknown-164
        [new object[] { "(0x180 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-165
        [new object[] { "(0x180 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 3114L }],
        // unknown-166
        [new object[] { "(0x1bf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-167
        [new object[] { "(0x1bf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 3114L }],
        // unknown-168
        [new object[] { "(0xfffeffff80 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-169
        [new object[] { "(0xfffeffff80 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-17
        [new object[] { "(0x040 )" }, new object[] { "()", 109L }],
        // unknown-170
        [new object[] { "(0xfffeffffbf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-171
        [new object[] { "(0xfffeffffbf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-172
        [new object[] { "(0x7ffffffff80 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-173
        [new object[] { "(0x7ffffffff80 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-174
        [new object[] { "(0x7ffffffffbf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-175
        [new object[] { "(0x7ffffffffbf (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-176
        [new object[] { "(0x0c0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-177
        [new object[] { "(0x0c0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 738L }],
        // unknown-178
        [new object[] { "(0x0ff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-179
        [new object[] { "(0x0ff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 738L }],
        // unknown-18
        [new object[] { "(0x07f )", null, null, true }, null],
        // unknown-180
        [new object[] { "(0x1c0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-181
        [new object[] { "(0x1c0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 1426L }],
        // unknown-182
        [new object[] { "(0x1ff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-183
        [new object[] { "(0x1ff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, new object[] { "()", 1426L }],
        // unknown-184
        [new object[] { "(0xfffeffffc0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-185
        [new object[] { "(0xfffeffffc0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-186
        [new object[] { "(0xfffeffffff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-187
        [new object[] { "(0xfffeffffff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-188
        [new object[] { "(0x7ffffffffc0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-189
        [new object[] { "(0x7ffffffffc0 (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-19
        [new object[] { "(0x07f )" }, new object[] { "()", 109L }],
        // unknown-190
        [new object[] { "(0x7ffffffffff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))", null, null, true }, null],
        // unknown-191
        [new object[] { "(0x7ffffffffff (q . 0x101010101010101010101010101010101010101010101010101010101010101010101010101010101010101101010101010101010101010101010101010101010101010101010101010101010101010101010101010101) (q . 0x101010101))" }, null],
        // unknown-192
        [new object[] { "(0x000 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-193
        [new object[] { "(0x000 (q . 1) (q . 2) (q . (1 10 20)))" }, new object[] { "()", 71L }],
        // unknown-194
        [new object[] { "(0x03f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-195
        [new object[] { "(0x03f (q . 1) (q . 2) (q . (1 10 20)))" }, new object[] { "()", 71L }],
        // unknown-196
        [new object[] { "(0x100 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-197
        [new object[] { "(0x100 (q . 1) (q . 2) (q . (1 10 20)))" }, new object[] { "()", 72L }],
        // unknown-198
        [new object[] { "(0x13f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-199
        [new object[] { "(0x13f (q . 1) (q . 2) (q . (1 10 20)))" }, new object[] { "()", 72L }],
        // unknown-20
        [new object[] { "(0x140 )", null, null, true }, null],
        // unknown-200
        [new object[] { "(0xfffeffff00 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-201
        [new object[] { "(0xfffeffff00 (q . 1) (q . 2) (q . (1 10 20)))" }, new object[] { "()", 4294901830L }],
        // unknown-202
        [new object[] { "(0xfffeffff3f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-203
        [new object[] { "(0xfffeffff3f (q . 1) (q . 2) (q . (1 10 20)))" }, new object[] { "()", 4294901830L }],
        // unknown-204
        [new object[] { "(0x7ffffffff00 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-205
        [new object[] { "(0x7ffffffff00 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-206
        [new object[] { "(0x7ffffffff3f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-207
        [new object[] { "(0x7ffffffff3f (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-208
        [new object[] { "(0x040 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-209
        [new object[] { "(0x040 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-21
        [new object[] { "(0x140 )" }, new object[] { "()", 208L }],
        // unknown-210
        [new object[] { "(0x07f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-211
        [new object[] { "(0x07f (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-212
        [new object[] { "(0x140 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-213
        [new object[] { "(0x140 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-214
        [new object[] { "(0x17f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-215
        [new object[] { "(0x17f (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-216
        [new object[] { "(0xfffeffff40 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-217
        [new object[] { "(0xfffeffff40 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-218
        [new object[] { "(0xfffeffff7f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-219
        [new object[] { "(0xfffeffff7f (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-22
        [new object[] { "(0x17f )", null, null, true }, null],
        // unknown-220
        [new object[] { "(0x7ffffffff40 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-221
        [new object[] { "(0x7ffffffff40 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-222
        [new object[] { "(0x7ffffffff7f (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-223
        [new object[] { "(0x7ffffffff7f (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-224
        [new object[] { "(0x080 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-225
        [new object[] { "(0x080 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-226 to unknown-254
        [new object[] { "(0x0bf (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x0bf (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x180 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x180 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x17f )" }, new object[] { "()", 208L }],
        [new object[] { "(0x1bf (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x1bf (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0xfffeffff80 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0xfffeffff80 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0xfffeffffbf (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0xfffeffffbf (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x7ffffffff80 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x7ffffffff80 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x7ffffffffbf (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x7ffffffffbf (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0xfffeffff40 )", null, null, true }, null],
        [new object[] { "(0x0c0 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x0c0 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x0ff (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x0ff (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x1c0 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x1c0 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x1ff (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x1ff (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0xfffeffffc0 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0xfffeffffc0 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0xfffeffff40 )" }, null],
        [new object[] { "(0xfffeffffff (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0xfffeffffff (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x7ffffffffc0 (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        [new object[] { "(0x7ffffffffc0 (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        [new object[] { "(0x7ffffffffff (q . 1) (q . 2) (q . (1 10 20)))", null, null, true }, null],
        // unknown-255
        [new object[] { "(0x7ffffffffff (q . 1) (q . 2) (q . (1 10 20)))" }, null],
        // unknown-26
        [new object[] { "(0xfffeffff7f )", null, null, true }, null],
        // unknown-27
        [new object[] { "(0xfffeffff7f )" }, null],
        // unknown-28
        [new object[] { "(0x7ffffffff40 )", null, null, true }, null],
        // unknown-29
        [new object[] { "(0x7ffffffff40 )" }, null],
        // unknown-30
        [new object[] { "(0x7ffffffff7f )", null, null, true }, null],
        // unknown-31
        [new object[] { "(0x7ffffffff7f )" }, null],
        // unknown-32
        [new object[] { "(0x080 )", null, null, true }, null],
        // unknown-33
        [new object[] { "(0x080 )" }, new object[] { "()", 102L }],
        // unknown-34
        [new object[] { "(0x0bf )", null, null, true }, null],
        // unknown-35
        [new object[] { "(0x0bf )" }, new object[] { "()", 102L }],
        // unknown-36
        [new object[] { "(0x180 )", null, null, true }, null],
        // unknown-37
        [new object[] { "(0x180 )" }, new object[] { "()", 194L }],
        // unknown-38
        [new object[] { "(0x1bf )", null, null, true }, null],
        // unknown-39
        [new object[] { "(0x1bf )" }, new object[] { "()", 194L }],
        // unknown-40
        [new object[] { "(0xfffeffff80 )", null, null, true }, null],
        // unknown-41
        [new object[] { "(0xfffeffff80 )" }, null],
        // unknown-42
        [new object[] { "(0xfffeffffbf )", null, null, true }, null],
        // unknown-43
        [new object[] { "(0xfffeffffbf )" }, null],
        // unknown-44
        [new object[] { "(0x7ffffffff80 )", null, null, true }, null],
        // unknown-45
        [new object[] { "(0x7ffffffff80 )" }, null],
        // unknown-46
        [new object[] { "(0x7ffffffffbf )", null, null, true }, null],
        // unknown-47
        [new object[] { "(0x7ffffffffbf )" }, null],
        // unknown-48
        [new object[] { "(0x0c0 )", null, null, true }, null],
        // unknown-49
        [new object[] { "(0x0c0 )" }, new object[] { "()", 152L }],
        // unknown-50
        [new object[] { "(0x0ff )", null, null, true }, null],
        // unknown-51
        [new object[] { "(0x0ff )" }, new object[] { "()", 152L }],
        // unknown-52
        [new object[] { "(0x1c0 )", null, null, true }, null],
        // unknown-53
        [new object[] { "(0x1c0 )" }, new object[] { "()", 294L }],
        // unknown-54
        [new object[] { "(0x1ff )", null, null, true }, null],
        // unknown-55
        [new object[] { "(0x1ff )" }, new object[] { "()", 294L }],
        // unknown-56
        [new object[] { "(0xfffeffffc0 )", null, null, true }, null],
        // unknown-57
        [new object[] { "(0xfffeffffc0 )" }, null],
        // unknown-58
        [new object[] { "(0xfffeffffff )", null, null, true }, null],
        // unknown-59
        [new object[] { "(0xfffeffffff )" }, null],
        // unknown-60
        [new object[] { "(0x7ffffffffc0 )", null, null, true }, null],
        // unknown-61
        [new object[] { "(0x7ffffffffc0 )" }, null],
        // unknown-62
        [new object[] { "(0x7ffffffffff )", null, null, true }, null],
        // unknown-63
        [new object[] { "(0x7ffffffffff )" }, null],
        // unknown-64
        [new object[] { "(0x000 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-65
        [new object[] { "(0x000 (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 71L }],
        // unknown-66
        [new object[] { "(0x03f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-67
        [new object[] { "(0x03f (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 71L }],
        // unknown-68
        [new object[] { "(0x100 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-69
        [new object[] { "(0x100 (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 72L }],
        // unknown-70
        [new object[] { "(0x13f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-71
        [new object[] { "(0x13f (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 72L }],
        // unknown-72
        [new object[] { "(0xfffeffff00 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-73
        [new object[] { "(0xfffeffff00 (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 4294901830L }],
        // unknown-74
        [new object[] { "(0xfffeffff3f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-75
        [new object[] { "(0xfffeffff3f (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 4294901830L }],
        // unknown-76
        [new object[] { "(0x7ffffffff00 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-77
        [new object[] { "(0x7ffffffff00 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-78
        [new object[] { "(0x7ffffffff3f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-79
        [new object[] { "(0x7ffffffff3f (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-80
        [new object[] { "(0x040 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-81
        [new object[] { "(0x040 (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 1138L }],
        // unknown-82
        [new object[] { "(0x07f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-83
        [new object[] { "(0x07f (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 1138L }],
        // unknown-84
        [new object[] { "(0x140 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-85
        [new object[] { "(0x140 (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 2206L }],
        // unknown-86
        [new object[] { "(0x17f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-87
        [new object[] { "(0x17f (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 2206L }],
        // unknown-88
        [new object[] { "(0xfffeffff40 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-89
        [new object[] { "(0xfffeffff40 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-90
        [new object[] { "(0xfffeffff7f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-91
        [new object[] { "(0xfffeffff7f (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-92
        [new object[] { "(0x7ffffffff40 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-93
        [new object[] { "(0x7ffffffff40 (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-94
        [new object[] { "(0x7ffffffff7f (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-95
        [new object[] { "(0x7ffffffff7f (q . 1) (q . 2) (q . 3))" }, null],
        // unknown-96
        [new object[] { "(0x080 (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-97
        [new object[] { "(0x080 (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 1962L }],
        // unknown-98
        [new object[] { "(0x0bf (q . 1) (q . 2) (q . 3))", null, null, true }, null],
        // unknown-99
        [new object[] { "(0x0bf (q . 1) (q . 2) (q . 3))" }, new object[] { "()", 1962L }],

    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestRun(object[] input, object[] output)
    {
        RunTestHelper.Run(input, output);
    }
}

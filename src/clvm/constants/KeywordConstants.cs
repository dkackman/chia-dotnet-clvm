namespace chia.dotnet.clvm;

public static class KeywordConstants
{
    private static IReadOnlyDictionary<string, ulong> keywords = new Dictionary<string, ulong>
    {
        {"q", 0x01},
        {"a", 0x02},
        {"i", 0x03},
        {"c", 0x04},
        {"f", 0x05},
        {"r", 0x06},
        {"l", 0x07},
        {"x", 0x08},
        {"=", 0x09},
        {">s", 0x0a},
        {"sha256", 0x0b},
        {"substr", 0x0c},
        {"strlen", 0x0d},
        {"concat", 0x0e},
        {"+", 0x10},
        {"-", 0x11},
        {"*", 0x12},
        {"/", 0x13},
        {"divmod", 0x14},
        {">", 0x15},
        {"ash", 0x16},
        {"lsh", 0x17},
        {"logand", 0x18},
        {"logior", 0x19},
        {"logxor", 0x1a},
        {"lognot", 0x1b},
        {"point_add", 0x1d},
        {"pubkey_for_exp", 0x1e},
        {"not", 0x20},
        {"any", 0x21},
        {"all", 0x22},
        {".", 0x23},
        {"softfork", 0x24},
    };

    public static IReadOnlyDictionary<string, ulong> Keywords { get => keywords; set => keywords = value; }
}
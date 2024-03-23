namespace chia.dotnet.clvm;

/// <summary>
/// Chia condition codes.
/// </summary>
public enum ConditionCodes
{
    /**
     * REMARK
     * This condition is always considered valid by the mempool.
     * This condition has no parameters.
     * Format: (1)
     */
    REMARK = 1,

    /**
     * AGG_SIG_PARENT
     * This condition is part of CHIP-0011, and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (43 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The parent coin id of the coin being spent.
     * - The domain string, sha256(genesis_id + 43).
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_PARENT = 43,

    /**
     * AGG_SIG_PUZZLE
     * This condition is part of CHIP-0011, and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (44 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The puzzle hash of the coin being spent.
     * - The domain string, sha256(genesis_id + 44).
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_PUZZLE = 44,

    /**
     * AGG_SIG_AMOUNT
     * This condition is part of CHIP-0011 and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (45 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The amount of the coin being spent.
     * - The domain string, sha256(genesis_id + 45).
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_AMOUNT = 45,

    /**
     * AGG_SIG_PUZZLE_AMOUNT
     * This condition is part of CHIP-0011, and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (46 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The puzzle hash of the coin being spent.
     * - The amount of the coin being spent.
     * - The domain string, sha256(genesis_id + 46).
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_PUZZLE_AMOUNT = 46,

    /**
     * AGG_SIG_PARENT_AMOUNT
     * This condition is part of CHIP-0011, and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (47 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The parent coin id of the coin being spent.
     * - The amount of the coin being spent.
     * - The domain string, sha256(genesis_id + 47).
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_PARENT_AMOUNT = 47,

    /**
     * AGG_SIG_PARENT_PUZZLE
     * This condition is part of CHIP-0011, and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (48 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The parent coin id of the coin being spent.
     * - The puzzle hash of the coin being spent.
     * - The domain string, sha256(genesis_id + 48).
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_PARENT_PUZZLE = 48,

    /**
     * AGG_SIG_UNSAFE
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (49 public_key message)
     * Verifies a signature for a given message. For security reasons, domain strings are not permitted at the end of AGG_SIG_UNSAFE messages.
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_UNSAFE = 49,

    /**
     * AGG_SIG_ME
     * TIP
     * In most cases, AGG_SIG_ME is the recommended condition for requiring signatures. Signatures created for a specific coin spend will only be valid for that exact coin, which prevents an attacker from reusing the signature for other spends.
     *
     * NOTE
     * This condition adds an additional CLVM cost of 1,200,000.
     *
     * Format: (50 public_key message)
     * Verifies a signature for a given message which is concatenated with:
     * - The id of the coin being spent.
     * - The domain string, genesis_id.
     *
     * Parameters:
     * - public_key: G1Element
     * - message: Bytes
     */
    AGG_SIG_ME = 50,

    /**
     * CREATE_COIN
     * NOTE
     * This condition adds an additional CLVM cost of 1,800,000.
     *
     * Format: (51 puzzle_hash amount (...memos)?)
     * Creates a new coin output with a given puzzle hash and amount. This coin is its parent.
     * For more information on the memos parameter, see the section on Memos and Hinting.
     *
     * Parameters:
     * - puzzle_hash: Bytes32
     * - amount: Unsigned Int
     * - memos (optional): Bytes32 List
     */
    CREATE_COIN = 51,

    /**
     * RESERVE_FEE
     * Format: (52 amount)
     * Requires that the total amount remaining in the transaction after all outputs have been created is no less than the reserved fee amount.
     *
     * Parameters:
     * - amount: Unsigned Int
     */
    RESERVE_FEE = 52,

    /**
     * CREATE_COIN_ANNOUNCEMENT
     * Format: (60 message)
     * Creates an announcement of a given message, tied to this coin's id. For more details, see the section on Announcements.
     *
     * Parameters:
     * - message: Bytes
     */
    CREATE_COIN_ANNOUNCEMENT = 60,

    /**
     * ASSERT_COIN_ANNOUNCEMENT
     * Format: (61 announcement_id)
     * Asserts an announcement with a given id, which is calculated as sha256(coin_id + message).
     * For more details, see the section on Announcements.
     *
     * Parameters:
     * - announcement_id: Bytes32
     */
    ASSERT_COIN_ANNOUNCEMENT = 61,

    /**
     * CREATE_PUZZLE_ANNOUNCEMENT
     * Format: (62 message)
     * Creates an announcement of a given message, tied to this coin's puzzle hash.
     * For more details, see the section on Announcements.
     *
     * Parameters:
     * - message: Bytes
     */
    CREATE_PUZZLE_ANNOUNCEMENT = 62,

    /**
     * ASSERT_PUZZLE_ANNOUNCEMENT
     * Format: (63 announcement_id)
     * Asserts an announcement with a given id, which is calculated as sha256(puzzle_hash + message).
     * For more details, see the section on Announcements.
     *
     * Parameters:
     * - announcement_id: Bytes32
     */
    ASSERT_PUZZLE_ANNOUNCEMENT = 63,

    /**
     * ASSERT_CONCURRENT_SPEND
     * Format: (64 coin_id)
     * Asserts that this coin is spent within the same block as the spend of a given coin.
     *
     * Parameters:
     * - coin_id: Bytes32
     */
    ASSERT_CONCURRENT_SPEND = 64,

    /**
     * ASSERT_CONCURRENT_PUZZLE
     * Format: (65 puzzle_hash)
     * Asserts that this coin is in the same block as the spend of another coin with a given puzzle hash.
     *
     * Parameters:
     * - puzzle_hash: Bytes32
     */
    ASSERT_CONCURRENT_PUZZLE = 65,

    /**
     * ASSERT_MY_COIN_ID
     * Format: (70 coin_id)
     * Asserts that the id of this coin matches a given value.
     *
     * Parameters:
     * - coin_id: Bytes32
     */
    ASSERT_MY_COIN_ID = 70,

    /**
     * ASSERT_MY_PARENT_ID
     * Format: (71 parent_id)
     * Asserts that the parent id of this coin matches a given value.
     *
     * Parameters:
     * - parent_id: Bytes32
     */
    ASSERT_MY_PARENT_ID = 71,

    /**
     * ASSERT_MY_PUZZLE_HASH
     * Format: (72 puzzle_hash)
     * Asserts that the puzzle hash of this coin matches a given value.
     *
     * Parameters:
     * - puzzle_hash: Bytes32
     */
    ASSERT_MY_PUZZLE_HASH = 72,

    /**
     * ASSERT_MY_AMOUNT
     * Format: (73 amount)
     * Asserts that the amount of this coin matches a given value.
     *
     * Parameters:
     * - amount: Unsigned Int
     */
    ASSERT_MY_AMOUNT = 73,

    /**
     * ASSERT_MY_BIRTH_SECONDS
     * Format: (74 seconds)
     * Asserts that this coin was created at a given timestamp.
     *
     * Parameters:
     * - seconds: Unsigned Int
     */
    ASSERT_MY_BIRTH_SECONDS = 74,

    /**
     * ASSERT_MY_BIRTH_HEIGHT
     * Format: (75 block_height)
     * Asserts that this coin was created at a given block height.
     *
     * Parameters:
     * - block_height: Unsigned Int
     */
    ASSERT_MY_BIRTH_HEIGHT = 75,

    /**
     * ASSERT_EPHEMERAL
     * Format: (76)
     * Asserts that this coin was created within the current block.
     * This condition has no parameters.
     */
    ASSERT_EPHEMERAL = 76,

    /**
     * ASSERT_SECONDS_RELATIVE
     * Format: (80 seconds_passed)
     * Asserts that the previous transaction block was created at least a given number of seconds after this coin was created.
     *
     * Parameters:
     * - seconds_passed: Unsigned Int
     */
    ASSERT_SECONDS_RELATIVE = 80,

    /**
     * ASSERT_SECONDS_ABSOLUTE
     * Format: (81 seconds)
     * Asserts that the previous transaction block was created at or after a given timestamp, in seconds.
     *
     * Parameters:
     * - seconds: Unsigned Int
     */
    ASSERT_SECONDS_ABSOLUTE = 81,

    /**
     * ASSERT_HEIGHT_RELATIVE
     * Format: (82 block_height_passed)
     * Asserts that the previous transaction block was created at least a given number of blocks after this coin was created.
     *
     * Parameters:
     * - block_height_passed: Unsigned Int
     */
    ASSERT_HEIGHT_RELATIVE = 82,

    /**
     * ASSERT_HEIGHT_ABSOLUTE
     * Format: (83 block_height)
     * Asserts that the previous transaction block was created at or after a given block height.
     *
     * Parameters:
     * - block_height: Unsigned Int
     */
    ASSERT_HEIGHT_ABSOLUTE = 83,

    /**
     * ASSERT_BEFORE_SECONDS_RELATIVE
     * Format: (84 seconds_passed)
     * Asserts that the previous transaction block was created before a given number of seconds after this coin was created.
     *
     * Parameters:
     * - seconds_passed: Unsigned Int
     */
    ASSERT_BEFORE_SECONDS_RELATIVE = 84,

    /**
     * ASSERT_BEFORE_SECONDS_ABSOLUTE
     * Format: (85 seconds)
     * Asserts that the previous transaction block was created before a given timestamp, in seconds.
     *
     * Parameters:
     * - seconds: Unsigned Int
     */
    ASSERT_BEFORE_SECONDS_ABSOLUTE = 85,

    /**
     * ASSERT_BEFORE_HEIGHT_RELATIVE
     * Format: (86 block_height_passed)
     * Asserts that the previous transaction block was created before a given number of blocks after this coin was created.
     *
     * Parameters:
     * - block_height_passed: Unsigned Int
     */
    ASSERT_BEFORE_HEIGHT_RELATIVE = 86,

    /**
     * ASSERT_BEFORE_HEIGHT_ABSOLUTE
     * Format: (87 block_height)
     * Asserts that the previous transaction block was created before a given height.
     *
     * Parameters:
     * - block_height: Unsigned Int
     */
    ASSERT_BEFORE_HEIGHT_ABSOLUTE = 87,

    /**
     * SOFTFORK
     * INFO
     * This condition is part of CHIP-0011, and will be available at block height 5,496,000.
     *
     * NOTE
     * This condition adds an additional CLVM cost equal to whatever the value of the first argument is.
     *
     * Format: (90 cost ...args)
     * Allows future conditions with non-zero CLVM costs to be added as soft forks. The cost argument is two bytes,
     * with a maximum size of 65,535 (an actual cost of 655,350,000).
     *
     * Parameters:
     * - cost: Unsigned Int
     * - ...args: Any (further arguments are not specified as the soft-forked condition defines these)
     */
    SOFTFORK = 90
}

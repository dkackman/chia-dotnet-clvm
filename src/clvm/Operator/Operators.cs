using chia.dotnet.bls;
using System.Numerics;

namespace chia.dotnet.clvm;

/// <summary>
/// Represents a collection of operators used in the CLVM language.
/// </summary>
public record OperatorsType
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorsType"/> class.
    /// </summary>
    public OperatorsType()
    {
        Operators = new Dictionary<string, Operator>(clvm.Operators.DefaultOperators);
    }

    /// <summary>
    /// Gets or sets the dictionary of operators.
    /// </summary>
    public IDictionary<string, Operator> Operators { get; init; }

    /// <summary>
    /// Gets or sets the unknown operator function.
    /// </summary>
    public Func<Program, Program, ProgramOutput> Unknown { get; set; } = clvm.Operators.DefaultUnknownOperator;

    /// <summary>
    /// Gets or sets the quote operator symbol.
    /// </summary>
    public string Quote { get; init; } = "q";

    /// <summary>
    /// Gets or sets the apply operator symbol.
    /// </summary>
    public string Apply { get; init; } = "a";
}

/// <summary>
/// Represents a delegate for an operator function.
/// </summary>
/// <param name="args">The arguments passed to the operator.</param>
/// <returns>The output of the operator.</returns>
public delegate ProgramOutput Operator(Program args);

internal static class Operators
{
    public static readonly IDictionary<string, Operator> DefaultOperators = new Dictionary<string, Operator>
    {
        { "i", new Operator(I) },
        { "c", new Operator(C) },
        { "f", new Operator(F) },
        { "r", new Operator(R) },
        { "l", new Operator(L) },
        { "x", new Operator(X) },
        { "=", new Operator(Equal) },
        { "sha256", new Operator(Sha256) },
        { "+", new Operator(Plus) },
        { "-", new Operator(Minus) },
        { "*", new Operator(Multiply) },
        { "divmod", new Operator(DivMod) },
        { "/", new Operator(Divide) },
        { ">", new Operator(GreaterThan) },
        { ">s", new Operator(GreaterThanSigned) },
        { "pubkey_for_exp", new Operator(PubkeyForExp ) },
        { "point_add", new Operator(PointAdd) },
        { "strlen", new Operator(Strlen ) },
        { "substr", new Operator(Substr) },
        { "concat", new Operator(Concat) },
        { "ash", new Operator(Ash) },
        { "lsh", new Operator(Lsh) },
        { "logand", new Operator(Logand) },
        { "logior", new Operator(Logior) },
        { "logxor", new Operator(Logxor) },
        { "lognot", new Operator(Lognot) },
        { "not", new Operator(Not) },
        { "any", new Operator(Any) },
        { "all", new Operator(All) },
        { "softfork", new Operator(Softfork) },
    };

    private static ProgramOutput I(Program args)
    {
        var list = args.ToList("i", 3);

        return new ProgramOutput
        {
            Value = list[0].IsNull ? list[2] : list[1],
            Cost = Costs.If
        };
    }

    private static ProgramOutput C(Program args)
    {
        var list = args.ToList("c", 2);

        return new ProgramOutput
        {
            Value = Program.FromCons(list[0], list[1]),
            Cost = Costs.Cons
        };
    }

    private static ProgramOutput F(Program args)
    {
        var list = args.ToList("f", 1, ArgumentType.Cons);

        return new ProgramOutput
        {
            Value = list[0].First,
            Cost = Costs.First
        };
    }

    private static ProgramOutput R(Program args)
    {
        var list = args.ToList("r", 1, ArgumentType.Cons);

        return new ProgramOutput
        {
            Value = list[0].Rest,
            Cost = Costs.Rest
        };
    }

    private static ProgramOutput L(Program args)
    {
        var list = args.ToList("l", 1);

        return new ProgramOutput
        {
            Value = Program.FromBool(list[0].IsCons),
            Cost = Costs.Listp
        };
    }

    private static ProgramOutput X(Program args) => throw new Exception($"The error {args} was raised{args.PositionSuffix}.");


    private static ProgramOutput Equal(Program args)
    {
        var list = args.ToList("=", 2, ArgumentType.Atom);

        return new ProgramOutput
        {
            Value = Program.FromBool(ByteUtils.BytesEqual(list[0].Atom, list[1].Atom)),
            Cost = Costs.EqBase + (list[0].Atom.Length + (long)list[1].Atom.Length) * Costs.EqPerByte
        };
    }

    private static ProgramOutput Sha256(Program args)
    {
        var list = args.ToList("sha256", null, ArgumentType.Atom);
        var cost = Costs.Sha256Base;
        var argLength = 0;
        var bytes = new List<byte>();

        foreach (var item in list)
        {
            bytes.AddRange(item.Atom);
            argLength += item.Atom.Length;
            cost += Costs.Sha256PerArg;
        }
        cost += (ulong)argLength * Costs.Sha256PerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBytes(Hmac.Hash256(bytes.ToArray())),
            Cost = cost
        });
    }

    private static ProgramOutput Plus(Program args)
    {
        var list = args.ToList("+", null, ArgumentType.Atom);
        var total = BigInteger.Zero;
        var cost = Costs.ArithBase;
        var argSize = 0;

        foreach (var item in list)
        {
            total += item.ToBigInt();
            argSize += item.Atom.Length;
            cost += Costs.ArithPerArg;
        }
        cost += argSize * Costs.ArithPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBigInt(total),
            Cost = cost
        });
    }

    private static ProgramOutput Minus(Program args)
    {
        var cost = Costs.ArithBase;
        if (args.IsNull)
        {
            return new ProgramOutput { Value = Program.Nil, Cost = cost };
        }

        var list = args.ToList("-", null, ArgumentType.Atom);
        var total = BigInteger.Zero;
        var sign = BigInteger.One;
        int argSize = 0;
        foreach (var item in list)
        {
            total += sign * item.ToBigInt();
            sign = -sign;
            argSize += item.Atom.Length;
            cost += Costs.ArithPerArg;
        }
        cost += argSize * Costs.ArithPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBigInt(total),
            Cost = cost
        });
    }

    private static ProgramOutput Multiply(Program args)
    {
        var list = args.ToList("*", null, ArgumentType.Atom);
        var cost = Costs.MulBase;
        if (!list.Any())
        {
            return ClvmHelper.MallocCost(new ProgramOutput { Value = Program.True, Cost = cost });
        }

        BigInteger value = list[0].ToBigInt();
        var size = list[0].Atom.Length;
        foreach (var item in list.Skip(1))
        {
            cost += Costs.MulPerOp +
                    ((ulong)item.Atom.Length + (ulong)size) * Costs.MulLinearPerByte +
                    (item.Atom.Length * size) / Costs.MulSquarePerByteDivider;
            value *= item.ToBigInt();
            size = ClvmHelper.LimbsForBigInt(value);
        }

        return ClvmHelper.MallocCost(new ProgramOutput { Value = Program.FromBigInt(value), Cost = cost });
    }

    private static ProgramOutput DivMod(Program args)
    {
        var list = args.ToList("divmod", 2, ArgumentType.Atom);
        var cost = Costs.DivmodBase;
        var numerator = list[0].ToBigInt();
        var denominator = list[1].ToBigInt();
        if (denominator == BigInteger.Zero)
            throw new Exception($"Cannot divide by zero in \"divmod\" operator{args.PositionSuffix}.");

        cost += ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.DivmodPerByte;
        var quotientValue = numerator / denominator;
        var remainderValue = ClvmHelper.Mod(numerator, denominator);
        if ((numerator < BigInteger.Zero) != (denominator < BigInteger.Zero) && remainderValue != BigInteger.Zero)
        {
            quotientValue -= BigInteger.One;
        }

        var quotient = Program.FromBigInt(quotientValue);
        var remainder = Program.FromBigInt(remainderValue);
        cost += ((ulong)quotient.Atom.Length + (ulong)remainder.Atom.Length) * Costs.MallocPerByte;

        return new ProgramOutput { Value = Program.FromCons(quotient, remainder), Cost = cost };
    }

    private static ProgramOutput Divide(Program args)
    {
        var list = args.ToList("/", 2, ArgumentType.Atom);
        var cost = Costs.DivBase;
        var numerator = list[0].ToBigInt();
        var denominator = list[1].ToBigInt();
        if (denominator == BigInteger.Zero)
            throw new Exception($"Cannot divide by zero in \"/\" operator{args.PositionSuffix}.");

        cost += ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.DivPerByte;
        var quotientValue = numerator / denominator;
        //var remainderValue = ClvmHelper.Mod(numerator, denominator);
        if ((numerator < BigInteger.Zero) != (denominator < BigInteger.Zero) && quotientValue < BigInteger.Zero)
        {
            quotientValue -= BigInteger.One;
        }

        var quotient = Program.FromBigInt(quotientValue);
        return ClvmHelper.MallocCost(new ProgramOutput { Value = quotient, Cost = cost });
    }
    private static ProgramOutput GreaterThan(Program args)
    {
        var list = args.ToList(">", 2, ArgumentType.Atom);
        var cost = Costs.GrBase + ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.GrPerByte;

        return new ProgramOutput
        {
            Value = Program.FromBool(list[0].ToBigInt() > list[1].ToBigInt()),
            Cost = cost
        };
    }

    private static ProgramOutput GreaterThanSigned(Program args)
    {
        var list = args.ToList(">s", 2, ArgumentType.Atom);
        var cost = Costs.GrsBase + ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.GrsPerByte;

        return new ProgramOutput
        {
            Value = Program.FromBool(String.Compare(list[0].ToHex(), list[1].ToHex(), StringComparison.InvariantCultureIgnoreCase) == 1),
            Cost = cost
        };
    }
    private static ProgramOutput PubkeyForExp(Program args)
    {
        var list = args.ToList("pubkey_for_exp", 1, ArgumentType.Atom);
        var value = ClvmHelper.Mod(list[0].ToBigInt(), Constants.N);
        var exponent = PrivateKey.FromBytes(value.BigIntToBytes(32, Endian.Big));
        var cost = Costs.PubkeyBase + (ulong)list[0].Atom.Length * Costs.PubkeyPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBytes(exponent.GetG1().ToBytes()),
            Cost = cost
        });
    }

    private static ProgramOutput PointAdd(Program args)
    {
        var list = args.ToList("point_add", null, ArgumentType.Atom);
        var cost = Costs.PointAddBase;
        var point = JacobianPoint.InfinityG1();
        foreach (var item in list)
        {
            point = point.Add(JacobianPoint.FromBytes(item.Atom, false));
            cost += Costs.PointAddPerArg;
        }

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBytes(point.ToBytes()),
            Cost = cost
        });
    }

    private static ProgramOutput Strlen(Program args)
    {
        var list = args.ToList("strlen", 1, ArgumentType.Atom);
        var size = list[0].Atom.Length;
        var cost = Costs.StrlenBase + (ulong)size * Costs.StrlenPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromInt(size), // Assuming FromInt method is defined in Program
            Cost = cost
        });
    }

    private static ProgramOutput Substr(Program args)
    {
        var list = args.ToList("substr", [2, 3], ArgumentType.Atom);
        var value = list[0].Atom;
        if (list[1].Atom.Length > 4 || (list.Count == 3 && list[2].Atom.Length > 4))
            throw new Exception($"Expected 4 byte indices in \"substr\" operator{args.PositionSuffix}.");

        var from = (int)list[1].ToInt(); // Assuming ToInt method is defined in Program
        var to = (int)(list.Count == 3 ? list[2].ToInt() : value.Length);
        if (to > value.Length || to < from || to < 0 || from < 0)
            throw new Exception($"Invalid indices in \"substr\" operator{args.PositionSuffix}.");

        return new ProgramOutput
        {
            Value = Program.FromBytes(value.Skip(from).Take(to - from).ToArray()), // Assuming FromBytes method is defined in Program
            Cost = 1 // Assuming cost is calculated as 1
        };
    }

    private static ProgramOutput Concat(Program args)
    {
        var list = args.ToList("concat", null, ArgumentType.Atom);
        var cost = Costs.ConcatBase;
        var bytes = new List<byte>();

        foreach (var item in list)
        {
            bytes.AddRange(item.Atom);
            cost += Costs.ConcatPerArg;
        }
        cost += (ulong)bytes.Count * Costs.ConcatPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBytes(bytes.ToArray()), // Assuming FromBytes method is defined in Program
            Cost = cost
        });
    }

    private static ProgramOutput Ash(Program args)
    {
        var list = args.ToList("ash", 2, ArgumentType.Atom);
        if (list[1].Atom.Length > 4)
            throw new Exception($"Shift must be 32 bits in \"ash\" operator{args.PositionSuffix}.");

        var shift = list[1].ToBigInt();
        if (BigInteger.Abs(shift) > 65535)
            throw new Exception($"Shift too large in \"ash\" operator{args.PositionSuffix}.");

        var value = list[0].ToBigInt();
        value = shift >= 0 ? value << (int)shift : value >> (int)-shift;
        var cost = Costs.AshiftBase + (list[0].Atom.Length + ClvmHelper.LimbsForBigInt(value)) * Costs.AshiftPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBigInt(value),
            Cost = cost
        });
    }

    private static ProgramOutput Lsh(Program args)
    {
        var list = args.ToList("lsh", 2, ArgumentType.Atom);
        if (list[1].Atom.Length > 4)
            throw new Exception($"Shift must be 32 bits in \"lsh\" operator{args.PositionSuffix}.");

        var shift = list[1].ToBigInt();
        if (BigInteger.Abs(shift) > 65535)
            throw new Exception($"Shift too large in \"lsh\" operator{args.PositionSuffix}.");

        var value = list[0].Atom.BytesToBigInt();
        if (value < BigInteger.Zero)
        {
            value = -value;
        }
        value = shift >= 0 ? value << (int)shift : value >> (int)-shift;
        var cost = Costs.LshiftBase + (list[0].Atom.Length + ClvmHelper.LimbsForBigInt(value)) * Costs.LshiftPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBigInt(value),
            Cost = cost
        });
    }

    private static ProgramOutput Logand(Program args) => ClvmHelper.BinopReduction("logand", BigInteger.MinusOne, args, (a, b) => a & b);

    private static ProgramOutput Logior(Program args) => ClvmHelper.BinopReduction("logior", BigInteger.Zero, args, (a, b) => a | b);
    private static ProgramOutput Logxor(Program args) => ClvmHelper.BinopReduction("logxor", BigInteger.Zero, args, (a, b) => a ^ b);

    private static ProgramOutput Lognot(Program args)
    {
        var items = args.ToList("lognot", 1, ArgumentType.Atom);
        var cost = Costs.LognotBase + (ulong)items[0].Atom.Length * Costs.LognotPerByte;

        return ClvmHelper.MallocCost(new ProgramOutput
        {
            Value = Program.FromBigInt(~items[0].ToBigInt()),
            Cost = cost
        });
    }

    private static ProgramOutput Not(Program args)
    {
        var items = args.ToList("not", 1);
        var cost = Costs.BoolBase;

        return new ProgramOutput
        {
            Value = Program.FromBool(items[0].IsNull),
            Cost = cost
        };
    }

    private static ProgramOutput Any(Program args)
    {
        var list = args.ToList("any");
        var cost = Costs.BoolBase + (ulong)list.Count * Costs.BoolPerArg;
        var result = false;

        foreach (var item in list)
        {
            if (!item.IsNull)
            {
                result = true;
                break;
            }
        }

        return new ProgramOutput
        {
            Value = Program.FromBool(result),
            Cost = cost
        };
    }

    private static ProgramOutput All(Program args)
    {
        var list = args.ToList("all");
        var cost = Costs.BoolBase + (ulong)list.Count * Costs.BoolPerArg;
        var result = true;

        foreach (var item in list)
        {
            if (item.IsNull)
            {
                result = false;
                break;
            }
        }

        return new ProgramOutput
        {
            Value = Program.FromBool(result),
            Cost = cost
        };
    }

    private static ProgramOutput Softfork(Program args)
    {
        var list = args.ToList("softfork", [1, int.MaxValue]);
        if (!list[0].IsAtom)
            throw new Exception($"Expected atom argument in \"softfork\" operator at {list[0].PositionSuffix}.");

        var cost = list[0].ToBigInt();
        if (cost < BigInteger.One)
            throw new Exception($"Cost must be greater than zero in \"softfork\" operator{args.PositionSuffix}.");

        return new ProgramOutput
        {
            Value = Program.False, // Assuming Program.False is defined
            Cost = cost
        };
    }

    public static ProgramOutput RunOperator(Program op, Program args, RunOptions options)
    {
        BigInteger symbol = op.ToBigInt();
        string keyword = KeywordConstants.Keywords.FirstOrDefault(entry => entry.Value == symbol).Key ?? op.ToText();
        if (options.Operators.Operators.TryGetValue(keyword, out Operator? value))
        {
            ProgramOutput result = value(args);
            return result;
        }

        return options.Operators.Unknown(op, args);
    }

    public static ProgramOutput DefaultUnknownOperator(Program op, Program args)
    {
        if (op.Atom.Length == 0 || op.Atom.Take(2).SequenceEqual(new byte[] { 0xff, 0xff }))
            throw new Exception($"Reserved operator {op.PositionSuffix}.");

        if (op.Atom.Length > 5)
            throw new Exception($"Invalid operator {op.PositionSuffix}.");

        var costFunction = (op.Atom[^1] & 0xc0) >> 6;
        var costMultiplier = ByteUtils.BytesToInt(op.Atom.Take(op.Atom.Length - 1).ToArray(), Endian.Big, false) + 1;
        BigInteger cost;

        if (costFunction == 0)
        {
            cost = 1;
        }
        else if (costFunction == 1)
        {
            cost = Costs.ArithBase;
            var argSize = 0;
            foreach (var item in args.ToList())
            {
                if (!item.IsAtom)
                    throw new Exception($"Expected atom argument {item.PositionSuffix}.");

                argSize += item.Atom.Length;
                cost += Costs.ArithPerArg;
            }
            cost += new BigInteger(argSize) * Costs.ArithPerByte;
        }
        else if (costFunction == 2)
        {
            cost = Costs.MulBase;
            var argList = args.ToList();
            if (argList.Any())
            {
                var first = argList[0];
                if (!first.IsAtom)
                    throw new Exception($"Expected atom argument {first.PositionSuffix}.");

                var current = first.Atom.Length;
                foreach (var item in argList.Skip(1))
                {
                    if (!item.IsAtom)
                        throw new Exception($"Expected atom argument {item.PositionSuffix}.");

                    cost += Costs.MulPerOp + (new BigInteger(item.Atom.Length + current) * Costs.MulLinearPerByte) + (new BigInteger(item.Atom.Length * current) / Costs.MulSquarePerByteDivider);
                    current += item.Atom.Length;
                }
            }
        }
        else if (costFunction == 3)
        {
            cost = Costs.ConcatBase;
            var length = 0;
            foreach (var item in args.ToList())
            {
                if (!item.IsAtom)
                    throw new Exception($"Expected atom argument {item.PositionSuffix}.");

                cost += Costs.ConcatPerArg;
                length += item.Atom.Length;
            }
            cost += new BigInteger(length) * Costs.ConcatPerByte;
        }
        else
        {
            throw new Exception($"Unknown cost function {op.PositionSuffix}.");
        }

        cost *= new BigInteger(costMultiplier);

        // 8125954129920
        if (cost >= BigInteger.Pow(2, 32))
            throw new Exception($"Invalid operator {op.PositionSuffix}.");

        return new ProgramOutput { Value = Program.Nil, Cost = cost };
    }
}

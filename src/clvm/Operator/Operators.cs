using chia.dotnet.bls;
using System.Numerics;

namespace chia.dotnet.clvm;

public delegate ProgramOutput Operator(Program args);

public record OperatorsType
{
    public IDictionary<string, Operator> Operators { get; init; } = new Dictionary<string, Operator>();
    public Func<Program, Program, ProgramOutput> Unknown { get; init; } = (a, b) => new ProgramOutput() { Value = a, Cost = 0 };
    public string Quote { get; init; } = string.Empty;
    public string Apply { get; init; } = string.Empty;
}

public static class Operators
{
    public static readonly IReadOnlyDictionary<string, Operator> operators = new Dictionary<string, Operator>
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
        { "substr", new Operator(Substr) }
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
        var list = args.ToList("f", 1, ClvmHelper.ArgumentType.Cons);
        return new ProgramOutput
        {
            Value = list[0].First,
            Cost = Costs.First
        };
    }

    private static ProgramOutput R(Program args)
    {
        var list = args.ToList("r", 1, ClvmHelper.ArgumentType.Cons);
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

    private static ProgramOutput X(Program args)
    {
        throw new Exception($"The error {args} was raised{args.PositionSuffix}.");
    }

    private static ProgramOutput Equal(Program args)
    {
        var list = args.ToList("=", 2, ClvmHelper.ArgumentType.Atom);
        return new ProgramOutput
        {
            Value = Program.FromBool(ByteUtils.BytesEqual(list[0].Atom, list[1].Atom)),
            Cost = Costs.EqBase + ((long)list[0].Atom.Length + (long)list[1].Atom.Length) * Costs.EqPerByte
        };
    }

    private static ProgramOutput Sha256(Program args)
    {
        var list = args.ToList("sha256", null, ClvmHelper.ArgumentType.Atom);
        var cost = Costs.Sha256Base;
        int argLength = 0;
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
        var list = args.ToList("+", null, ClvmHelper.ArgumentType.Atom);
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
            return new ProgramOutput { Value = Program.Nil, Cost = cost };

        var list = args.ToList("-", null, ClvmHelper.ArgumentType.Atom);
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
        var list = args.ToList("*", null, ClvmHelper.ArgumentType.Atom);
        var cost = Costs.MulBase;
        if (!list.Any())
            return ClvmHelper.MallocCost(new ProgramOutput { Value = Program.True, Cost = cost });

        BigInteger value = list[0].ToBigInt();
        int size = list[0].Atom.Length;
        foreach (var item in list.Skip(1))
        {
            cost += Costs.MulPerOp +
                    ((ulong)item.Atom.Length + (ulong)size) * Costs.MulLinearPerByte +
                    ((ulong)item.Atom.Length * (ulong)size) / Costs.MulSquarePerByteDivider;
            value *= item.ToBigInt();
            size = ClvmHelper.LimbsForBigInt(value);
        }
        return ClvmHelper.MallocCost(new ProgramOutput { Value = Program.FromBigInt(value), Cost = cost });
    }

    private static ProgramOutput DivMod(Program args)
    {
        var list = args.ToList("divmod", 2, ClvmHelper.ArgumentType.Atom);
        var cost = Costs.DivmodBase;
        var numerator = list[0].ToBigInt();
        var denominator = list[1].ToBigInt();
        if (denominator == BigInteger.Zero)
            throw new Exception($"Cannot divide by zero in \"divmod\" operator{args.PositionSuffix}.");

        cost += ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.DivmodPerByte;
        var quotientValue = numerator / denominator;
        var remainderValue = ClvmHelper.Mod(numerator, denominator);
        if ((numerator < BigInteger.Zero) != (denominator < BigInteger.Zero) && remainderValue != BigInteger.Zero)
            quotientValue -= BigInteger.One;

        var quotient = Program.FromBigInt(quotientValue);
        var remainder = Program.FromBigInt(remainderValue);
        cost += ((ulong)quotient.Atom.Length + (ulong)remainder.Atom.Length) * Costs.MallocPerByte;

        return new ProgramOutput { Value = Program.FromCons(quotient, remainder), Cost = cost };
    }

    private static ProgramOutput Divide(Program args)
    {
        var list = args.ToList("/", 2, ClvmHelper.ArgumentType.Atom);
        var cost = Costs.DivBase;
        var numerator = list[0].ToBigInt();
        var denominator = list[1].ToBigInt();
        if (denominator == BigInteger.Zero)
            throw new Exception($"Cannot divide by zero in \"/\" operator{args.PositionSuffix}.");

        cost += ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.DivPerByte;
        var quotientValue = numerator / denominator;
        //var remainderValue = ClvmHelper.Mod(numerator, denominator);
        if ((numerator < BigInteger.Zero) != (denominator < BigInteger.Zero) && quotientValue < BigInteger.Zero)
            quotientValue -= BigInteger.One;

        var quotient = Program.FromBigInt(quotientValue);
        return ClvmHelper.MallocCost(new ProgramOutput { Value = quotient, Cost = cost });
    }
    private static ProgramOutput GreaterThan(Program args)
    {
        var list = args.ToList(">", 2, ClvmHelper.ArgumentType.Atom);
        var cost = Costs.GrBase + ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.GrPerByte;
        return new ProgramOutput
        {
            Value = Program.FromBool(list[0].ToBigInt() > list[1].ToBigInt()),
            Cost = cost
        };
    }

    private static ProgramOutput GreaterThanSigned(Program args)
    {
        var list = args.ToList(">s", 2, ClvmHelper.ArgumentType.Atom);
        var cost = Costs.GrsBase + ((ulong)list[0].Atom.Length + (ulong)list[1].Atom.Length) * Costs.GrsPerByte;
        return new ProgramOutput
        {
            Value = Program.FromBool(String.Compare(list[0].ToHex(), list[1].ToHex(), StringComparison.Ordinal) == 1),
            Cost = cost
        };
    }
    private static ProgramOutput PubkeyForExp(Program args)
    {
        var list = args.ToList("pubkey_for_exp", 1, ClvmHelper.ArgumentType.Atom);
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
        var list = args.ToList("point_add", null, ClvmHelper.ArgumentType.Atom);
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
        var list = args.ToList("strlen", 1, ClvmHelper.ArgumentType.Atom);
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
        var list = args.ToList("substr", new[] { 2, 3 }, ClvmHelper.ArgumentType.Atom);
        var value = list[0].Atom;
        if (list[1].Atom.Length > 4 || (list.Count == 3 && list[2].Atom.Length > 4))
            throw new Exception($"Expected 4 byte indices in \"substr\" operator{args.PositionSuffix}.");

        var from = list[1].ToInt(); // Assuming ToInt method is defined in Program
        var to = list.Count == 3 ? list[2].ToInt() : value.Length;
        if (to > value.Length || to < from || to < 0 || from < 0)
            throw new Exception($"Invalid indices in \"substr\" operator{args.PositionSuffix}.");

        return new ProgramOutput
        {
            Value = Program.FromBytes(value.Skip(from).Take(to - from).ToArray()), // Assuming FromBytes method is defined in Program
            Cost = 1 // Assuming cost is calculated as 1
        };
    }

}
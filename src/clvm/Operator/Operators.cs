using chia.dotnet.bls;

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
        { "sha256", new Operator(Sha256) }
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
}
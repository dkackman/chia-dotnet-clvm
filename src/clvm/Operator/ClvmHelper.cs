using System.Numerics;

namespace chia.dotnet.clvm;

internal enum ArgumentType { Atom, Cons }

internal static class ClvmHelper
{
    public static BigInteger Mod(BigInteger value, BigInteger modulus) => ((value % modulus) + modulus) % modulus;

    public static int LimbsForBigInt(BigInteger value)
    {
        int length = value == 0 ? 0 : value < 0 ? (-value).ToByteArray().Length : value.ToByteArray().Length;
        if (value < 0) length++;
        return (length + 7) / 8;
    }

    public static ProgramOutput BinopReduction(
        string opName,
        BigInteger initialValue,
        Program args,
        Func<BigInteger, BigInteger, BigInteger> opFunction)
    {
        var total = initialValue;
        var argSize = 0;
        var cost = Costs.LogBase;

        var list = args.ToList(opName, null, ArgumentType.Atom);

        foreach (var item in list)
        {
            total = opFunction(total, item.ToBigInt());
            argSize += item.Atom.Length;
            cost += Costs.LogPerArg;
        }

        cost += argSize * Costs.LogPerByte;

        return MallocCost(new ProgramOutput
        {
            Value = Program.FromBigInt(total),
            Cost = cost
        });
    }

    public static ProgramOutput MallocCost(ProgramOutput output)
    {
        return new ProgramOutput
        {
            Value = output.Value,
            Cost = output.Cost + (output.Value.Atom.Length * Costs.MallocPerByte)
        };
    }

    public static IList<Program> ToList(
        this Program program,
        string name,
        int length,
        ArgumentType? type = null)
    {
        var list = program.ToList();
        if (list.Count != length)
        {
            throw new Exception($"Expected {length} arguments in {name} operator{program.PositionSuffix}.");
        }

        ValidateListType(list, type, name);
        return list;
    }

    public static IList<Program> ToList(
            this Program program,
            string name,
            ArgumentType? type = null)
    {
        var list = program.ToList();

        ValidateListType(list, type, name);

        return list;
    }

    public static IList<Program> ToList(
        this Program program,
        string name,
        int[]? lengthRange,
        ArgumentType? type = null)
    {
        var list = program.ToList();
        lengthRange ??= [];
        if (lengthRange.Length >= 2 && (list.Count < lengthRange[0] || list.Count > lengthRange[1]))
        {
            var rangeDescription = lengthRange[1] == int.MaxValue ? $"at least {lengthRange[0]}" : $"between {lengthRange[0]} and {lengthRange[1]}";
            throw new Exception($"Expected {rangeDescription} arguments in {name} operator{program.PositionSuffix}.");
        }

        ValidateListType(list, type, name);
        return list;
    }

    private static void ValidateListType(IList<Program> list, ArgumentType? type, string name)
    {
        if (type.HasValue)
        {
            foreach (var item in list)
            {
                switch (type.Value)
                {
                    case ArgumentType.Atom when !item.IsAtom:
                    case ArgumentType.Cons when !item.IsCons:
                        throw new Exception($"Expected {type.Value} argument in {name} operator{item.PositionSuffix}.");
                }
            }
        }
    }
}

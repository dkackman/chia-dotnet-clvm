using System.Numerics;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public static class DefaultOperators
{
    public static OperatorsType MakeDefaultOperators()
    {
        return  new OperatorsType
        {
            Unknown = DefaultUnknownOperator,
            Quote = "q",
            Apply = "a"
        };
    }

    public static ProgramOutput DefaultUnknownOperator(Program op, Program args)
    {
        if (op.Atom.Length == 0 || op.Atom.Take(2).SequenceEqual(new byte[] { 0xff, 0xff }))
            throw new Exception($"Reserved operator {op.PositionSuffix}.");

        if (op.Atom.Length > 5)
            throw new Exception($"Invalid operator {op.PositionSuffix}.");

        var costFunction = (op.Atom[^1] & 0xc0) >> 6;
        var costMultiplier = ByteUtils.BytesToInt(op.Atom.Take(op.Atom.Length - 1).ToArray(), Endian.Big, true) + 1;
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

        if (cost >= BigInteger.Pow(2, 32))
            throw new Exception($"Invalid operator {op.PositionSuffix}.");

        return new ProgramOutput { Value = Program.Nil, Cost = cost };
    }
}
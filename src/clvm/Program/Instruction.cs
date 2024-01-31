using System.Numerics;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

internal delegate BigInteger Instruction(Stack<Instruction> instructions, Stack<Program> stack, RunOptions options);

internal static class InstructionsClass
{
    public static IDictionary<string, Instruction> Instructions = new Dictionary<string, Instruction>
    {
        ["swap"] = (instructionStack, stack, options) =>
        {
            var second = stack.Pop();
            var first = stack.Pop();
            stack.Push(second);
            stack.Push(first);

            return BigInteger.Zero;
        },
        ["cons"] = (instructionStack, stack, options) =>
        {
            var first = stack.Pop();
            var second = stack.Pop();
            stack.Push(Program.FromCons(first, second));

            return BigInteger.Zero;
        },
        ["eval"] = (instructionStack, stack, options) =>
        {
            var pair = stack.Pop();
            var program = pair.First;
            var args = pair.Rest;
            if (program.IsAtom)
            {
                var output = TraversePath(program, args);
                stack.Push(output.Value);

                return output.Cost;
            }
            var op = program.First;
            if (op.IsCons)
            {
                var (newOperator, mustBeNil) = op.Cons;
                if (newOperator.IsCons || !mustBeNil.IsNull)
                    throw new Exception($"Operators that are lists must contain a single atom{op.PositionSuffix}.");
                var newOperandList = program.Rest;
                stack.Push(newOperator);
                stack.Push(newOperandList);
                instructionStack.Push(Instructions["apply"]);

                return Costs.Apply;
            }
            var operandList = program.Rest;
            if (ByteUtils.BytesEqual(op.Atom, Atoms.QuoteAtom))
            {
                stack.Push(operandList);

                return Costs.Quote;
            }
            instructionStack.Push(Instructions["apply"]);
            stack.Push(op);
            while (!operandList.IsNull)
            {
                stack.Push(Program.FromCons(operandList.First, args));
                instructionStack.Push(Instructions["cons"]);
                instructionStack.Push(Instructions["eval"]);
                instructionStack.Push(Instructions["swap"]);
                operandList = operandList.Rest;
            }
            stack.Push(Program.Nil);

            return BigInteger.One;
        },
        ["apply"] = (instructionStack, stack, options) =>
        {
            var operandList = stack.Pop();
            var op = stack.Pop();
            if (op.IsCons)
                throw new Exception($"An internal error occurred{op.PositionSuffix}.");

            if (ByteUtils.BytesEqual(op.Atom, Atoms.ApplyAtom))
            {
                var args = operandList.ToList();
                if (args.Count != 2)
                    throw new Exception($"Expected 2 arguments in \"a\" operator{operandList.PositionSuffix}.");
                stack.Push(Program.FromCons(args[0], args[1]));
                instructionStack.Push(Instructions["eval"]);

                return Costs.Apply;
            }
            var output = Operators.RunOperator(op, operandList, options);
            stack.Push(output.Value);

            return output.Cost;
        }
    };

    public static ProgramOutput TraversePath(Program value, Program environment)
    {
        BigInteger cost = Costs.PathLookupBase + Costs.PathLookupPerLeg;
        if (value.IsNull)
        {
            return new ProgramOutput { Value = Program.Nil, Cost = cost };
        }
        int endByteCursor = 0;
        byte[] atom = value.Atom;
        while (endByteCursor < atom.Length && atom[endByteCursor] == 0)
        {
            endByteCursor++;
        }
        cost += BigInteger.Multiply(endByteCursor, Costs.PathLookupPerZeroByte);
        if (endByteCursor == atom.Length)
        {
            return new ProgramOutput { Value = Program.Nil, Cost = cost };
        }
        byte endBitMask = MsbMask(atom[endByteCursor]);
        int byteCursor = atom.Length - 1;
        int bitMask = 0x01;
        while (byteCursor > endByteCursor || bitMask < endBitMask)
        {
            if (environment.IsAtom)
                throw new Exception($"Cannot traverse into {environment}{environment.PositionSuffix}.");
            if ((atom[byteCursor] & bitMask) != 0)
            {
                environment = environment.Rest;
            }
            else
            {
                environment = environment.First;
            }
            cost += Costs.PathLookupPerLeg;
            bitMask <<= 1;
            if (bitMask == 0x100)
            {
                byteCursor--;
                bitMask = 0x01;
            }
        }

        return new ProgramOutput { Value = environment, Cost = cost };
    }

    public static byte MsbMask(byte byteValue)
    {
        byteValue |= (byte)(byteValue >> 1);
        byteValue |= (byte)(byteValue >> 2);
        byteValue |= (byte)(byteValue >> 4);

        return (byte)((byteValue + 1) >> 1);
    }
}
using System.Numerics;

namespace chia.dotnet.clvm;

public delegate BigInteger Instruction(IList<Instruction> instructions, IList<Program> stack, RunOptions options);
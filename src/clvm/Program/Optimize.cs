using chia.dotnet.bls;

namespace chia.dotnet.clvm;

public static class Optimize
{
    public static bool SeemsConstant(Program program)
    {
        if (!program.IsCons) return program.IsNull;
        var op = program.First;
        if (!op.IsCons)
        {
            var value = op.Atom;
            if (ByteUtils.BytesEqual(value, Atoms.QuoteAtom))
            {
                return true;
            }
            else if (ByteUtils.BytesEqual(value, Atoms.RaiseAtom))
            {
                return false;
            }
        }
        else if (!SeemsConstant(op))
        {
            return false;
        }
        return program.Rest.ToList().All(item => SeemsConstant(item));
    }

    public static Program ConstantOptimizer(Program program, Eval evalAsProgram)
    {
        if (SeemsConstant(program) && !program.IsNull)
        {
            var newProgram = evalAsProgram(program, Program.Nil).Value;
            program = Compile.QuoteAsProgram(newProgram);
        }
        return program;
    }

    public static bool IsArgsCall(Program program)
    {
        return program.IsAtom && program.ToBigInt() == 1;
    }
    public static Program ConsQuoteApplyOptimizer(Program program, Eval _evalAsProgram)
    {
        var matched = Bindings.Match(Program.FromSource("(a (q . (: . sexp)) (: . args))"), program);
        if (matched != null && IsArgsCall(matched["args"]))
        {
            return matched["sexp"];
        }
        return program;
    }

    public static Program ConsFirst(Program args)
    {
        var matched = Bindings.Match(Program.FromSource("(c (: . first) (: . rest))"), args);
        if (matched != null)
        {
            return matched["first"];
        }
        return Program.FromList(new List<Program> { Program.FromBigInt(KeywordConstants.Keywords["f"]), args });
    }

    public static Program ConsRest(Program args)
    {
        var matched = Bindings.Match(Program.FromSource("(c (: . first) (: . rest))"), args);
        if (matched != null)
        {
            return matched["rest"];
        }
        return Program.FromList(new List<Program> { Program.FromBigInt(KeywordConstants.Keywords["r"]), args });
    }
    public static Program PathFromArgs(Program program, Program args)
    {
        var value = program.ToBigInt();
        if (value <= 1)
        {
            return args;
        }
        program = Program.FromBigInt(value >> 1);
        if ((value & 1) == 1)
        {
            return PathFromArgs(program, ConsRest(args));
        }
        return PathFromArgs(program, ConsFirst(args));
    }

    public static Program SubArgs(Program program, Program args)
    {
        if (!program.IsCons)
        {
            return PathFromArgs(program, args);
        }
        var first = program.First;
        if (first.IsCons)
        {
            first = SubArgs(first, args);
        }
        else if (ByteUtils.BytesEqual(first.Atom, Atoms.QuoteAtom))
        {
            return program;
        }
        return Program.FromList(new List<Program> { first }.Concat(program.Rest.ToList().Select(item => SubArgs(item, args))).ToList());
    }

    public static Program VarChangeOptimizerConsEval(Program program, Eval evalAsProgram)
    {
        var matched = Bindings.Match(Program.FromSource("(a (q . (: . sexp)) (: . args))"), program);
        if (matched == null)
        {
            return program;
        }
        var originalArgs = matched["args"];
        var originalCall = matched["sexp"];
        var newEvalProgramArgs = SubArgs(originalCall, originalArgs);
        if (SeemsConstant(newEvalProgramArgs))
        {
            return OptimizeProgram(newEvalProgramArgs, evalAsProgram);
        }
        var newOperands = newEvalProgramArgs.ToList();
        var optOperands = newOperands.Select(item => OptimizeProgram(item, evalAsProgram)).ToList();
        var nonConstantCount = optOperands.Count(item => item.IsCons && (item.First.IsCons || !ByteUtils.BytesEqual(item.First.Atom, Atoms.QuoteAtom)));
        if (nonConstantCount < 1)
        {
            return Program.FromList(optOperands);
        }
        return program;
    }

    public static Program ChildrenOptimizer(Program program, Eval evalAsProgram)
    {
        if (!program.IsCons)
        {
            return program;
        }

        var @operator = program.First;
        if (@operator.IsAtom && ByteUtils.BytesEqual(@operator.Atom, Atoms.QuoteAtom))
        {
            return program;
        }
        
        return Program.FromList(program.ToList().Select(item => OptimizeProgram(item, evalAsProgram)).ToList());
    }

    public static Program ConsOptimizer(Program program, Eval _evalAsProgram)
    {
        var matched = Bindings.Match(Program.FromSource("(f (c (: . first) (: . rest)))"), program);
        if (matched != null)
        {
            return matched["first"];
        }
        matched = Bindings.Match(Program.FromSource("(r (c (: . first) (: . rest)))"), program);
        if (matched != null)
        {
            return matched["rest"];
        }
        return program;
    }

    public static Program PathOptimizer(Program program, Eval _evalAsProgram)
    {
        var matched = Bindings.Match(Program.FromSource("(f ($ . atom))"), program);
        if (matched != null && !matched["atom"].IsNull)
        {
            var node = new NodePath(matched["atom"].ToBigInt()).Add(NodePath.Left);
            return Program.FromBytes(node.AsPath());
        }
        matched = Bindings.Match(Program.FromSource("(r ($ . atom))"), program);
        if (matched != null && !matched["atom"].IsNull)
        {
            var node = new NodePath(matched["atom"].ToBigInt()).Add(NodePath.Right);
            return Program.FromBytes(node.AsPath());
        }
        return program;
    }

    public static Program QuoteNullOptimizer(Program program, Eval _evalAsProgram)
    {
        var matched = Bindings.Match(Program.FromSource("(q . 0)"), program);
        if (matched != null)
        {
            return Program.Nil;
        }
        return program;
    }

    public static Program ApplyNullOptimizer(Program program, Eval _evalAsProgram)
    {
        var matched = Bindings.Match(Program.FromSource("(a 0 . (: . rest))"), program);
        if (matched != null)
        {
            return Program.Nil;
        }
        return program;
    }

    public static Program OptimizeProgram(Program program, Eval evalAsProgram)
    {
        if (program.IsAtom)
        {
            return program;
        }
        var optimizers = new List<Func<Program, Eval, Program>>
    {
        ConsOptimizer,
        ConstantOptimizer,
        ConsQuoteApplyOptimizer,
        VarChangeOptimizerConsEval,
        ChildrenOptimizer,
        PathOptimizer,
        QuoteNullOptimizer,
        ApplyNullOptimizer,
    };
        while (program.IsCons)
        {
            var startProgram = program;
            foreach (var optimizer in optimizers)
            {
                program = optimizer(program, evalAsProgram);
                if (!startProgram.Equals(program)) break;
            }
            if (startProgram.Equals(program))
            {
                return program;
            }
        }
        return program;
    }

    public static Operator MakeDoOpt(Eval runProgram)
    {
        return args =>
        {
            return new ProgramOutput
            {
                Value = OptimizeProgram(args.First, runProgram),
                Cost = 1
            };
        };
    }
}

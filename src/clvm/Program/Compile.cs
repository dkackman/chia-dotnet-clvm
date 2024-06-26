using System.Text;
using chia.dotnet.bls;

namespace chia.dotnet.clvm;

internal delegate ProgramOutput Eval(Program program, Program args);

internal static class Compile
{
    public static HashSet<string> PassThroughOperators = new(
        KeywordConstants.Keywords.Values.Select(value => ByteUtils.ToHex(value.Encode()))
        .Concat(new[] { Encoding.UTF8.GetBytes("com").ToHex(), Encoding.UTF8.GetBytes("opt").ToHex() })
    );

    public static Program CompileQq(Program args, Program macroLookup, Program symbolTable, Eval runProgram) =>
        CompileQq(args, macroLookup, symbolTable, runProgram, 1);

    public static Program CompileQq(Program args, Program macroLookup, Program symbolTable, Eval runProgram, int level = 1)
    {
        Program Com(Program program) => DoComProgram(program, macroLookup, symbolTable, runProgram);

        var program = args.First;
        if (!program.IsCons)
        {
            return QuoteAsProgram(program);
        }

        if (!program.First.IsCons)
        {
            var op = program.First.ToText();
            if (op == "qq")
            {
                var expression = CompileQq(program.Rest, macroLookup, symbolTable, runProgram, level + 1);

                return Com(Program.FromList(
                [
                    Program.FromBytes(Atoms.ConsAtom),
                    Program.FromText(op),
                    Program.FromList(
                        [
                            Program.FromBytes(Atoms.ConsAtom),
                            expression,
                            QuoteAsProgram(Program.Nil)
                        ])
                ]));
            }
            else if (op == "unquote")
            {
                if (level == 1)
                {
                    return Com(program.Rest.First);
                }

                var expression = CompileQq(program.Rest, macroLookup, symbolTable, runProgram, level - 1);
                return Com(Program.FromList(
                [
                    Program.FromBytes(Atoms.ConsAtom),
                    Program.FromText(op),
                    Program.FromList(
                    [
                        Program.FromBytes(Atoms.ConsAtom),
                        expression,
                        QuoteAsProgram(Program.Nil)
                    ])
                ]));
            }
        }

        var first = Com(Program.FromList([Program.FromText("qq"), program.First]));
        var rest = Com(Program.FromList([Program.FromText("qq"), program.Rest]));

        return Program.FromList([Program.FromBytes(Atoms.ConsAtom), first, rest]);
    }

    public static Program CompileMacros(Program args, Program macroLookup, Program symbolTable, Eval runProgram) => QuoteAsProgram(macroLookup);

    public static Program CompileSymbols(Program args, Program macroLookup, Program symbolTable, Eval runProgram) => QuoteAsProgram(symbolTable);

    public static IDictionary<string, Func<Program, Program, Program, Eval, Program>> CompileBindings =
        new Dictionary<string, Func<Program, Program, Program, Eval, Program>>
        {
            { "qq", CompileQq },
            { "macros", CompileMacros },
            { "symbols", CompileSymbols },
            { "lambda", Mod.CompileMod },
            { "mod", Mod.CompileMod },
        };

    public static Program LowerQuote(Program program, Program? macroLookup = null, Program? symbolTable = null, Eval? runProgram = null)
    {
        if (program.IsAtom)
        {
            return program;
        }

        if (program.First.IsAtom && program.First.ToText() == "quote")
        {
            if (!program.Rest.Rest.IsNull)
                throw new Exception($"Compilation error while compiling {program}. Quote takes exactly one argument{program.PositionSuffix}.");

            return QuoteAsProgram(LowerQuote(program.Rest.First));
        }

        return Program.FromCons(LowerQuote(program.First), LowerQuote(program.Rest));
    }

    public static Operator MakeDoCom(Eval runProgram)
    {
        return (sexp) =>
        {
            var prog = sexp.First;
            var symbolTable = Program.Nil;
            Program macroLookup;
            if (!sexp.Rest.IsNull)
            {
                macroLookup = sexp.Rest.First;
                if (!sexp.Rest.Rest.IsNull)
                {
                    symbolTable = sexp.Rest.Rest.First;
                }
            }
            else
            {
                macroLookup = Macros.DefaultMacroLookup(runProgram);
            }

            return new ProgramOutput
            {
                Value = DoComProgram(prog, macroLookup, symbolTable, runProgram),
                Cost = 1,
            };
        };
    }

    public static Program DoComProgram(Program program, Program macroLookup, Program symbolTable, Eval runProgram)
    {
        program = LowerQuote(program, macroLookup, symbolTable, runProgram);
        if (!program.IsCons)
        {
            var atom = program.ToText();
            if (atom == "@")
            {
                return Program.FromBytes(NodePath.Top.AsPath());
            }

            foreach (var pair in symbolTable.ToList())
            {
                var symbol = pair.First;
                var value = pair.Rest.First;
                if (symbol.IsAtom && symbol.ToText() == atom)
                {
                    return value;
                }
            }

            return QuoteAsProgram(program);
        }

        var op = program.First;
        if (op.IsCons)
        {
            var inner = EvalAsProgram(
                Program.FromList([
                    Program.FromText("com"),
                    QuoteAsProgram(op),
                    QuoteAsProgram(macroLookup),
                    QuoteAsProgram(symbolTable),
                ]),
                Program.FromBytes(NodePath.Top.AsPath())
            );

            return Program.FromList([inner]);
        }

        var atom1 = op.ToText();
        foreach (var macroPair in macroLookup.ToList())
        {
            if (macroPair.First.IsAtom && macroPair.First.ToText() == atom1)
            {
                var macroCode = macroPair.Rest.First;
                var postProgram = BrunAsProgram(macroCode, program.Rest);
                var result1 = EvalAsProgram(
                    Program.FromList([
                        Program.FromText("com"),
                        postProgram,
                        QuoteAsProgram(macroLookup),
                        QuoteAsProgram(symbolTable),
                    ]),
                    Program.FromBytes(NodePath.Top.AsPath())
                );

                return result1;
            }
        }

        if (CompileBindings.TryGetValue(atom1, out Func<Program, Program, Program, Eval, Program>? binding))
        {
            var compiler = binding;
            var postProgram = compiler(program.Rest, macroLookup, symbolTable, runProgram);

            return EvalAsProgram(
                QuoteAsProgram(postProgram),
                Program.FromBytes(NodePath.Top.AsPath())
            );
        }

        if (ByteUtils.BytesEqual(op.Atom, Atoms.QuoteAtom))
        {
            return program;
        }

        var compiledArgs = program.Rest.ToList().Select(item => DoComProgram(item, macroLookup, symbolTable, runProgram)).ToList();

        var result = Program.FromList([op, .. compiledArgs]);
        if (PassThroughOperators.Contains(ByteUtils.ToHex(atom1.ToBytes())) || atom1.StartsWith("_"))
        {
            return result;
        }

        foreach (var item in symbolTable.ToList())
        {
            var itemList = item.ToList();
            var symbol = itemList[0];
            var value = itemList[1];
            if (!symbol.IsAtom)
            {
                continue;
            }

            var symbolText = symbol.ToText();
            if (symbolText == "*")
            {
                return result;
            }

            if (symbolText == atom1)
            {
                var newArgs = EvalAsProgram(
                    Program.FromList([
                        Program.FromText("opt"),
                        Program.FromList([
                            Program.FromText("com"),
                            QuoteAsProgram(
                                Program.FromList([
                                    Program.FromText("list"), .. program.Rest.ToList(),
                                ])
                            ),
                            QuoteAsProgram(macroLookup),
                            QuoteAsProgram(symbolTable),
                        ]),
                    ]),
                    Program.FromBytes(NodePath.Top.AsPath())
                );
                return Program.FromList([
                    Program.FromBytes(Atoms.ApplyAtom),
                    value,
                    Program.FromList([
                        Program.FromBytes(Atoms.ConsAtom),
                        Program.FromBytes(NodePath.Left.AsPath()),
                        newArgs,
                    ]),
                ]);
            }
        }

        throw new Exception($"Can't compile unknown operator {program}{program.PositionSuffix}.");
    }

    public static Program QuoteAsProgram(Program program) => Program.FromCons(Program.FromBigInt(KeywordConstants.Keywords["q"]), program);

    public static Program EvalAsProgram(Program program, Program args) => Program.FromList([Program.FromBigInt(KeywordConstants.Keywords["a"]), program, args]);

    public static Program RunAsProgram(Program program, Program macroLookup)
    {
        return EvalAsProgram(
            Program.FromList([
                Program.FromText("com"),
                program,
                QuoteAsProgram(macroLookup),
            ]),
            Program.FromBytes(NodePath.Top.AsPath())
        );
    }

    public static Program BrunAsProgram(Program program, Program args) => EvalAsProgram(QuoteAsProgram(program), QuoteAsProgram(args));
}

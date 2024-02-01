namespace chia.dotnet.clvm;

internal static class Mod
{
    public const string MainName = "";

    public static Program BuildTree(List<Program> items)
    {
        if (items.Count == 0)
        {
            return Program.Nil;
        }

        if (items.Count == 1)
        {
            return items[0];
        }

        var halfSize = items.Count >> 1;

        return Program.FromCons(
            BuildTree(items.GetRange(0, halfSize)),
            BuildTree(items.GetRange(halfSize, items.Count - halfSize))
        );
    }

    public static Program BuildTreeProgram(List<Program> items)
    {
        if (items.Count == 0)
        {
            return Program.FromList([Compile.QuoteAsProgram(Program.Nil)]);
        }

        if (items.Count == 1)
        {
            return items[0];
        }

        var halfSize = items.Count >> 1;

        return Program.FromList(
        [
            Program.FromBytes(Atoms.ConsAtom),
            BuildTreeProgram(items.GetRange(0, halfSize)),
            BuildTreeProgram(items.GetRange(halfSize, items.Count - halfSize)),
        ]);
    }

    public static IList<string> Flatten(Program program)
    {
        if (program.IsCons)
        {
            return [.. Flatten(program.First), .. Flatten(program.Rest)];
        }

        return [program.ToText()];
    }

    public static SortedSet<string> BuildUsedConstantNames(IDictionary<string, Program> functions, IDictionary<string, Program> constants, IList<Program> macros)
    {
        var macrosAsDict = new Dictionary<string, Program>();
        foreach (var item in macros)
        {
            macrosAsDict[item.Rest.First.ToText()] = item;
        }

        var possibleSymbols = new HashSet<string>(functions.Keys);
        possibleSymbols.UnionWith(constants.Keys);
        var newNames = new HashSet<string> { MainName };
        var usedNames = new HashSet<string>(newNames);
        while (newNames.Count > 0)
        {
            var priorNewNames = new HashSet<string>(newNames);
            newNames = [];
            foreach (var item in priorNewNames)
            {
                foreach (var group in new List<IDictionary<string, Program>> { functions, macrosAsDict })
                {
                    if (group.ContainsKey(item))
                    {
                        newNames.UnionWith(Flatten(group[item]));
                    }
                }
            }
            newNames.ExceptWith(usedNames);
            usedNames.UnionWith(newNames);
        }

        usedNames.IntersectWith(possibleSymbols);
        usedNames.Remove(MainName);

        return new SortedSet<string>(usedNames);
    }

    public static void ParseInclude(Program name, HashSet<string> @namespace, IDictionary<string, Program> functions, IDictionary<string, Program> constants, IList<Program> macros, Eval runProgram)
    {
        var program = Program.FromSource("(_read (_full_path_for_name 1))");
        var output = runProgram(program, name).Value;
        foreach (var item in output.ToList())
        {
            ParseModProgram(item, @namespace, functions, constants, macros, runProgram);
        }
    }

    public static Program UnquoteArgs(Program program, IList<string> args)
    {
        if (program.IsCons)
        {
            return Program.FromCons(
                UnquoteArgs(program.First, args),
                UnquoteArgs(program.Rest, args)
            );
        }

        if (args.Contains(program.ToText()))
        {
            return Program.FromList(new List<Program> { Program.FromText("unquote"), program });
        }

        return program;
    }

    public static Program DefunInlineToMacro(Program program)
    {
        var second = program.Rest;
        var third = second.Rest;
        var items = new List<Program> { Program.FromText("defmacro"), second.First, third.First };
        var code = third.Rest.First;
        var args = Flatten(third.First).Where(item => !string.IsNullOrEmpty(item)).ToList();
        var unquotedCode = UnquoteArgs(code, args);
        items.Add(Program.FromList(new List<Program> { Program.FromText("qq"), unquotedCode }));

        return Program.FromList(items);
    }

    public static void ParseModProgram(Program declarationProgram, HashSet<string> @namespace, IDictionary<string, Program> functions, IDictionary<string, Program> constants, IList<Program> macros, Eval runProgram)
    {
        var op = declarationProgram.First.ToText();
        var nameProgram = declarationProgram.Rest.First;
        if (op == "include")
        {
            ParseInclude(nameProgram, @namespace, functions, constants, macros, runProgram);
            return;
        }

        var name = nameProgram.ToText();
        if (@namespace.Contains(name))
        {
            throw new Exception($"Symbol {name} redefined.");
        }

        @namespace.Add(name);
        if (op == "defmacro")
        {
            macros.Add(declarationProgram);
        }
        else if (op == "defun")
        {
            functions[name] = declarationProgram.Rest.Rest;
        }
        else if (op == "defun-inline")
        {
            macros.Add(DefunInlineToMacro(declarationProgram));
        }
        else if (op == "defconstant")
        {
            constants[name] = Compile.QuoteAsProgram(declarationProgram.Rest.Rest.First);
        }
        else
        {
            throw new Exception($"Expected \"defun\", \"defun-inline\", \"defmacro\", or \"defconstant\", but got {op}.");
        }
    }

    public static (IDictionary<string, Program> functions, IDictionary<string, Program> constants, IList<Program> macros) CompileModStage1(Program args, Eval runProgram)
    {
        var functions = new Dictionary<string, Program>();
        var constants = new Dictionary<string, Program>();
        var macros = new List<Program>();
        var mainLocalArguments = args.First;
        var @namespace = new HashSet<string>();
        while (true)
        {
            args = args.Rest;
            if (args.Rest.IsNull)
            {
                break;
            }

            ParseModProgram(args.First, @namespace, functions, constants, macros, runProgram);
        }

        var uncompiledMain = args.First;
        functions[MainName] = Program.FromList(new List<Program> { mainLocalArguments, uncompiledMain });

        return (functions, constants, macros);
    }

    public static Program SymbolTableForTree(Program tree, NodePath rootNode)
    {
        if (tree.IsNull)
        {
            return Program.Nil;
        }

        if (!tree.IsCons)
        {
            return Program.FromList(new List<Program> { Program.FromList(new List<Program> { tree, Program.FromBytes(rootNode.AsPath()) }) });
        }

        var left = SymbolTableForTree(tree.First, rootNode.Add(NodePath.Left));
        var right = SymbolTableForTree(tree.Rest, rootNode.Add(NodePath.Right));

        return Program.FromList(left.ToList().Concat(right.ToList()).ToList());
    }

    public static Program BuildMacroLookupProgram(Program macroLookup, IList<Program> macros, Eval runProgram)
    {
        var macroLookupProgram = Compile.QuoteAsProgram(macroLookup);
        foreach (var macro in macros)
        {
            macroLookupProgram = Compile.EvalAsProgram(
                Program.FromList(new List<Program> {
                Program.FromText("opt"),
                Program.FromList(new List<Program> {
                    Program.FromText("com"),
                    Compile.QuoteAsProgram(
                        Program.FromList(new List<Program> {
                            Program.FromBytes(Atoms.ConsAtom),
                            macro,
                            macroLookupProgram
                        })
                        ),
                        macroLookupProgram
                    })
                }),
                Program.FromBytes(NodePath.Top.AsPath())
            );
            macroLookupProgram = Optimize.OptimizeProgram(macroLookupProgram, runProgram);
        }

        return macroLookupProgram;
    }

    public static IDictionary<string, Program> CompileFunctions(IDictionary<string, Program> functions, Program macroLookupProgram, Program constantSymbolTable, NodePath argsRootNode)
    {
        var compiledFunctions = new Dictionary<string, Program>();
        foreach (var function in functions)
        {
            var name = function.Key;
            var lambdaExpression = function.Value;
            var localSymbolTable = SymbolTableForTree(lambdaExpression.First, argsRootNode);
            var allSymbols = Program.FromList(localSymbolTable.ToList().Concat(constantSymbolTable.ToList()).ToList());
            compiledFunctions[name] = Program.FromList(new List<Program> {
            Program.FromText("opt"),
            Program.FromList(new List<Program> {
                Program.FromText("com"),
                Compile.QuoteAsProgram(lambdaExpression.Rest.First),
                macroLookupProgram,
                Compile.QuoteAsProgram(allSymbols)
                })
            });
        }

        return compiledFunctions;
    }

    public static Program CompileMod(Program args, Program macroLookup, Program _symbolTable, Eval runProgram)
    {
        var (functions, constants, macros) = CompileModStage1(args, runProgram);
        var macroLookupProgram = BuildMacroLookupProgram(macroLookup, macros, runProgram);
        var allConstantNames = BuildUsedConstantNames(functions, constants, macros);
        var hasConstantTree = allConstantNames.Count > 0;
        var constantTree = BuildTree(allConstantNames.Select(Program.FromText).ToList());
        var constantRootNode = NodePath.Left;
        var argsRootNode = hasConstantTree ? NodePath.Right : NodePath.Top;
        var constantSymbolTable = SymbolTableForTree(constantTree, constantRootNode);
        var compiledFunctions = CompileFunctions(functions, macroLookupProgram, constantSymbolTable, argsRootNode);
        var mainPathSource = compiledFunctions[MainName].ToString();
        string argTreeSource;
        if (hasConstantTree)
        {
            var allConstantsLookup = new Dictionary<string, Program>();
            foreach (var function in compiledFunctions)
            {
                if (allConstantNames.Contains(function.Key))
                {
                    allConstantsLookup[function.Key] = function.Value;
                }
            }

            foreach (var constant in constants)
            {
                allConstantsLookup[constant.Key] = constant.Value;
            }

            var allConstantsList = allConstantNames.Select(item => allConstantsLookup[item]).ToList();
            var allConstantsTreeProgram = BuildTreeProgram(allConstantsList);
            var allConstantsTreeSource = allConstantsTreeProgram.ToString();
            argTreeSource = $"(c {allConstantsTreeSource} 1)";
        }
        else
        {
            argTreeSource = "1";
        }

        return Program.FromSource($"(opt (q . (a {mainPathSource} {argTreeSource})))");
    }
}

namespace chia.dotnet.clvm;

public static class ClvmHelper
{
    public enum ArgumentType { Atom, Cons }

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
        var list = program.ToList(); // Assuming Program.ToList() returns List<Program>
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
        int[] lengthRange,
        ArgumentType? type = null)
    {
        var list = program.ToList(); // Assuming Program.ToList() returns List<Program>
        if (lengthRange.Length != 2 || list.Count < lengthRange[0] || list.Count > lengthRange[1])
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

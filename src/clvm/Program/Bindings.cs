using System.Text;

namespace chia.dotnet.clvm; 

public class Group : Dictionary<string, Program> { }

public static class Bindings
{
    private static readonly byte[] AtomMatch = Encoding.UTF8.GetBytes("$");
    private static readonly byte[] SexpMatch = Encoding.UTF8.GetBytes(":");

    public static Group UnifyBindings(Group bindings, string key, Program valueProgram)
    {
        if (bindings.ContainsKey(key))
        {
            if (!bindings[key].Equals(valueProgram)) return null;
            return bindings;
        }
        bindings[key] = valueProgram;
        return bindings;
    }

    public static Group Match(Program pattern, Program sexp, Group knownBindings = null)
    {
        knownBindings = knownBindings ?? new Group();

        if (!pattern.IsCons)
        {
            if (sexp.IsCons) return null;
            return pattern.Atom.SequenceEqual(sexp.Atom) ? knownBindings : null;
        }

        var left = pattern.First;
        var right = pattern.Rest;

        if (left.IsAtom && left.Atom.SequenceEqual(AtomMatch))
        {
            if (sexp.IsCons) return null;
            if (right.IsAtom && right.Atom.SequenceEqual(AtomMatch))
            {
                if (sexp.Atom.SequenceEqual(AtomMatch)) return new Group();
                return null;
            }
            return UnifyBindings(knownBindings, right.ToText(), sexp);
        }

        if (left.IsAtom && left.Atom.SequenceEqual(SexpMatch))
        {
            if (right.IsAtom && right.Atom.SequenceEqual(SexpMatch))
            {
                if (sexp.Atom.SequenceEqual(SexpMatch)) return new Group();
                return null;
            }
            return UnifyBindings(knownBindings, right.ToText(), sexp);
        }

        if (!sexp.IsCons) return null;
        var newBindings = Match(left, sexp.First, knownBindings);
        if (newBindings == null) return null;
        return Match(right, sexp.Rest, newBindings);
    }
}
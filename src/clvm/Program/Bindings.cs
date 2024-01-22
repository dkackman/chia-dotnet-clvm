using System.Text;
using System.Reflection;

namespace chia.dotnet.clvm;

public class Group : Dictionary<string, Program> { }

public static class Bindings
{
    public static T Merge<T>(T item1, T? item2) where T : class, new()
    {
        if (item2 is null)
        {
            return item1;
        }

        T result = new();

        // merge all public, instance properties that have both getters and setters
        // the values from object 2 take precedence unless they are null
        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty))
        {
            var value1 = property.GetValue(item1);
            var value2 = property.GetValue(item2);

            property.SetValue(result, value2 ?? value1);
        }

        return result;
    }

    private static readonly byte[] AtomMatch = Encoding.UTF8.GetBytes("$");
    private static readonly byte[] SexpMatch = Encoding.UTF8.GetBytes(":");

    public static Group? UnifyBindings(Group bindings, string key, Program valueProgram)
    {
        if (bindings.ContainsKey(key))
        {
            if (!bindings[key].Equals(valueProgram))
            {
                return null;
            }
            return bindings;
        }
        bindings[key] = valueProgram;
        return bindings;
    }

    public static Group? Match(Program pattern, Program sexp, Group? knownBindings = null)
    {
        knownBindings ??= [];

        if (!pattern.IsCons)
        {
            if (sexp.IsCons)
            {
                return null;
            }
            return pattern.Atom.SequenceEqual(sexp.Atom) ? knownBindings : null;
        }

        var left = pattern.First;
        var right = pattern.Rest;

        if (left.IsAtom && left.Atom.SequenceEqual(AtomMatch))
        {
            if (sexp.IsCons)
            {
                return null;
            }
            if (right.IsAtom && right.Atom.SequenceEqual(AtomMatch))
            {
                if (sexp.Atom.SequenceEqual(AtomMatch))
                {
                    return new Group();
                }
                return null;
            }
            return UnifyBindings(knownBindings, right.ToText(), sexp);
        }

        if (left.IsAtom && left.Atom.SequenceEqual(SexpMatch))
        {
            if (right.IsAtom && right.Atom.SequenceEqual(SexpMatch))
            {
                if (sexp.Atom.SequenceEqual(SexpMatch))
                {
                    return new Group();
                }
                return null;
            }
            return UnifyBindings(knownBindings, right.ToText(), sexp);
        }

        if (!sexp.IsCons)
        {
            return null;
        }
        var newBindings = Match(left, sexp.First, knownBindings);
        if (newBindings == null)
        {
            return null;
        }
        return Match(right, sexp.Rest, newBindings);
    }
}
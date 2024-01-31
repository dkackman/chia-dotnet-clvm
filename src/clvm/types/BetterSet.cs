namespace chia.dotnet.clvm;

internal class BetterSet<T> : HashSet<T>
{
    public BetterSet() : base() { }

    public BetterSet(IEnumerable<T> collection) : base(collection) { }

    public bool IsSuperset(BetterSet<T> set)
    {
        return this.IsSupersetOf(set);
    }

    public bool IsSubset(BetterSet<T> set)
    {
        return this.IsSubsetOf(set);
    }

    public bool IsSupersetProper(BetterSet<T> set)
    {
        return this.IsSuperset(set) && !this.IsSubset(set);
    }

    public bool IsSubsetProper(BetterSet<T> set)
    {
        return this.IsSubset(set) && !this.IsSuperset(set);
    }

    public bool EqualsSet(BetterSet<T> set)
    {
        return this.SetEquals(set);
    }

    public BetterSet<T> Union(BetterSet<T> set)
    {
        var union = new BetterSet<T>(this);
        union.UnionWith(set);
        return union;
    }

    public BetterSet<T> Intersection(BetterSet<T> set)
    {
        var intersection = new BetterSet<T>(this);
        intersection.IntersectWith(set);
        return intersection;
    }

    public BetterSet<T> SymmetricDifference(BetterSet<T> set)
    {
        var difference = new BetterSet<T>(this);
        difference.SymmetricExceptWith(set);
        return difference;
    }

    public BetterSet<T> Difference(BetterSet<T> set)
    {
        var difference = new BetterSet<T>(this);
        difference.ExceptWith(set);
        return difference;
    }

    public void Update(BetterSet<T> set)
    {
        this.UnionWith(set);
    }

    public void DifferenceUpdate(BetterSet<T> set)
    {
        this.ExceptWith(set);
    }

    public void SymmetricDifferenceUpdate(BetterSet<T> set)
    {
        this.SymmetricExceptWith(set);
    }

    public void IntersectionUpdate(BetterSet<T> set)
    {
        this.IntersectWith(set);
    }

    public BetterSet<U> Map<U>(Func<T, U> mapper)
    {
        var result = new BetterSet<U>();
        foreach (var item in this)
        {
            result.Add(mapper(item));
        }
        return result;
    }

    public BetterSet<T> Filter(Func<T, bool> predicate)
    {
        var result = new BetterSet<T>();
        foreach (var item in this)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    // Note: Sorting is not inherent to sets in C#, so the sort method is not included.
    // If needed, you can convert to a list, sort it, and then convert back to a set.
}

using System;
using System.Collections.Generic;
using System.Linq;

public class CustomSet : IEnumerable<int>
{
    public int Length => set.Count;
    private Dictionary<int, int> set = new Dictionary<int, int>();

    public CustomSet() { }

    public CustomSet(int x) : this() { Add(x); }

    public CustomSet(IEnumerable<int> xs)
        : this()
    {
        foreach (var x in xs) Add(x);
    }

    public bool Empty() => Length == 0;

    public bool Contains(int x) => set.ContainsKey(x.GetHashCode());

    public CustomSet Add(int x)
    {
        var s = this.ToString();
        set[x.GetHashCode()] = x;
        set = set.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        return this;
    }

    public CustomSet Remove(int x) 
    {
        set.Remove(x.GetHashCode());
        return this;
    }

    public override bool Equals(object obj) => Equals(obj as CustomSet);
    public bool Equals(CustomSet other) => Length.Equals(other?.Length) &&
        Union(other).Length == Intersection(other).Length;

    public override int GetHashCode() => base.GetHashCode();

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => set.Values.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => set.Values.GetEnumerator();

    public CustomSet Union(CustomSet other) => new CustomSet(this.Concat(other));

    public bool Subset(CustomSet other) => this.All(other.Contains);

    public bool Disjoint(CustomSet other) => !this.Any(other.Contains);

    public CustomSet Intersection(CustomSet other) => this.Where(other.Contains).Aggregate(
            new CustomSet(),
            (acc, val) => acc.Add(val)
        );

    public CustomSet Difference(CustomSet other) => other.Aggregate(
            new CustomSet(this),
            (acc, val) => acc.Remove(val)
        );
}

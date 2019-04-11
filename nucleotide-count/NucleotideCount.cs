using System;
using System.Collections.Generic;

public class NucleotideCount
{
    public NucleotideGroup NucleotideCounts { get; } = new NucleotideGroup();

    public int Count(char ch) => NucleotideCounts[ch];
    public static Dictionary<char, int> Count(string dna) => new NucleotideCount(dna).NucleotideCounts;

    public NucleotideCount(string dna) { foreach (var ch in dna) NucleotideCounts[ch]++; }
}

public class NucleotideGroup : Dictionary<char, int>
{
    public NucleotideGroup() : base()
    {
        base['A'] = 0;
        base['C'] = 0;
        base['G'] = 0;
        base['T'] = 0;
    }
    public new int this[char key]
    {
        get
        {
            if (ContainsKey(key)) return base[key];
            throw new ArgumentException();
        }
        set { base[key] = value; }
    }
}
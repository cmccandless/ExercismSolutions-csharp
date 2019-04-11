using System;
using System.Linq;

public static class Series
{
    public static string[] Slices(string seriesText, int n)
    {
        if (n < 1 || n > seriesText.Length) throw new ArgumentException();
        return Enumerable.Range(0, seriesText.Length - n + 1)
            .Select(i => seriesText.Substring(i, n))
            .ToArray();
    }
}

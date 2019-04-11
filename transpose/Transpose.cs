using System.Collections.Generic;
using System.Linq;

public static class Transpose
{
    private static string[] PadLines(string[] lines)
    {
        var padded = new List<string>{lines.Last()};
        for (int i = lines.Length - 2; i >= 0; i--)
            padded.Insert(0, lines[i].PadRight(padded[0].Length, ' '));
        return padded.ToArray();
    }

    public static char CharAtOrDefault(this string s, int i, char defaultValue = '\0') =>
        s.Length > i ? s[i] : defaultValue;

    private static string Column(string[] lines, int index) =>
        new string(lines.Select(line => line.CharAtOrDefault(index))
            .Where(c => c != '\0')
            .ToArray());

    public static string String(string input)
    {
        var padded = PadLines(input.Split('\n'));
        return string.Join(
            "\n",
            padded[0].Select((_, i) => Column(padded, i))
        );
    }
}

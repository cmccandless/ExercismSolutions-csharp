using System;
using System.Collections.Generic;
using System.Linq;

public static class ScaleGenerator
{
    private static string[] majorKeys = new[]
    { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", };

    private static string[] minorKeys = new[]
    { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", };

    private static HashSet<string> validMajorTonics = new HashSet<string>
    { "A", "a", "B", "b", "C", "c#", "D", "d#","E", "e", "F#", "f#", "G", "g#", };

    private static Dictionary<char, int> StepAmount = new Dictionary<char, int>
    { ['m'] = 1, ['M'] = 2, ['A'] = 3, };

    private static bool IsMajorScale(this string tonic) => validMajorTonics.Contains(tonic);

    private static string[] Keys(string tonic) => validMajorTonics.Contains(tonic) ? majorKeys : minorKeys;

    public static string[] Chromatic(string tonic) => Interval(tonic, "mmmmmmmmmmmm");
    public static string[] Interval(string tonic, string steps) => 
        steps.Substring(0, steps.Length - 1).Dump(x => $"steps:{steps}").Aggregate(
            new[] { tonic.ToTitle().Dump(x => $"tonic:{x}") },
            (interval, step) =>
            {
                var keys = Keys(tonic);
                return interval.Append(keys[(Array.IndexOf(keys, interval.Last()) + StepAmount[step]) % keys.Length]).ToArray();
            }
        ).Dump();
}

static class Extensions
{
    public static T Dump<T>(this T obj) => obj.Dump(x => x.ToString());

    public static T[] Dump<T>(this T[] arr) => arr.Dump(a => $"[{string.Join(", ", arr.Select(x => x.ToString()))}]");

    public static T Dump<T>(this T obj, Func<T, string> formatter)
    {
        Console.WriteLine(formatter(obj));
        return obj;
    }

    public static string ToTitle(this string s) => $"{s.First().ToString().ToUpper()}{new string(s.Skip(1).ToArray())}";
}

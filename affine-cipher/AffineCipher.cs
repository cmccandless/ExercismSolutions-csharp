using System;
using System.Collections.Generic;
using System.Linq;

static class Extensions
{
    public static IEnumerable<List<T>> Slices<T>(this IEnumerable<T> col, int sliceSize)
    {
        var q = new Queue<T>(col);
        while(q.Any())
        {
            var l = new List<T>();
            for (int i=0; i < sliceSize && q.Any(); i++)
                l.Add(q.Dequeue());
            yield return l;
        }
    }

    public static IEnumerable<string> Slices(this string str, int sliceSize) =>
        str.ToCharArray().Slices(sliceSize).Select(l => new String(l.ToArray()));

    public static int LetterToIndex(this char ch, char _base = 'a') => ch - _base;
    public static char IndexToLetter(this int index, char _base = 'a', int alphaSize = 26) =>
        (char)((index % alphaSize) + _base);

    public static char Transcode(this char ch, Func<int, int> transcoder) =>
        transcoder(ch.LetterToIndex()).IndexToLetter();

    public static string Transcode(this string str, Func<int, int> transcoder) =>
        str.ToLower()
            .Where(Char.IsLetterOrDigit)
            .Select(ch =>
                Char.IsLetter(ch) ? ch.Transcode(transcoder) : ch
            ).ToArray()
            .Join()
            .Trim();

    public static string Join<T>(this IEnumerable<T> col, string delimiter = "") =>
        col.Join(delimiter, x => x.ToString());

    public static string Join<T>(this IEnumerable<T> col, string delimiter, Func<T, string> formatter) =>
        string.Join(delimiter, col.Select(formatter));
}

public static class AffineCipher
{
    private static char[] letters = Enumerable.Range(0, 26).Select(i => (char)(i + 'a')).ToArray();
    private static int ALPHA_SIZE => letters.Length;

    private static (int x, int y) minMax(int x, int y) => (Math.Min(x, y), Math.Max(x, y));

    private static int GCD((int x, int y) t) =>
        t.x == 0 ? t.y : GCD(minMax(t.x, t.y % t.x));

    private static int GCD(int x, int y) => GCD(minMax(x, y));

    private static bool Coprime(int x, int y) => GCD(x, y) == 1;

    public static string Encode(string plainText, int a, int b)
    {
        if (!Coprime(a, ALPHA_SIZE)) throw new ArgumentException();
        return plainText.Transcode(x => a * x + b).Slices(5).Join(" ");
    }

    public static string Decode(string cipheredText, int a, int b)
    {
        if (!Coprime(a, ALPHA_SIZE)) throw new ArgumentException();
        int mmi;
        for (mmi = 1; (a * mmi) % ALPHA_SIZE != 1 && mmi < ALPHA_SIZE; mmi++);
        Func<int, int> decode(int _mmi) =>
            (int index) => _mmi * (index - b + 2 * ALPHA_SIZE);

        return cipheredText.Transcode(decode(mmi));
    }
}

static class DebugExtensions
{
    public static T Dump<T>(this T obj, Func<T, string> formatter, string message = null)
    {
        var s = message == null ? formatter(obj) : $"{message}: {formatter(obj)}";
        Console.WriteLine(s);
        return obj;
    }

    public static T Dump<T>(this T obj, string message = null) => obj.Dump(x => x.ToString(), message);
}

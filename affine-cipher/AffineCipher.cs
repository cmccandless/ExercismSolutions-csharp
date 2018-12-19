using System;
using System.Collections.Generic;
using System.Linq;

public static class AffineCipher
{
    private static char[] letters = Enumerable.Range(0, 26).Select(i => (char)(i + 'a')).ToArray();
    private static int M => letters.Length;
    private static (int x, int y) minMax(int x, int y) => (Math.Min(x, y), Math.Max(x, y));
    private static int GCD((int x, int y) t) =>
        t.x == 0 ? t.y : GCD(minMax(t.x, t.y % t.x));
    private static int GCD(int x, int y) => GCD(minMax(x, y));
    private static bool Coprime(int x, int y) => GCD(x, y) == 1;
    private static string Transcode(string text, Func<char, int, string> transcoder) =>
        string.Join("", text.ToLower().Where(Char.IsLetterOrDigit).Select(transcoder)).Trim();
    public static string Encode(string plainText, int a, int b)
    {
        if (!Coprime(a, M)) throw new ArgumentException();
        string encode(char ch, int i) =>
            (
                Char.IsLetter(ch) ?
                (char)(((a * (ch - 'a') + b) % M) + 'a') :
                ch
            ).ToString() +
            ((i != 0 && i % 5 == 4) ? " " : "");
        return Transcode(plainText, encode);
    }
    private static T Dump<T>(this T obj, string msg="")
    {
        Console.Out.WriteLine(msg + obj.ToString());
        return obj;
    }

    public static string Decode(string cipheredText, int a, int b)
    {
        if (!Coprime(a, M)) throw new ArgumentException();
        int mmi;
        for (mmi = 1; (a * mmi) % M != 1 && mmi < M; mmi++);
        Func<char, int, string> decode(int _mmi) =>
            (char ch, int i) =>
                (
                    Char.IsLetter(ch)
                    ? (
                        (char)(
                            ((_mmi * (ch - 'a' - b + 2 * M)) % M) + 'a'
                        )
                    )
                    : ch
                ).ToString();
        return Transcode(cipheredText, decode(mmi));
    }
}

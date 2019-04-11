using System.Linq;

public class TwelveDays
{
    private enum Ordinal
    {
        first = 1,
        second = 2,
        third = 3,
        fourth = 4,
        fifth = 5,
        sixth = 6,
        seventh = 7,
        eighth = 8,
        ninth = 9,
        tenth = 10,
        eleventh = 11,
        twelfth = 12,
    }

    private static string[] verses = new string[]
    {
        "and a Partridge in a Pear Tree.",
        "two Turtle Doves",
        "three French Hens",
        "four Calling Birds",
        "five Gold Rings",
        "six Geese-a-Laying",
        "seven Swans-a-Swimming",
        "eight Maids-a-Milking",
        "nine Ladies Dancing",
        "ten Lords-a-Leaping",
        "eleven Pipers Piping",
        "twelve Drummers Drumming",
    };

    private static string OnTheDay(int verseNum) =>
        $"On the {(Ordinal)verseNum} day of Christmas my true love gave to me:";

    private static string Parade(int verseNum) =>
        string.Join(", ", Enumerable.Range(1, verseNum).Select(i => verses[verseNum - i]));

    public static string Recite(int verseNum) => $"{OnTheDay(verseNum)} {Parade(verseNum)}".Replace(": and", ":");

    public static string Recite(int startVerse, int stopVerse) =>
        string.Join("\n", Enumerable.Range(startVerse, stopVerse - startVerse + 1).Select(i => Recite(i)));
}

using System.Collections.Generic;
using System.Linq;

public static class BeerSong
{ 
    private static string Bottles(int verse) => verse == 0 ?
        "No more bottles of beer" :
        $"{(verse + 100) % 100} bottle{(verse != 1 ? "s" : string.Empty)} of beer";
    public static string Verse(int verse) => Verse(verse, Bottles(verse));

    private static string Action(int verse) => verse == 0 ?
        "Go to the store and buy some more" :
        $"Take {(verse == 1 ? "it" : "one")} down and pass it around";

    private static string Verse(int verse, string bottles) =>
        $"{bottles} on the wall, {bottles.ToLower()}.\n" +
        $"{Action(verse)}, {Bottles(verse - 1).ToLower()} on the wall.";

    public static string Recite(int start, int count) =>
        string.Join("\n\n", Enumerable.Range(start - count + 1, count).Reverse().Select(Verse));
}

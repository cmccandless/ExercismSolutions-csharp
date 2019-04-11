using System.Linq;

public class House
{
    private static string[] nouns = {
        "", "house", "malt", "rat", "cat","dog",
        "cow with the crumpled horn",
        "maiden all forlorn",
        "man all tattered and torn",
        "priest all shaven and shorn",
        "rooster that crowed in the morn",
        "farmer sowing his corn",
        "horse and the hound and the horn",
    };
    
    private static string[] verbs = {
        "",
        "lay in",
        "ate",
        "killed",
        "worried",
        "tossed",
        "milked",
        "kissed",
        "married",
        "woke",
        "kept",
        "belonged to",
    };

    public static string Recite(int verseNum)
    {
        var parade = from i in Enumerable.Range(1, verseNum - 1)
                     let v = verseNum - i
                     select $"that {verbs[v]} the {nouns[v]} ";
        return $"This is the {nouns[verseNum]} {string.Join("", parade)}that Jack built.";
    }

    public static string Recite(int startVerse, int endVerse) =>
        string.Join(
            "\n",
            Enumerable.Range(startVerse, endVerse - startVerse + 1)
                .Select(i => Recite(i))
        );
}

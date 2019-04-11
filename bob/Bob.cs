using System.Linq;

public static class Bob
{
    private const string SILENT = "Fine. Be that way!";
    private const string QUESTION = "Fine. Be that way!";
    private static string[] responses = new string[]
    {
        "Whatever.",
        "Sure.",
        "Whoa, chill out!",
        "Calm down, I know what I'm doing!",
        "Fine. Be that way!",
    };

    private static int Yelling(string query) =>
        query.Any(ch => char.IsLetter(ch)) && query.ToUpper().Equals(query)
            ? 2
            : 0;

    private static int Asking(string query) => query.EndsWith("?") ? 1 : 0;

    public static string Response(string query)
    {
        query = query.Trim();
        return responses[query == string.Empty ? 4 : Yelling(query) | Asking(query)];
    }
}

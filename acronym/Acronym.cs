using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class Acronym
{
    private static Regex rgxSignificant =
        new Regex("^[A-Z]|(?<=[^A-Z])[A-Z]|(?<=[ -])[a-z]");
    public static string Abbreviate(string phrase) => string.Join(
        "",
        rgxSignificant.Matches(phrase.Replace("'", ""))
            .Select(m => m.Groups[0].Value)
    ).ToUpper();
}

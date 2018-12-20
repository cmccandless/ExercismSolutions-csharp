using System;
using System.Collections.Generic;
using System.Linq;

public class DndCharacter
{
    private static string[] validAbilities = new[]{
        "strength",
        "dexterity",
        "constitution",
        "intelligence",
        "wisdom",
        "charisma"
    };

    private static Random rand = new Random();

    private Dictionary<string, int> abilities = new Dictionary<string, int>();

    public int Strength => this.abilities["strength"];
    public int Dexterity => this.abilities["dexterity"];
    public int Constitution => this.abilities["constitution"];
    public int Intelligence => this.abilities["intelligence"];
    public int Wisdom => this.abilities["wisdom"];
    public int Charisma => this.abilities["charisma"];
    public int Hitpoints => Modifier(Constitution) + 10;

    public static int Modifier(int score) => (int)(score / 2) - 5;

    private static int Roll(int count, int drop=0, int sides=6) =>
        Enumerable.Range(0, count)
            .Select(_ => rand.Next(1, sides))
            .OrderBy(d => d)
            .Skip(drop).Sum();

    public static int Ability() => Roll(4, 1);

    public static DndCharacter Generate() =>
        new DndCharacter(validAbilities.ToDictionary(s => s, _ => Ability()));

    public DndCharacter(Dictionary<string, int> abilities)
    {
        foreach (var kvp in abilities)
            this.abilities[kvp.Key] = kvp.Value;
    }
}

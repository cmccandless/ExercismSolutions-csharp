using System;
using System.Collections.Generic;
using System.Linq;

public enum YachtCategory
{
    Ones = 1,
    Twos = 2,
    Threes = 3,
    Fours = 4,
    Fives = 5,
    Sixes = 6,
    FullHouse = 7,
    FourOfAKind = 8,
    LittleStraight = 9,
    BigStraight = 10,
    Choice = 11,
    Yacht = 12,
}

public static class YachtGame
{
    public static IOrderedEnumerable<T> Sorted<T>(this IEnumerable<T> col) => col.OrderBy(x => x);
    public static IEnumerable<IGrouping<T, T>> Grouped<T>(this IEnumerable<T> col) => col.GroupBy(x => x, x => x);
    public static int SumWhere(this IEnumerable<int> col, Func<int, bool> filter) => col.Where(filter).Sum();
    public static int Score(int[] dice, YachtCategory category)
    {
        string counts = String.Join(",", from grp in dice.Grouped() let c = grp.Count() orderby c select c.ToString());
        bool isStraight() => dice.Sorted().SequenceEqual(Enumerable.Range((int)category - 8, 5));
        switch(category)
        {
            case YachtCategory.FullHouse:
                if (counts == "2,3") return dice.Sum();
                break;
            case YachtCategory.FourOfAKind: 
                switch(counts)
                {
                    case "1,4":
                    case "5":
                        return 4 * dice.Grouped().First(g => g.Count() > 1).Key;
                }
                break;
            case YachtCategory.LittleStraight:
            case YachtCategory.BigStraight:
                if (isStraight()) return 30;
                break;
            case YachtCategory.Choice: return dice.Sum();
            case YachtCategory.Yacht:
                if (dice.Distinct().Count() == 1) return 50;
                break;
        }
        return dice.SumWhere(d => d == (int)category);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;

public class HighScores
{
    private List<int> scores;
    public HighScores(List<int> list)
    {
        this.scores = new List<int>(list);
    }

    public List<int> Scores()
    {
        return scores.ToList();
    }

    public int Latest()
    {
        return scores[scores.Count - 1];
    }

    public int PersonalBest()
    {
        return scores.Max();
    }

    public List<int> PersonalTop()
    {
        return scores.OrderByDescending(i => i).Take(3).ToList();
    }

    public string Report()
    {
        var latest = Latest();
        var diff = PersonalBest() - latest;
        var diff_msg = diff > 0 ? $"{diff} short of " : "";
        return $"Your latest score was {latest}. That's {diff_msg}your personal best!";
    }
}
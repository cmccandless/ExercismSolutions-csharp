
using System;
using System.Collections.Generic;
using System.Linq;

public class WordSearch
{
    public WordSearch(string grid) =>
        this.Grid = grid.Split('\n').Select(line => line.ToArray()).ToArray();

    private char[][] Grid { get; }

    private bool IsValidPoint(int x, int y) => y.IsBetween(0, Grid.Length) && x.IsBetween(0, Grid[y].Length);

    private bool Match(string word, (int x, int y) start, int xStep, int yStep, out (int x, int y) stop)
    {
        stop = start;
        for (int i = 0; i < word.Length; i++)
        {
            if (!IsValidPoint(stop.x, stop.y) || word[i] != Grid[stop.y][stop.x])
                return false;
            stop = stop.Increment(xStep, yStep);
        }
        stop = stop.Increment(1 - xStep, 1 - yStep);
        return true;
    }

    private bool FindAt(string word, (int x, int y) start, out ((int x, int y) start, (int x, int y) stop) result)
    {
        result = (start.Increment(), start);
        for (int dy = -1; dy < 2; dy++)
        {
            for (int dx = -1; dx < 2; dx++)
            {
                if (dy == 0 && dx == 0) continue;
                if (Match(word, start, dx, dy, out result.stop))
                    return true;
            }
        }
        return false;
    }

    private ((int, int), (int, int))? Find(string word)
    {
        ((int, int), (int, int)) result;
        for (int y = 0; y < Grid.Length; y++)
            for (int x = 0; x < Grid[y].Length; x++)
                if (FindAt(word, (x, y), out result))
                    return result;
        return null;
    }

    public Dictionary<string, ((int, int), (int, int))?> Search(string[] wordsToSearchFor) =>
        wordsToSearchFor.ToDictionary(word => word, Find);
}

static class Extensions
{
    public static bool IsBetween(this int x, int minInclusive, int maxExclusive) => minInclusive <= x && x < maxExclusive;
    public static (int x, int y) Increment(this (int x, int y) p, int dx = 1, int dy = 1) => (x: p.x + dx, y: p.y + dy);
}
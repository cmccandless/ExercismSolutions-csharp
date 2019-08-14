using System;
using System.Collections.Generic;
using System.Linq;
using static MoreLinq.Extensions.PermutationsExtension;

public class Alphametics
{
	private static HashSet<string> Operators = new [] {"+", "=="}.ToHashSet();
	private const string ERR_START_WITH_ZERO = "word cannot start with zero";
	private const string ERR_NO_SOLUTION = "no solution";

	public static bool TrySolution(char[] letters, int[] solution, IEnumerable<string> words)
	{
		int mapLetterToDigit(char ch) => solution[letters.IndexOf(ch)];
		long sub(string word) =>
			word.Select(mapLetterToDigit).Aggregate((long)0, (total, digit) => total * 10 + digit); 
		var parts = words.Select(sub).AsStack();
		return parts.Pop() == parts.Sum();
	}

	private static Dictionary<char, int> CreateSolutionDict(char[] letters, int[] solution) =>
		letters.Zip(solution, (l, d) => (l, d)).ToDictionary(o => o.l, o => o.d);

	public static Dictionary<char, int> Solve(string expr)
	{
		var words = expr.Split(' ').Where(w => !Operators.Contains(w)).ToArray();
		var letters = expr.Where(Char.IsLetter).Distinct().ToArray();
		var cannotBeZero = words.Select(w => letters.IndexOf(w[0])).ToHashSet();
		var solution = Enumerable.Range(0, 10)
			.Reverse()
			.Permutations(letters.Length)
			.Select(p => p.ToArray())
			.Where(p => !cannotBeZero.Contains(p.IndexOf(0)))
			.FirstOrDefault(p => TrySolution(letters, p, words));
		if (solution != null) return CreateSolutionDict(letters, solution);
		throw new ArgumentException(ERR_NO_SOLUTION);
	}
}

public static class Extensions
{
	public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> col, int length)
	{
		if (length == 0) return new[] { new T[0] };
		return col.SelectMany((e, i) => col.Skip(i + 1).Combinations(length - 1).Select(c => (new[] {e}).Concat(c)));
	}

	public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> col, int length) =>
		col.Combinations(length).SelectMany(c => c.Permutations());

	public static Stack<T> AsStack<T>(this IEnumerable<T> col) => new Stack<T>(col);

	public static int IndexOf<T>(this T[] arr, T x) => Array.IndexOf(arr, x);
}

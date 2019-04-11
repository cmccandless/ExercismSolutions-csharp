using System;
using System.Collections.Generic;
using System.Linq;

public class Alphametics
{
	public static Dictionary<char, int> Solve(string expr, Dictionary<char, int> dict = null, HashSet<char> invalidZero = null)
	{
		if (dict == null) dict = new Dictionary<char, int>();
		if (invalidZero == null)
			invalidZero = new HashSet<char>(expr.Split(' ').Where(w => !w.Contains('+') && !w.Contains('=')).Select(w => w[0]));
		var letters = new HashSet<char>(expr.Where(c => char.IsLetter(c)));
		var attempted = letters.FirstOrDefault(ch => !dict.ContainsKey(ch));
		if (attempted == default(char))
		{
			var parts = expr.Split(new[] { " == " }, StringSplitOptions.None);
			if (dict[parts[1][0]] == 0) return null;
			var equal = long.Parse(string.Join("", parts[1].Select(ch => dict[ch])));
			var terms = parts[0].Split(new[] { " + " }, StringSplitOptions.None);
			if (terms.Any(t => dict[t[0]] == 0)) return null;
			var termSum = terms.Sum(w => long.Parse(string.Join("", w.Select(ch => dict[ch]))));
			if (equal.Equals(termSum)) return dict;
		}
		else
		{
			var values = new HashSet<int>(dict.Values);
			for (int x = invalidZero.Contains(attempted) ? 1 : 0; x < 10; x++)
			{
				if (values.Contains(x)) continue;
				dict[attempted] = x;
				var result = Solve(expr, dict, invalidZero);
				if (result != null) return dict;
			}
			dict.Remove(attempted);
		}
		if (dict.Count == 0) throw new ArgumentException();
		return null;
	}

	private static string Sub(string str, Dictionary<char, char> mappings)
	{
		foreach (var kvp in mappings)
		{
			str = str.Replace(kvp.Key, kvp.Value);
		}
		return str;
	}

/*	public static Dictionary<char, int> Solve(string expr)
	{
		// var words = expr.Split(' ').Where(w => w != "+" && w != "==").ToArray();
		var letters = new HashSet<char>(expr.ToCharArray().Where(ch => !"+= ".Contains(ch)));
		var solutions = "0123456789"
			.Permutations(letters.Count)
			.Select(perm =>
				letters.Zip(
					perm,
					(letter, digit) => (k: letter, v: digit)
				).ToDictionary(t => t.k, t => t.v)
			);
		var solution = (
			from sol in solutions
			let words = Sub(expr, sol)
			.Split(' ')
			.Except(new[] { "+", "==" })
			where words.All(w => w[0] != '0')
			let numbers = new Stack<long>(words.Select(long.Parse))
			where numbers.Pop() == numbers.Sum()
			select sol.ToDictionary(
				kvp => kvp.Key,
				kvp => kvp.Value - '0'
			)
		).FirstOrDefault();
		if (solution != null) return solution;
		// foreach (var solution in solutions)
		// {
		// 	// Console.WriteLine(solution.DictToString());
		// 	var words = Sub(expr, solution)
		// 		.Split(' ')
		// 		.Except(new[] { "+", "==" });
		// 	var numbers = new Stack<long>();
		// 	var doContinue = false;
		// 	foreach (var word in words)
		// 	{
		// 		if (word[0] == '0')
		// 		{
		// 			doContinue = true;
		// 			break;
		// 		}
		// 		numbers.Push(long.Parse(word));
		// 	}
		// 	if (!doContinue && numbers.Pop() == numbers.Sum())
		// 	{
		// 		return solution.ToDictionary(kvp => kvp.Key, kvp => kvp.Value - '0');
		// 	}
		// }
		throw new ArgumentException();
	}*/
}

public static class Extensions
{
	// Source: https://stackoverflow.com/a/10630026/3229611
	public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> list, int length)
	{
		if (length == 1) return list.Select(t => new T[] { t });

		return list.Permutations(length - 1)
			.SelectMany(t => list.Except(t),
				(t1, t2) => t1.Append(t2));
	}

	public static string DictToString<TKey, TValue>(this Dictionary<TKey, TValue> dict)
	{
		return "{ " + string.Join(", ",
			dict.Select(kvp => $"{kvp.Key}: {kvp.Value}")
		) + " }";
	}

	public static string EnumToString<T>(this IEnumerable<T> col)
	{
		return "[ " + string.Join(", ", col.Select(x => x.ToString())) + " ]";
	}
}

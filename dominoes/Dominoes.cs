using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DominoChain = System.Collections.Generic.List<(int left, int right)>;

public class Dominoes
{
	public static bool CanChain((int left, int right)[] dominoes)
	{
		DominoChain results;
		return dominoes.Length == 0 || TryChain(dominoes, out results) && results.Any(d => d.right == d.left);
	}
	private static bool TryChain((int left, int right)[] dominoes, out DominoChain c)
	{
		c = new DominoChain();
		if (dominoes.Length < 2)
		{
			c.AddRange(dominoes);
			return true;
		}
		if (dominoes.Length == 2) return TryChain(dominoes[0], dominoes[1], out c);
		for (int i = 0; i < dominoes.Length; i++)
		{
			var set = dominoes.ToList();
			var t1 = set[i];
			set.RemoveAt(i);
			DominoChain subResults;
			if (!TryChain(set.ToArray(), out subResults)) continue;
			foreach (var t2 in subResults)
			{
				DominoChain matches;
				if (TryChain(t1, t2, out matches)) c.AddRange(matches);
			}
		}
		return c.Count > 0;
	}
	private static bool TryChain((int left, int right) a, (int left, int right) b, out DominoChain c)
	{
		c = new DominoChain();
		if (a.left == b.left) c.Add((a.right, b.right));
		if (a.left == b.right) c.Add((a.right, b.left));
		if (a.right == b.left) c.Add((a.left, b.right));
		if (a.right == b.right) c.Add((a.left, b.left));
		return c.Count > 0;
	}
}


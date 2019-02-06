using System;
using System.Collections.Generic;
using System.Linq;

public static class Poker
{
	// From https://stackoverflow.com/a/10555037
	public static IEnumerable<IEnumerable<T>> Transpose<T>(
		this IEnumerable<IEnumerable<T>> @this)
		{
			var enumerators = @this.Select(t => t.GetEnumerator())
				.Where(e => e.MoveNext());
			while (enumerators.Any())
			{
				yield return enumerators.Select(e => e.Current);
				enumerators = enumerators.Where(e => e.MoveNext());
			}
		}

	public static int CompareTo<T>(this T[] @this, T[] other) where T : IComparable<T>
	{
		var c = @this.Length.CompareTo(other.Length);
		if (c != 0) return c;
		for (int i=0; i < @this.Length; i++)
		{
			c = @this[i].CompareTo(other[i]);
			if (c != 0) return c;
		}
		return 0;
	}

	public static int CompareTo<T>(this T[][] @this, T[][] other) where T : IComparable<T>
	{
		var c = @this.Length.CompareTo(other.Length);
		if (c != 0) return c;
		for (int i=0; i < @this.Length; i++)
		{
			c = @this[i].CompareTo(other[i]);
			if (c != 0) return c;
		}
		return 0;
	}

	class Card : IComparable<Card>
	{
		private static string Values = "_123456789TJQKA";
		public readonly int Value;
		public readonly char Suit;
		public Card(string cardStr)
		{
			Value = cardStr.StartsWith("10") ? 10 :
				Values.IndexOf(cardStr[0]);
			Suit = cardStr[cardStr.Length - 1];
		}
		public override string ToString()
		{
			switch(Value)
			{
				case 10: return $"10{Suit}";
				case 1: return $"A{Suit}";
				default: return $"{Values[Value]}{Suit}";
			}
		}
		public int CompareTo(Card other)
		{
			var c = Value.CompareTo(other.Value);
			return c == 0 ? Suit.CompareTo(other.Suit) : c;
		}

		public Card LowAce => Value == 14 ? new Card($"1{Suit}") : this;
	}
	class Hand : IComparable<Hand>
	{
		private static string[] Types = new[]{
			"11111", "2111", "221", "311", "S", "F", "32", "41", "SF"
		};
		public readonly Card[] Cards;

        public Hand(string handStr)
		{
			Cards = handStr.Split(' ').Select(c => new Card(c)).ToArray();
			if (CountsStr(0) == "145432")
			{
				Cards = Cards.Select(c => c.LowAce).ToArray();
			}
		}

		public override string ToString() => string.Join(
			" ", Cards.Select(c => c.ToString())
		);

        private int[][] GetCounts()
        {
            return (from card in Cards
                    group card by card.Value into g
                    let gSize = g.ToArray().Length
                    orderby gSize descending, g.Key descending
                    select new[] { g.Key, gSize }
			).ToArray();
        }

        private string CountsStr(int index) => string.Join(
			"", GetCounts().Transpose().ElementAt(index).Select(x => x.ToString())
		);

		private bool IsStraight()
		{
			var values = GetCounts().Select(c => c[0]).OrderBy(x => x).ToArray();
			if (values.Length != 5) return false;
			if (values[4] - values[0] == 4) return true;
			// Low straight with Ace
			return (
				values[4] == 14 &&
				values[0] == 2 &&
				values[3] - values[0] == 3
			);
		}
		private bool IsFlush() => Cards.Select(c => c.Suit).Distinct().Count() == 1;

		private int Classify()
		{
			var countStr = IsStraight() ? "S" : "";
			if (IsFlush()) countStr += "F";
			if (countStr == "") countStr = CountsStr(1);
			return Array.IndexOf(Types, countStr);
		}

        public int CompareTo(Hand other)
        {
            var c = Classify().CompareTo(other.Classify());
			if (c != 0) return c;
			return GetCounts().CompareTo(other.GetCounts());
        }
    }
	private static T[] KeepBest<T>(T[] left, T right) where T : IComparable<T>
	{
		switch(left[0].CompareTo(right))
		{
			case 0: return left.Concat(new[] {right}).ToArray();
			case -1: return new[] {right};
			default: return left;
		}
	}
	public static string[] BestHands(string[] handStrs)
	{
		if (handStrs.Length == 0) return handStrs;
		var hands = handStrs.Select(h => new Hand(h));
		return hands.Skip(1).Aggregate(hands.Take(1).ToArray(), KeepBest<Hand>)
			.Select(h => h.ToString()).ToArray();
	}
}
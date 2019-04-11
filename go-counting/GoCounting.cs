using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Owner { None, Black, White, Disputed }
class GoCounting
{
	private static Dictionary<char, Owner> tokens = new Dictionary<char, Owner>
	{
		[' '] = Owner.None,
		['B'] = Owner.Black,
		['W'] = Owner.White,
	};

	private Owner[][] board;

	public GoCounting(string board)
	{
		this.board = (
			from line in board.Split('\n')
			select (
				from ch in line
				select tokens[ch]
			).ToArray()
		).ToArray();
	}

	public (Owner owner, (int, int)[] points) Territory((int x, int y) targetPoint)
	{
		var point = new Point(targetPoint);
		if (!point.ValidWith(board))
			throw new ArgumentException("out of range");
		if (board[point.Y][point.X] != Owner.None)
			return (Owner.None, Array.Empty<(int, int)>());
		var player = Owner.None;
		var border = new Dictionary<Point, Owner>();
		var pending = new HashSet<Point>();
		var invalid = new HashSet<Point>();
		var queue = new Queue<Point>(new[] {point});
		while (queue.Any())
		{
			point = queue.Dequeue();
			if (board[point.Y][point.X] != Owner.None) continue;
			var n = point.Neighbors;
			var t = Owner.None;
			foreach (var x in n)
			{
				try { t |= board[x.Y][x.X]; }
				catch (IndexOutOfRangeException) { invalid.Add(x); }
			}
			switch (t)
			{
				case Owner.Disputed:
					player = t;
					goto case Owner.Black;
				case Owner.None:
					pending.Add(point);
					break;
				case Owner.Black:
				case Owner.White:
					border[point] = t;
					break;
			}
			foreach (var x in n.Except(invalid).Except(border.Keys).Except(pending).Except(queue))
			{
				queue.Enqueue(x);
			}
		}
		if (player == Owner.None)
		{
			var g = border.GroupBy(kvp => kvp.Value).ToArray();
			switch(g.Length)
			{
				case 0: break; // Leave as Owner.None
				case 1: player = g[0].Key; break;
				default: player = Owner.Disputed; break;
			}
		}
		return (
			(Owner)((int)player % 3),
			(
				from q in pending.Union(border.Keys)
				where q.ValidWith(board)
				where board[q.Y][q.X] == Owner.None
				orderby q
				select q.AsTuple()
			).ToArray()
		);
	}

	public Dictionary<Owner, (int x, int y)[]> Territories()
	{
		var dict = new Dictionary<Owner, HashSet<(int, int)>>();
		dict[Owner.Black] = new HashSet<(int, int)>();
		dict[Owner.White] = new HashSet<(int, int)>();
		dict[Owner.None] = new HashSet<(int, int)>();
		var visited = new HashSet<(int, int)>();
		for (int y = 0; y < board.Length; y++)
			for (int x = 0; x < board[y].Length; x++)
			{
				var point = (x, y);
				if (visited.Contains(point)) continue;
				var (owner, points) = Territory(point);
				foreach (var n in points)
				{
					dict[owner].Add(n);
					visited.Add(n);
				}
				visited.Add(point);
			}
		return dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
	}
}
public class Point : IComparable<Point>, IEquatable<Point>
{
	public int X { get; set; }
	public int Y { get; set; }

	public Point((int x, int y) point)
	{
		this.X = point.x;
		this.Y = point.y;
	}

	public static Point operator +(Point a, Point b) => new Point((a.X + b.X, a.Y + b.Y));

	public static Point operator -(Point a, Point b) => new Point((a.X - b.X, a.Y - b.Y));

	public static readonly Point Origin = new Point((0, 0));

	public override string ToString() => $"({X},{Y})";

	private static int[] nums = new[] { -1, 0, 1 };

	private static Point[] neighborsRelative = (from x in nums
												from y in nums
												where (x == 0 || y == 0) && x != y
												select new Point((x, y))).ToArray();
	public HashSet<Point> Neighbors => new HashSet<Point>(from n in neighborsRelative select this + n);

	public int CompareTo(Point other) => (this.X.CompareTo(other.X) << 16) + this.Y.CompareTo(other.Y);

	public bool Equals(Point other) => this.X == other.X && this.Y == other.Y;

	public override bool Equals(object obj) => obj is Point p && this.Equals(p);

	public override int GetHashCode() => (this.X << 16) + this.Y;

	public (int x, int y) AsTuple() => (this.X, this.Y);

	public bool ValidWith<T>(T[][] grid) => this.Y >= 0 && this.Y < grid.Length && this.X >= 0 && this.X < grid[this.Y].Length;
}

static class Extensions
{
	static IEnumerable<T> WhereNot<T>(this IEnumerable<T> col, Func<T, bool> predicate) => col.Where(x => !predicate(x));
}
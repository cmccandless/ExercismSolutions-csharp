﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercism.connect
{
public class Connect
{
	public enum Winner { None, Black, White }
	private readonly char[][] board;
	public Connect(string board)
	{
		this.board = board.Split('\n').Select(line => line.ToCharArray()).ToArray();
	}
	public Winner Result()
	{
		var token = new[] { '.', 'X', 'O' };
		var nums = new[] { -1, 0, 1 };
		var children = (from x in nums
						from y in nums
						where x != y
						select new Point(x, y)).ToArray();
		var visited = new bool[board.Length, board[0].Length];
		var s = new Stack<Tuple<Point, char>>();
		for (int y = 0; y < board.Length; y++) s.Push(Tuple.Create(new Point(0, y), token[(int)Winner.Black]));
		for (int x = 0; x < board[0].Length; x++) s.Push(Tuple.Create(new Point(x, 0), token[(int)Winner.White]));
		while (s.Any())
		{
			var tup = s.Pop();
			var p = tup.Item1;
			var t = tup.Item2;
			try { if (board[p.Y][p.X] != t) continue; }
			catch (IndexOutOfRangeException) { continue; }
			if (visited[p.Y, p.X]) continue;
			if (t == token[(int)Winner.Black] && p.X == board[0].Length - 1) return Winner.Black;
			else if (t == token[(int)Winner.White] && p.Y == board.Length - 1) return Winner.White;
			visited[p.Y, p.X] = true;
			foreach (var c in children) s.Push(Tuple.Create(c + p, t));
		}
		return Winner.None;
	}
}
public class Point
{
	public int X { get; set; }
	public int Y { get; set; }
	public Point(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}
	public override bool Equals(object obj)
	{
		var other = obj as Point;
		if (other == null) return false;
		return this.X == other.X && this.Y == other.Y;
	}
	public override int GetHashCode() { return base.GetHashCode(); }
	public static Point operator +(Point a, Point b)
	{
		return new Point(a.X + b.X, a.Y + b.Y);
	}
	public override string ToString()
	{
		return string.Format("({0},{1})", X, Y);
	}
}
}
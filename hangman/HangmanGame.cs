﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HangmanGame
{
	public enum Status { Busy, Lose, Win }

	public delegate void OnStateChanged(object sender, HangmanState state);
	public OnStateChanged StateChanged;

	internal readonly string Word;
	internal List<char> Misses = new List<char>();
	internal HashSet<char> Hits = new HashSet<char>();
	private HangmanState state;
	public HangmanGame(string word) { this.Word = word; } 
	public void Start()
	{
		this.state = new HangmanState(this);
		if (StateChanged != null) StateChanged(this, state);
	}
	public void Guess(char ch)
	{
		if (Word.Contains(ch) && !Hits.Contains(ch)) Hits.Add(ch);
		else Misses.Add(ch);
		if (StateChanged != null) StateChanged(this, state);
	}
}
public class HangmanState
{
	private readonly HangmanGame game;
	public HangmanGame.Status Status
	{
		get
		{
			if (MaskedWord.Equals(game.Word))
				return HangmanGame.Status.Win;
			else if (RemainingGuesses <= 0)
				return HangmanGame.Status.Lose;
			return HangmanGame.Status.Busy;
		}
	}
	public int RemainingGuesses { get { return 9 - game.Misses.Count; } }
	public string MaskedWord { get { return string.Join(string.Empty, 
		game.Word.Select(ch=>game.Hits.Contains(ch)?ch:'_')); } }
	public HangmanState(HangmanGame game) { this.game = game; }
}
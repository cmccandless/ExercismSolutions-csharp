using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;

public class HangmanState
{
    public string MaskedWord { get; }
    public ImmutableHashSet<char> GuessedChars { get; }
    public int RemainingGuesses { get; }

    public HangmanState(string maskedWord, ImmutableHashSet<char> guessedChars, int remainingGuesses)
    {
        MaskedWord = maskedWord;
        GuessedChars = guessedChars;
        RemainingGuesses = remainingGuesses;
    }
}

public class TooManyGuessesException : Exception
{
}

public class Hangman
{
	private const int MaxGuesses = 9;

    public IObservable<HangmanState> StateObservable { get; }
    public IObserver<char> GuessObserver { get; }

    public Hangman(string word)
    {
		var emptySet = new HashSet<char>();
		var subject = new BehaviorSubject<HangmanState>(new HangmanState(
			Mask(word, emptySet),
			emptySet.ToImmutableHashSet(),
			MaxGuesses
		));

		this.StateObservable = subject;

		this.GuessObserver = Observer.Create<char>(ch =>
			{
				var guessedChars = new HashSet<char>(subject.Value.GuessedChars);
				var remainingGuesses = subject.Value.RemainingGuesses;
				if (!word.Contains(ch) || guessedChars.Contains(ch))
					remainingGuesses--;
				guessedChars.Add(ch);
				var masked = Mask(word, guessedChars);
				if (masked == word)
				{
					subject.OnCompleted();
				}
				else if (remainingGuesses < 0) // Game over
				{
					subject.OnError(new TooManyGuessesException());
				}
				else
				{
					subject.OnNext(new HangmanState(
						masked,
						guessedChars.ToImmutableHashSet(),
						remainingGuesses
					));
				}
			}
		);
    }

	private static string Mask(string word, HashSet<char> guessedChars) => new string(
		word.ToCharArray().Select(ch => guessedChars.Contains(ch) ? ch : '_').ToArray()
	);
}

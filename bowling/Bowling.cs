using System;
using System.Collections.Generic;
using System.Linq;

public class BowlingGame
{
    private const string ERR_PINS_NEGATIVE = "pins cannot be negative";
    private const string ERR_PINS_OVER_TEN = "pins cannot be greater than 10";
    private const string ERR_BONUS_OVER_TEN =
        "bonus rolls cannot exceed 10 without bonus strike";
    private const string ERR_FRAME_OVER_TEN = "frame total cannot exceed 10";
    private const string ERR_GAME_COMPLETE = "cannot roll on complete game";
    private const string ERR_GAME_INCOMPLETE = "cannot score incomplete game";

    private List<int> rolls = new List<int>();

    public void Roll(int pins)
    {
        if (pins < 0) throw new ArgumentException(ERR_PINS_NEGATIVE);
        if (pins > 10) throw new ArgumentException(ERR_PINS_OVER_TEN);
        rolls.Add(pins);
        UnsafeScore();
    }

    private int UnsafeScore(bool checkIfComplete = false)
    {
        var score = 0;
        var q = new Queue<int>(rolls);
        int? safeDequeue()
        {
            int item;
            if (!q.TryDequeue(out item))
            {
                if (checkIfComplete)
                    throw new ArgumentException(ERR_GAME_INCOMPLETE);
                return null;
            }
            return item;
        }
        for (int i = 1; i <= 10; i++)
        {
            var ball1 = safeDequeue();
            if (!ball1.HasValue) break;
            score += ball1.Value;
            // Strike
            if (ball1 == 10)
            {
                score += q.Take(2).Sum();
                if (i == 10)
                {
                    var bonus1 = safeDequeue();
                    if (!bonus1.HasValue) break;
                    var bonus2 = safeDequeue();
                    if (!bonus2.HasValue) break;
                    if (bonus1.Value < 10 && bonus1.Value + bonus2.Value > 10)
                    {
                        throw new ArgumentException(ERR_BONUS_OVER_TEN);
                    }
                }
            }
            else
            {
                var ball2 = safeDequeue();
                if (!ball2.HasValue) break;
                if (ball1.Value + ball2.Value > 10)
                    throw new ArgumentException(ERR_FRAME_OVER_TEN);
                score += ball2.Value;
                // Spare
                if (ball1.Value + ball2.Value == 10)
                {
                    score += q.FirstOrDefault();
                    if (i == 10) safeDequeue();
                }
            }
        }
        if (q.Any()) throw new ArgumentException(ERR_GAME_COMPLETE);
        return score;
    }

    public int Score() => UnsafeScore(true);
}

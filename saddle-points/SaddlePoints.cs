using System;
using System.Collections.Generic;
using System.Linq;

public class SaddlePoints
{
    private readonly int[,] Matrix;
    private readonly int RowCount;
    private readonly int ColCount;
    private readonly int[] RowMax;
    private readonly int[] ColMin;
    public SaddlePoints(int[,] values)
    {
        Matrix = values;
        RowCount = Matrix.GetUpperBound(0) + 1;
        ColCount = Matrix.GetUpperBound(1) + 1;
        RowMax = new int[RowCount];
        ColMin = new int[ColCount];
        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColCount; c++)
            {
                var value = Matrix[r, c];
                if (c == 0 || value > RowMax[r]) RowMax[r] = value;
                if (r == 0 || value < ColMin[c]) ColMin[c] = value;
            }
        }
    }

    public IEnumerable<(int, int)> Calculate() =>
        from r in Enumerable.Range(0, RowCount)
        join c in Enumerable.Range(0, ColCount)
        on RowMax[r] equals ColMin[c]
        select (r + 1, c + 1);

    public static (int, int)[] Calculate(int[,] matrix) => new SaddlePoints(matrix).Calculate().ToArray();
}

using System;

public static class Darts
{
    public static int Score(double x, double y)
    {
        var dist = Math.Sqrt(x * x + y * y);
        return dist <= 1 ? 10 :
            dist <= 5 ? 5 :
            dist <= 10 ? 1 :
            0;
    }
}

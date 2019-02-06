using System;
using System.Linq;

static class DifferenceOfSquares
{
    public static int CalculateSquareOfSum(int n) => (int)Math.Pow(Enumerable.Range(1, n).Sum(), 2);
    
	public static int CalculateSumOfSquares(int n) => Enumerable.Range(1,n).Select(x => x * x).Sum();

    public static int CalculateDifferenceOfSquares(int n) => CalculateSquareOfSum(n) - CalculateSumOfSquares(n);
}

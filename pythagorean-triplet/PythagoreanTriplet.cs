using System;
using System.Collections.Generic;
using System.Linq;

public class PythagoreanTriplet
{
    public static IEnumerable<(int a, int b, int c)> TripletsWithSum(int sum) =>
        from a in Enumerable.Range(3, sum / 2 - 3)
        let a2 = a * a
        from b in Enumerable.Range(a, sum / 2 - a)
        let b2 = b * b
        let c = sum - a - b
        where a2 + b2 == c * c
        select (a, b, c);
}

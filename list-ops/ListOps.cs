using System;
using System.Collections.Generic;

public class ListOps
{
    public static int Length<T>(List<T> list) => list.Count;

    public static IEnumerable<T> Reverse<T>(IEnumerable<T> list) => Reverse(new List<T>(list));

    public static IEnumerable<T> Reverse<T>(List<T> list)
    {
        for (int i = Length(list) - 1; i >= 0; i--) yield return list[i];
    }

    public static IEnumerable<TOut> Map<TIn, TOut>(IEnumerable<TIn> list, Func<TIn, TOut> f)
    {
        foreach (var x in list) yield return f(x);
    }

    public static IEnumerable<T> Append<T>(IEnumerable<T> list1, IEnumerable<T> list2) =>
        Concat(new[] { list1, list2 });

    public static IEnumerable<T> Filter<T>(IEnumerable<T> list, Func<T, bool> f)
    {
        foreach (var x in list) if (f(x)) yield return x;
    }

    public static TOut Foldl<TIn, TOut>(IEnumerable<TIn> list, TOut _default, Func<TOut, TIn, TOut> f)
    {
        foreach (var x in list) _default = f(_default, x);
        return _default;
    }

    public static TOut Foldr<TIn, TOut>(IEnumerable<TIn> list, TOut _default, Func<TIn, TOut, TOut> f) =>
         Foldl(Reverse(list), _default, (x, y) => f(y, x));

    public static IEnumerable<T> Concat<T>(IEnumerable<IEnumerable<T>> lists)
    {
        foreach (var list in lists) foreach (var item in list) yield return item;
    }
}

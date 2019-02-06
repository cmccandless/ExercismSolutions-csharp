using System;
using System.Collections.Generic;
using System.Linq;

public class FlattenArray
{
    public static int[] Flatten(int[] arr) => Flatten(arr.Cast<object>().ToArray());
    public static int[] Flatten(object[] arr)
    {
        var sin = new Stack<object>(arr);
        var sout = new Stack<int>();
        while (sin.Any())
        {
            switch(sin.Pop())
            {
                case object[] xs:
                    foreach (var y in xs) sin.Push(y);
                    break;
                case int i:
                    sout.Push(i);
                    break;
            }
        }
        return sout.ToArray();
    }
}

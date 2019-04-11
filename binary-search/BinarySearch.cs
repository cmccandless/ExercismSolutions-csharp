using System;

public static class BinarySearch
{
    public static int Find(int[] array, int x)
    {
        if (array.Length == 0) return -1;

        var p = 0;
        var r = array.Length;
        while (p < r)
        {
            var q = (r - p) / 2 + p;
            if (array[q] == x) return q;

            if (array[q] < x)
            {
                if (p == q) return -1;
                p = q;
            }
            else r = q;
        }
        return -1;
    }
}

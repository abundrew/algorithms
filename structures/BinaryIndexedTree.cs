using System;

namespace algorithms.structures
{
    public static class BinaryIndexedTree
    {
        // ----- Binary Indexed Tree (Fenwick Tree) ----------------------------
        //
        // A data structure that can efficiently update elements and calculate
        // prefix sums in a table of numbers
        //
        // O(nlogn) create
        // O(logn) update / get prefix sum
        //
        // int[] CreateFenwick(int[] a)
        // void UpdateFenwick(int[] ft, int v, int ix)
        // int SumFenwick(int[] ft, int ix)
        // ---------------------------------------------------------------------
        // int[,] CreateFenwick(int[,] a)
        // void UpdateFenwick(int[,] ft, int v, int ix, int jx)
        // int SumFenwick(int[,] ft, int ix, int jx)
        // ---------------------------------------------------------------------
        // int[] CreateFenwick(int[] a, Func<int, int, int> func)
        // void UpdateFenwick(int[] ft, int v, int ix, Func<int, int, int> func)
        // int SumFenwick(int[] ft, int ix, Func<int, int, int> func)
        // ---------------------------------------------------------------------
        public static int[] CreateFenwick(int[] a)
        {
            int[] ft = new int[a.Length + 1];
            for (int i = 1; i <= a.Length; i++) UpdateFenwick(ft, a[i - 1], i);
            return ft;
        }
        public static void UpdateFenwick(int[] ft, int v, int ix)
        {
            while (ix < ft.Length)
            {
                ft[ix] += v;
                ix = ix + (ix & -ix);
            }
        }
        public static int SumFenwick(int[] ft, int ix)
        {
            ix++;
            int sum = 0;
            while (ix > 0)
            {
                sum += ft[ix];
                ix = ix - (ix & -ix);
            }
            return sum;
        }
        // ---------------------------------------------------------------------
        public static int[,] CreateFenwick(int[,] a)
        {
            int[,] ft = new int[a.GetLength(0) + 1, a.GetLength(1) + 1];
            for (int i = 1; i <= a.GetLength(0); i++)
                for (int j = 1; j <= a.GetLength(1); j++)
                    UpdateFenwick(ft, a[i - 1, j - 1], i, j);
            return ft;
        }
        public static void UpdateFenwick(int[,] ft, int v, int ix, int jx)
        {
            while (ix < ft.GetLength(0))
            {
                while (jx < ft.GetLength(1))
                {
                    ft[ix, jx] += v;
                    jx = jx + (jx & -jx);
                }
                ix = ix + (ix & -ix);
            }
        }
        public static int SumFenwick(int[,] ft, int ix, int jx)
        {
            ix++;
            jx++;
            int sum = 0;
            while (ix > 0)
            {
                while (jx > 0)
                {
                    sum += ft[ix, jx];
                    jx = jx - (jx & -jx);
                }
                ix = ix - (ix & -ix);
            }
            return sum;
        }
        // ---------------------------------------------------------------------
        public static int[] CreateFenwick(int[] a, Func<int, int, int> func)
        {
            int[] ft = new int[a.Length + 1];
            for (int i = 1; i <= a.Length; i++) UpdateFenwick(ft, a[i - 1], i, func);
            return ft;
        }
        public static void UpdateFenwick(int[] ft, int v, int ix, Func<int, int, int> func)
        {
            while (ix < ft.Length)
            {
                ft[ix] = func(ft[ix], v);
                ix = ix + (ix & -ix);
            }
        }
        public static int SumFenwick(int[] ft, int ix, Func<int, int, int> func)
        {
            ix++;
            int sum = 0;
            while (ix > 0)
            {
                sum = func(sum, ft[ix]);
                ix = ix - (ix & -ix);
            }
            return sum;
        }
        // ---------------------------------------------------------------------
    }
}

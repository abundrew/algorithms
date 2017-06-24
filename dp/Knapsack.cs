using System;

namespace algorithms.dp
{
    public static class Knapsack
    {
        // ----- Knapsack ------------------------------------------------------
        // -- Given weights and values of n items, put these items in a knapsack
        // -- of capacity W to get the maximum total value in the knapsack.
        //
        // -- v: values, w: weights, W: max weight capacity
        //
        // -- O(nW)
        //
        // -- knapsack 0-1
        // -- restricts the number of copies of each kind of item to zero or one
        //
        // int ZeroOne(int[] v, int[] w, int W)
        //
        // -- unbounded (UKP)
        // -- no upper bound on the number of copies of each kind of item
        //
        // int Unbounded(int[] v, int[] w, int W)
        // int Unbounded2(int[] v, int[] w, int W)
        // ---------------------------------------------------------------------
        public static int ZeroOne(int[] v, int[] w, int W)
        {
            int n = v.Length;
            int[] m = new int[W + 1];

            for (int i = 0; i < n; i++)
                for (int j = W; j >= 0; j--)
                    m[j] = j < w[i] ? m[j] : Math.Max(m[j], m[j - w[i]] + v[i]);
            return m[W];
        }
        public static int Unbounded(int[] v, int[] w, int W)
        {
            int n = v.Length;
            int[] m = new int[W + 1];

            for (int i = 0; i < n; i++)
                for (int j = w[i]; j <= W; j++)
                    m[j] = Math.Max(m[j], m[j - w[i]] + v[i]);
            return m[W];
        }
        public static int Unbounded2(int[] v, int[] w, int W)
        {
            int n = v.Length;
            int[] m = new int[W + 1];
            for (int c = 1; c <= W; c++)
            {
                m[c] = m[c - 1];
                for (int i = 0; i < n; i++)
                    if (w[i] <= c)
                        m[c] = Math.Max(m[c], m[c - w[i]] + v[i]);
            }
            return m[W];
        }
        // ---------------------------------------------------------------------
    }
}

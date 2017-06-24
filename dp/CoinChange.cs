using System;

namespace algorithms.dp
{
    public static class CoinChange
    {
        // ----- Coin Change ---------------------------------------------------
        //
        // -- min number of coins of denomination d[] to add up S
        // -- d[0] = 1
        //
        // int Make(int[] d, int S)
        //
        // -- number of ways to add up S with coins of denomination d[]
        // -- d[0] = 1
        //
        // int Count(int[] d, int S)
        // ---------------------------------------------------------------------
        public static int Make(int[] d, int S)
        {
            int n = d.Length;
            int[] m = new int[S + 1];
            for (int s = 1; s <= S; s++)
            {
                int t = int.MaxValue;
                int j = 0;
                while (j < n && d[j] <= s)
                {
                    t = Math.Min(m[s - d[j]], t);
                    j++;
                }
                m[s] = t + 1;
            }
            return m[S];
        }
        public static int Count(int[] d, int S)
        {
            int n = d.Length;
            int[] m = new int[S + 1];
            m[0] = 1;
            for (int i = 0; i < n; i++)
                for (int j = d[i]; j <= S; j++)
                    m[j] += m[j - d[i]];
            return m[S];
        }
        // ---------------------------------------------------------------------
    }
}

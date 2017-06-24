using System;

namespace algorithms.strings
{
    public static class Searching
    {
        // ----- Searching -----------------------------------------------------
        //
        // Search with Suffix Array
        // http://www.inf.fu-berlin.de/lehre/WS05/aldabi/downloads/stringMatching_part2.pdf
        //
        // O(mlogn)
        //
        // Dependencies:
        // -- Suffix Array
        //
        // int SearchSA(char[] S, int[] suffixArray, char[] pattern, out int L, out int R)
        // ---------------------------------------------------------------------
        static int CompareSA(char[] pattern, char[] S, int ix)
        {
            for (int i = 0; i < Math.Min(S.Length - ix, pattern.Length); i++)
                if (pattern[i] != S[ix + i])
                    return pattern[i].CompareTo(S[ix + i]);
            if (pattern.Length > S.Length) return 1;
            return 0;
        }
        public static int SearchSA(char[] S, int[] suffixArray, char[] pattern, out int Lp, out int Rp)
        {
            int N = S.Length;
            if (CompareSA(pattern, S, suffixArray[0]) <= 0) Lp = 0;
            else
            if (CompareSA(pattern, S, suffixArray[N - 1]) > 0) Lp = N;
            else
            {
                int L = 0;
                int R = N - 1;
                while (R - L > 1)
                {
                    int M = (L + R + 1) / 2;
                    if (CompareSA(pattern, S, suffixArray[M]) <= 0) R = M; else L = M;
                }
                Lp = R;
            }
            if (CompareSA(pattern, S, suffixArray[0]) < 0) Rp = -1;
            else
            if (CompareSA(pattern, S, suffixArray[N - 1]) >= 0) Rp = N - 1;
            else
            {
                int L = 0;
                int R = N - 1;
                while (R - L > 1)
                {
                    int M = (L + R + 1) / 2;
                    if (CompareSA(pattern, S, suffixArray[M]) >= 0) L = M; else R = M;
                }
                Rp = L;
            }
            return Rp - Lp + 1;
        }
        // ---------------------------------------------------------------------
    }
}

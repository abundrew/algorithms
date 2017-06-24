using System.Linq;

namespace algorithms.strings
{
    public static class LongestCommonPrefixArray
    {
        // ----- Longest Common Prefix Array -----------------------------------
        // An auxiliary data structure to the suffix array
        //
        // Stores the lengths of the longest common prefixes (LCPs) between
        //   all pairs of consecutive suffixes in a sorted suffix array
        //
        // O(n) worst-case time and space
        //
        // Dependencies:
        // -- Suffix Array
        //
        // int[] LCP(char[] S, int[] suffixArray)
        // ---------------------------------------------------------------------
        public static int[] LCP(char[] S, int[] suffixArray)
        {
            int n = S.Length;
            int h = 0;
            int[] lcp = new int[n];
            int[] isa = new int[n];
            for (int i = 0; i < n; i++) isa[suffixArray[i]] = i;
            for (int i = 0; i < n; i++)
            {
                if (isa[i] > 0)
                {
                    for (int j = suffixArray[isa[i] - 1]; j + h < n && i + h < n && S[j + h] == S[i + h]; h++) ;
                    lcp[isa[i]] = h;
                }
                else
                {
                    lcp[isa[i]] = 0;
                }
                if (h > 0) h--;
            }
            return lcp;
        }
        public static int[] LCP(string S, int[] suffixArray)
        {
            return LCP(S.ToArray(), suffixArray);
        }
        // ---------------------------------------------------------------------
    }
}

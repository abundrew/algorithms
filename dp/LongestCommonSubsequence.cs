using System;

namespace algorithms.dp
{
    public static class LongestCommonSubsequence<T> where T : IEquatable<T>
    {
        // ----- Longest Common Subsequence ------------------------------------
        //
        // -- O(mn)
        //
        // T[] LCS(T[] A, T[] B)
        // ---------------------------------------------------------------------
        public static T[] LCS(T[] X, T[] Y)
        {
            int m = X.Length;
            int n = Y.Length;
            int[][] L = new int[m + 1][];
            for (int i = 0; i < m + 1; i++)
                L[i] = new int[n + 1];
            // Following steps build L[m+1][n+1] in bottom up fashion. Note
            //   that L[i][j] contains length of LCS of X[0..i-1] and Y[0..j-1]
            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (i == 0 || j == 0)
                        L[i][j] = 0;
                    else if (X[i - 1].Equals(Y[j - 1]))
                        L[i][j] = L[i - 1][j - 1] + 1;
                    else
                        L[i][j] = Math.Max(L[i - 1][j], L[i][j - 1]);
                }
            }
            // Following code is used to print LCS
            int index = L[m][n];
            // Create a character array to store the lcs string
            T[] lcs = new T[index];
            // Start from the right-most-bottom-most corner and
            // one by one store characters in lcs[]
            int ix = m, jx = n;
            while (ix > 0 && jx > 0)
            {
                // If current character in X[] and Y are same, then
                // current character is part of LCS
                if (X[ix - 1].Equals(Y[jx - 1]))
                {
                    lcs[index - 1] = X[ix - 1]; // Put current character in result
                    ix--; jx--; index--;        // reduce values of i, j and index
                }
                // If not same, then find the larger of two and
                // go in the direction of larger value
                else if (L[ix - 1][jx] > L[ix][jx - 1])
                    ix--;
                else
                    jx--;
            }
            return lcs;
        }
        // ---------------------------------------------------------------------
    }
}

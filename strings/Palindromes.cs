using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.strings
{
    public static class Palindromes
    {
        // ----- Palindromes ---------------------------------------------------
        //
        // IEnumerable<string> Palindromes_01(string s)
        // int[] Palindromes_02(string s)
        // IEnumerable<string> Palindromes_02(string s, int[] r)
        // int number_of_palindromic_subsequences(string s)
        // ---------------------------------------------------------------------
        public static IEnumerable<string> Palindromes_01(string s)
        {
            int radius = 0;
            int N = s.Length;
            int[,] pTable = new int[2, N + 1];
            s = "@" + s + "#";
            for (int j = 0; j <= 1; j++)
            {
                radius = 0;
                pTable[j, 0] = 0;
                int i = 1;
                while (i <= N)
                {
                    while (s[i - radius - 1] == s[i + j + radius]) radius++;
                    pTable[j, i] = radius;
                    int k = 1;
                    while ((pTable[j, i - k] != radius - k) && (k < radius))
                    {
                        pTable[j, i + k] = Math.Min(pTable[j, i - k], radius - k);
                        k++;
                    }
                    radius = Math.Max(radius - k, 0);
                    i += k;
                }
            }
            s = s.Substring(1, N);
            for (int i = 1; i <= N; i++)
                for (int j = 0; j <= 1; j++)
                    for (int rp = pTable[j, i]; rp > 0; rp--)
                        yield return s.Substring(i - rp - 1, 2 * rp + j);
        }
        public static int[] Palindromes_02(string s)
        {
            int n = s.Length;
            int[] r = new int[2 * n];
            int k = 0;
            for (int i = 0, j = 0; i < 2 * n; i += k, j = Math.Max(j - k, 0))
            {
                while (i - j >= 0 && i + j + 1 < 2 * n && s[(i - j) / 2] == s[(i + j + 1) / 2]) j++;
                r[i] = j;
                for (k = 1; i - k >= 0 && r[i] - k >= 0 && r[i - k] != r[i] - k; k++)
                {
                    r[i + k] = Math.Min(r[i - k], r[i] - k);
                }
            }
            return r;
        }
        public static IEnumerable<string> Palindromes_02(string s, int[] r)
        {
            for (int i = 0; i < r.Length / 2; i++)
            {
                yield return s.Substring(i - r[i * 2] / 2, r[i * 2]);
                if (r[i * 2 + 1] > 0)
                    yield return s.Substring(i + 1 - r[i * 2 + 1] / 2, r[i * 2 + 1]);
            }
        }
        // ---------------------------------------------------------------------
        static int[,] nops_gg = null;
        static string nops_S = null;
        static int nops_R = 1000 * 1000 * 1000 + 7;
        static int nops_G(int ix, int len)
        {
            if (ix >= nops_S.Length) return 0;
            if (len <= 0) return 0;
            if (len == 1) return 1;
            if (nops_gg[ix, len] != -1) return nops_gg[ix, len];

            int g = (nops_G(ix + 1, len - 1) + nops_G(ix, len - 1) - nops_G(ix + 1, len - 2) + (nops_S[ix] == nops_S[ix + len - 1] ? 1 : 0) * (1 + nops_G(ix + 1, len - 2))) % nops_R;
            nops_gg[ix, len] = g;
            return g;
        }
        public static int number_of_palindromic_subsequences(string s)
        {
            int n = s.Length;
            nops_gg = new int[n, n + 1];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n + 1; j++)
                    nops_gg[i, j] = -1;
            nops_S = s;
            return nops_G(0, n);
        }
        // ---------------------------------------------------------------------
    }
}

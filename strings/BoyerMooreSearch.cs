using System;
using System.Collections.Generic;

namespace algorithms.strings
{
    // ----- Boyer Moore Search ------------------------------------------------
    //
    // An efficient string searching algorithm
    //
    // O(nm) in the worst case
    //
    // BoyerMooreSearch()
    // BoyerMooreSearch(int alphabetSize, Func<char, byte> alphabetFunc)
    // int Search(string text, string pattern)
    // IEnumerable<int> SearchAll(string text, string pattern)
    // -------------------------------------------------------------------------
    public class BoyerMooreSearch
    {
        private int alphabetSize = 0;
        Func<char, byte> alphabetFunc = null;
        private int[] occurenceFunc = null;

        public BoyerMooreSearch() : this(26, p => (byte)(Convert.ToByte(p) - 97)) { }

        public BoyerMooreSearch(int alphabetSize, Func<char, byte> alphabetFunc)
        {
            this.alphabetSize = alphabetSize;
            this.alphabetFunc = alphabetFunc;
            occurenceFunc = new int[alphabetSize];
        }

        public int Search(string text, string pattern)
        {
            int tLen = text.Length;
            int pLen = pattern.Length;

            for (int i = 0; i < alphabetSize; i++) occurenceFunc[i] = -1;
            for (int i = 0; i < pLen; i++) occurenceFunc[alphabetFunc(pattern[i])] = i;

            int[] f = new int[pLen + 1];
            int[] s = new int[pLen + 1];

            int i1 = pLen, i2 = pLen + 1;
            f[i1] = i2;
            while (i1 > 0)
            {
                while (i2 <= pLen && pattern[i1 - 1] != pattern[i2 - 1])
                {
                    if (s[i2] == 0)
                    {
                        s[i2] = i2 - i1;
                    }
                    i2 = f[i2];
                }
                i1--; i2--;
                f[i1] = i2;
            }

            i2 = f[0];
            for (i1 = 0; i1 <= pLen; i1++)
            {
                if (s[i1] == 0) s[i1] = i2;
                if (i1 == i2) i2 = f[i2];
            }


            i1 = 0;
            while (i1 <= tLen - pLen)
            {
                i2 = pLen - 1;
                while (i2 >= 0 && pattern[i2] == text[i1 + i2]) i2--;
                if (i2 < 0)
                {
                    return i1;
                }
                else
                    i1 += Math.Max(s[i2 + 1], i2 - occurenceFunc[alphabetFunc(text[i1 + i2])]);
            }
            return -1;
        }

        public IEnumerable<int> SearchAll(string text, string pattern)
        {
            int tLen = text.Length;
            int pLen = pattern.Length;

            for (int i = 0; i < alphabetSize; i++) occurenceFunc[i] = -1;
            for (int i = 0; i < pLen; i++) occurenceFunc[alphabetFunc(pattern[i])] = i;

            int[] f = new int[pLen + 1];
            int[] s = new int[pLen + 1];

            int i1 = pLen, i2 = pLen + 1;
            f[i1] = i2;
            while (i1 > 0)
            {
                while (i2 <= pLen && pattern[i1 - 1] != pattern[i2 - 1])
                {
                    if (s[i2] == 0)
                    {
                        s[i2] = i2 - i1;
                    }
                    i2 = f[i2];
                }
                i1--; i2--;
                f[i1] = i2;
            }

            i2 = f[0];
            for (i1 = 0; i1 <= pLen; i1++)
            {
                if (s[i1] == 0) s[i1] = i2;
                if (i1 == i2) i2 = f[i2];
            }


            i1 = 0;
            while (i1 <= tLen - pLen)
            {
                i2 = pLen - 1;
                while (i2 >= 0 && pattern[i2] == text[i1 + i2]) i2--;
                if (i2 < 0)
                {
                    yield return i1;
                    i1 += s[0];
                }
                else
                    i1 += Math.Max(s[i2 + 1], i2 - occurenceFunc[alphabetFunc(text[i1 + i2])]);
            }
        }
    }
}

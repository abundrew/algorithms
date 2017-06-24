using System;
using System.Collections.Generic;

namespace algorithms.strings
{
    public static class SuffixAutomatonHacks
    {
        // ----- Suffix Automato Hacks -----------------------------------------
        //
        // Dependencies:
        // -- Suffix Automaton
        //
        // bool find_substring(SuffixAutomaton sa, char[] a)
        // void longest_common_substring(SuffixAutomaton sa, char[] a, out int pos, out int len)
        // long number_of_different_substrings(char[] s)
        // long total_length_of_different_substrings(char[] s)
        // char[] least_cyclic_shift(char[] s)
        // ---------------------------------------------------------------------
        // bool test_find_substring(char[] s, char[] a)
        // void test_longest_common_substring(char[] s, char[] a, out int pos, out int len)
        // long test_number_of_different_substrings(char[] s)
        // long test_total_length_of_different_substrings(char[] s)
        // char[] test_least_cyclic_shift(char[] s)
        // ---------------------------------------------------------------------
        public static bool find_substring(SuffixAutomaton sa, char[] a)
        {
            SuffixAutomaton.Node v = sa.t0;
            foreach (char c in a)
            {
                if (!v.ContainsKeyNext(c)) return false;
                v = v.GetNext(c);
            }
            return true;
        }
        public static void longest_common_substring(SuffixAutomaton sa, char[] a, out int pos, out int len)
        {
            pos = 0;
            len = 0;
            SuffixAutomaton.Node v = sa.t0;
            int l = 0;
            for (int u = 0; u < a.Length; u++)
            {
                while (v != sa.t0 && !v.ContainsKeyNext(a[u]))
                {
                    v = v.link;
                    l = v.len;
                }
                if (v.ContainsKeyNext(a[u]))
                {
                    v = v.GetNext(a[u]);
                    l++;
                }
                if (l > len)
                {
                    len = l; pos = u;
                }
            }
        }
        public static long number_of_different_substrings(char[] s)
        {
            SuffixAutomaton sa = SuffixAutomaton.Build(s);
            if (!sa.sortedTopologically) sa.SortTopologically();
            long[] D = new long[sa.gen];
            for (int i = sa.gen - 1; i >= 0; i--)
            {
                D[i] = 1;
                for (int j = 0; j < sa.nodes[i].np; j++)
                    D[i] += D[sa.nodes[i].next[j].id];
            }
            return D[0] - 1;
        }
        public static long total_length_of_different_substrings(char[] s)
        {
            SuffixAutomaton sa = SuffixAutomaton.Build(s);
            if (!sa.sortedTopologically) sa.SortTopologically();
            long[] D = new long[sa.gen];
            long[] W = new long[sa.gen];
            for (int i = sa.gen - 1; i >= 0; i--)
            {
                D[i] = 1;
                for (int j = 0; j < sa.nodes[i].np; j++)
                {
                    D[i] += D[sa.nodes[i].next[j].id];
                    W[i] += D[sa.nodes[i].next[j].id] + W[sa.nodes[i].next[j].id];
                }
            }
            return W[0];
        }
        public static char[] least_cyclic_shift(char[] s)
        {
            char[] ss = new char[s.Length * 2];
            Buffer.BlockCopy(s, 0, ss, 0, sizeof(char) * s.Length);
            Buffer.BlockCopy(s, 0, ss, sizeof(char) * s.Length, sizeof(char) * s.Length);
            SuffixAutomaton sa = SuffixAutomaton.Build(ss);
            if (!sa.lexsorted) sa.LexSort();
            SuffixAutomaton.Node v = sa.t0;
            char[] a = new char[s.Length];
            int ix = 0;
            while (ix < s.Length)
            {
                v = v.next[0];
                a[ix++] = v.key;
            }
            return a;
        }
        // ---------------------------------------------------------------------
        public static bool test_find_substring(char[] s, char[] a)
        {
            return new string(s).IndexOf(new string(a)) > -1;
        }
        public static void test_longest_common_substring(char[] s, char[] a, out int pos, out int len)
        {
            pos = 0;
            len = 0;
            for (int l = Math.Min(s.Length, a.Length); l > 0; l--)
                for (int u = 0; u < a.Length - l; u++)
                {
                    int p = new string(s).IndexOf(new string(a).Substring(u, l));
                    if (p >= 0)
                    {
                        pos = p;
                        len = l;
                        return;
                    }
                }
        }
        public static long test_number_of_different_substrings(char[] s)
        {
            HashSet<string> hs = new HashSet<string>();
            for (int i = 0; i < s.Length; i++)
                for (int len = 1; len <= s.Length - i; len++)
                    hs.Add(new string(s, i, len));
            return hs.Count;
        }
        public static long test_total_length_of_different_substrings(char[] s)
        {
            HashSet<string> hs = new HashSet<string>();
            long total = 0;
            for (int i = 0; i < s.Length; i++)
                for (int len = 1; len <= s.Length - i; len++) {
                    string ss = new string(s, i, len);
                    if (!hs.Contains(ss)) {
                        hs.Add(ss);
                        total += ss.Length;
                    }
                }
            return total;
        }
        public static char[] test_least_cyclic_shift(char[] s)
        {
            string a = new string(s);
            for (int i = 1; i < s.Length; i++)
            {
                string b = a.Substring(i) + a.Substring(0, i);
                if (string.Compare(a, b) == 1) a = b;
            }
            return a.ToCharArray();
        }
        // ---------------------------------------------------------------------
    }
}

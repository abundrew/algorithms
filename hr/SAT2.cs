using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using algorithms.graphs;

namespace algorithms.hr
{
    // ----- 2-SAT (SAT-isfiability) -------------------------------------------
    //
    // https://en.wikipedia.org/wiki/2-satisfiability
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    // -- TarjanSCC (algorithms.graphs)
    //
    // N - number of variables [0..N-1].
    //
    // int NOT(int a)
    // void MUST(int a)
    // void OR(int a, int b)
    // void XOR(int a, int b)
    // void AND(int a, int b)
    // void NOT_AND(int a, int b)
    //
    // bool Possible()
    // -------------------------------------------------------------------------
    public class SAT2
    {
        int N = 0;
        List<int[]> E = null;
        public SAT2(int n)
        {
            N = n;
            E = new List<int[]>();
        }
        public int c_not(int a)
        {
            return -a - 1;
        }
        int c_convert(int a)
        {
            return a < 0 ? (c_not(a) << 1) ^ 1 : a << 1;
        }
        void c_must(int a)
        {
            E.Add(new int[] { a ^ 1, a, 1 });
        }
        void c_or(int a, int b)
        {
            E.Add(new int[] { a ^ 1, b, 1 });
            E.Add(new int[] { b ^ 1, a, 1 });
        }
        public void c_xor(int a, int b)
        {
            c_or(a, b);
            c_or(a ^ 1, b ^ 1);
        }
        void c_and(int a, int b)
        {
            E.Add(new int[] { a, b, 1 });
            E.Add(new int[] { b, a, 1 });
        }
        void c_not_and(int a, int b)
        {
            E.Add(new int[] { a, b ^ 1, 1 });
            E.Add(new int[] { b, a ^ 1, 1 });
        }

        public int NOT(int a) { return c_not(a); }
        public void MUST(int a) { c_must(c_convert(a)); }
        public void OR(int a, int b) { c_or(c_convert(a), c_convert(b)); }
        public void XOR(int a, int b) { c_xor(c_convert(a), c_convert(b)); }
        public void AND(int a, int b) { c_and(c_convert(a), c_convert(b)); }
        public void NOT_AND(int a, int b) { c_not_and(c_convert(a), c_convert(b)); }

        public bool Possible()
        {
            Graph g = new Graph(N * 2, E, true);
            TarjanSCC scc = new TarjanSCC(g);
            for (int v = 0; v < N; v++)
                if (scc.StronglyConnected(v << 1, (v << 1) ^ 1))
                    return false;
            return true;
        }
    }
    // -------------------------------------------------------------------------
}

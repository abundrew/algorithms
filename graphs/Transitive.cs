using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using algorithms.math;

namespace algorithms.graphs
{
    public static class Transitive
    {
        // ----- Transitive ----------------------------------------------------
        //
        // Depends on:
        //   Bits (algorithms.math)
        //
        // Graph TransitiveReduction(Graph g, out int[] v2v)
        // ---------------------------------------------------------------------
        public static Graph TransitiveReduction(Graph g, out int[] v2v)
        {
            TarjanSCC scc = new TarjanSCC(g);

            int[] id2 = new int[g.V];
            for (int v = 0; v < g.V; v++)
                id2[v] = scc.ID(v);
            Array.Sort(id2);
            int[] id2v = new int[g.V];
            int nd2v = 0;
            id2v[id2[0]] = nd2v++;
            for (int i = 1; i < g.V; i++)
                if (id2[i] != id2[i - 1])
                    id2v[id2[i]] = nd2v++;

            v2v = new int[g.V];
            for (int v = 0; v < g.V; v++)
                v2v[v] = id2v[scc.ID(v)];

            HashSet<long> hse = new HashSet<long>();
            for (int v = 0; v < g.V; v++)
            {
                long vkey = (long)v2v[v] * g.V;
                for (int i = 0; i < g.Deg(v); i++)
                    hse.Add(vkey + v2v[g.AdjV(v, i)]);
            }

            List<int[]> e = new List<int[]>();
            foreach (long vkey in hse)
                e.Add(new int[] { (int)(vkey / g.V), (int)(vkey % g.V) });

            return new Graph(nd2v, e, true);
        }

        // topological sort
        // backwards Adj -> Bdj

        public static int[][] TransitiveClosure(Graph g)
        {
            int[][] closure = new int[g.V][];
            for (int v = 0; v < g.V; v++)
                closure[v] = Bits.BitsArray(g.V);
            for (int v = 0; v < g.V; v++)
            {
                Bits.MarkBit(closure[v], v);
                for (int i = 0; i < g.Deg(v); i++)
                {
                    int v1 = g.BdjV(v, i);
                    for (int u = 0; u < g.V; u++)
                        if (Bits.IsMarked(closure[v], u)) Bits.MarkBit(closure[v1], u);
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using algorithms.structures;

namespace algorithms.graphs
{
    // ----- Kruskal Minimum Spanning Tree (Forest) ----------------------------
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/KruskalMST.java.html
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    // -- BinaryHeapPQ (algorithms.structures)
    // -- DisjointSets (algorithms.structures)
    //
    // KruskalMST(Graph g)
    // IEnumerable<int[]> MST()
    // -------------------------------------------------------------------------
    public class KruskalMST
    {
        public class Edge: IComparable
        {
            public int U { get; set; }
            public int V { get; set; }
            public int W { get; set; }
            public int CompareTo(object obj)
            {
                return W.CompareTo((obj as Edge).W);
            }
        }
        List<Edge> mst = new List<Edge>();
        public KruskalMST(Graph g)
        {
            BinaryHeapPQ<Edge> pq = new BinaryHeapPQ<Edge>();
            for (int v = 0; v < g.V; v++)
                for (int i = 0; i < g.Deg(v); i++)
                    pq.Insert(new Edge() { U = v, V = g.AdjV(v, i), W = g.AdjW(v, i) });
            DisjointSets ds = new DisjointSets(g.V);
            while (pq.Count > 0 && mst.Count < g.V - 1)
            {
                Edge e = pq.ExtractMin();
                int v = e.U;
                int w = e.V;
                if (ds.FindSet(v) != ds.FindSet(w))
                {
                    ds.Union(v, w);
                    mst.Add(e);
                }
            }
        }
        public IEnumerable<Edge> MST()
        {
            return mst;
        }
    }
    // -------------------------------------------------------------------------
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using algorithms.structures;

namespace algorithms.graphs
{
    // ----- Dijkstra Shortest Path --------------------------------------------
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DijkstraSP.java.html
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    // -- IndexBinaryHeapPQ (algorithms.structures)
    //
    // DijkstraSP(Graph g, int s)
    // int Dist(int v)
    // bool HasPath(int v)
    // IEnumerable<int> Path(int v)
    // -------------------------------------------------------------------------
    public class DijkstraSP
    {
        int[] dist = null;
        int[] prev = null;
        IndexBinaryHeapPQ<int> pq = null;
        public DijkstraSP(Graph g, int s)
        {
            dist = new int[g.V];
            prev = new int[g.V];
            for (int i = 0; i < g.V; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = -1;
            }
            dist[s] = 0;
            pq = new IndexBinaryHeapPQ<int>(g.V);
            pq.Insert(s, dist[s]);
            while (pq.Count > 0)
            {
                int v = pq.ExtractMin();
                for (int i = 0; i < g.Deg(v); i++) Relax(v, g.AdjV(v, i), g.AdjW(v, i));
            }
        }
        public int Dist(int v)
        {
            return dist[v];
        }
        public bool HasPath(int v)
        {
            return dist[v] < int.MaxValue;
        }
        public IEnumerable<int> Path(int v)
        {
            if (!HasPath(v)) return null;
            Stack<int> stack = new Stack<int>();
            while (v != -1)
            {
                stack.Push(v);
                v = prev[v];
            }
            return stack;
        }
        void Relax(int u, int v, int w)
        {
            if (dist[v] > dist[u] + w)
            {
                dist[v] = dist[u] + w;
                prev[v] = u;
                if (pq.Contains(v)) pq.Decrease(v, dist[v]);
                else pq.Insert(v, dist[v]);
            }
        }
        public static int[,] SP(Graph g)
        {
            int[,] sp = new int[g.V, g.V];
            for (int v = 0; v < g.V; v++)
            {
                DijkstraSP dsp = new DijkstraSP(g, v);
                for (int u = 0; u < g.V; u++) sp[v, u] = dsp.Dist(u);
            }
            return sp;
        }
    }
    // -------------------------------------------------------------------------
}

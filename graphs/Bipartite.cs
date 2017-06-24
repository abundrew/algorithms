using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algorithms.graphs
{
    // ----- Bipartite --------------------------------------------
    //
    // The class represents a data type for determining whether
    // an UNDIRECTED graph is bipartite or whether it has
    // an odd-length cycle.
    //
    // O(V + E)
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Bipartite.java.html
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    //
    // DijkstraSP(Graph g, int s)
    // int Dist(int v)
    // bool HasPath(int v)
    // IEnumerable<int> Path(int v)
    // -------------------------------------------------------------------------
    public class Bipartite
    {
        public bool IsBipartite { get; private set; }
        bool[] color = null;
        bool[] marked = null;
        int[] edgeTo = null;
        Stack<int> cycle = null;
        public Bipartite(Graph g)
        {
            IsBipartite = true;
            color = new bool[g.V];
            marked = new bool[g.V];
            edgeTo = new int[g.V];
            for (int v = 0; v < g.V; v++)
                if (!marked[v])
                    Dfs(g, v);
        }
        void Dfs(Graph g, int v)
        {
            marked[v] = true;
            for (int i = 0; i < g.Deg(v); i++) {
                int w = g.AdjV(v, i);
                if (cycle != null) return;
                if (!marked[w])
                {
                    edgeTo[w] = v;
                    color[w] = !color[v];
                    Dfs(g, w);
                }
                else
                    if (color[w] == color[v])
                    {
                        IsBipartite = false;
                        cycle = new Stack<int>();
                        cycle.Push(w);  // don't need this unless you want to include start vertex twice
                        for (int x = v; x != w; x = edgeTo[x]) cycle.Push(x);
                        cycle.Push(w);
                    }
            }
        }
        public IEnumerable<int> OddCycle()
        {
            return cycle;
        }
    }
    // -------------------------------------------------------------------------
}

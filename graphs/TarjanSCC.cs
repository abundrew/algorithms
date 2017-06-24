using System;
using System.Collections.Generic;

namespace algorithms.graphs
{
    // ----- Tarjan Strongly Connected Components ------------------------------
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TarjanSCC.java.html
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    //
    // TarjanSCC(Graph g)
    // int Count
    // int ID(int v)
    // bool StronglyConnected(int u, int v)
    // static Graph TransitiveReduction(Graph g, out int[] v2v)
    // -------------------------------------------------------------------------
    public class TarjanSCC
    {
        public int Count { get; private set; }

        bool[] marked;
        int[] id;
        int[] low;
        int pre;
        Stack<int> stack;

        public TarjanSCC(Graph g)
        {
            marked = new bool[g.V];
            stack = new Stack<int>();
            id = new int[g.V];
            low = new int[g.V];
            for (int v = 0; v < g.V; v++)
            {
                if (!marked[v]) dfs(g, v);
            }
        }

        void dfs(Graph g, int v)
        {
            int w;
            marked[v] = true;
            low[v] = pre++;
            int min = low[v];
            stack.Push(v);
            for (int i = 0; i < g.Deg(v); i++)
            {
                w = g.AdjV(v, i);
                if (!marked[w]) dfs(g, w);
                if (low[w] < min) min = low[w];
            }
            if (min < low[v])
            {
                low[v] = min;
                return;
            }
            do
            {
                w = stack.Pop();
                id[w] = Count;
                low[w] = g.V;
            } while (w != v);
            Count++;
        }

        public int ID(int v) { return id[v]; }
        public bool StronglyConnected(int u, int v)
        {
            return id[u] == id[v];
        }

    }
    // -------------------------------------------------------------------------
}

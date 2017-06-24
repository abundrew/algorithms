using System.Collections.Generic;

namespace algorithms.graphs
{
    // ----- Connected Components ----------------------------------------------
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/CC.java.html
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    //
    // ConnectedComponents(Graph g)
    // bool Connected(int u, int v)
    // int Count
    // int ID(int v)
    // int Size(int id)
    // -------------------------------------------------------------------------
    public class ConnectedComponents
    {
        public int Count { get; private set; }
        int[] id = null;
        int[] size = null;
        public ConnectedComponents(Graph g)
        {
            bool[] marked = new bool[g.V];
            Count = 0;
            id = new int[g.V];
            size = new int[g.V];
            Stack<int> stack = new Stack<int>();
            for (int u = 0; u < g.V; u++)
                if (!marked[u])
                {
                    stack.Clear();
                    stack.Push(u);
                    while (stack.Count > 0)
                    {
                        int v = stack.Pop();
                        marked[v] = true;
                        id[v] = Count;
                        size[Count]++;
                        for (int i = 0; i < g.Deg(v); i++)
                        {
                            int v1 = g.AdjV(v, i);
                            if (!marked[v1]) stack.Push(v1);
                        }
                    }
                    Count++;
                }
        }
        public int ID(int v) { return id[v]; }
        public int Size(int id) { return size[id]; }
        public bool Connected(int u, int v)
        {
            return id[u] == id[v];
        }
    }
    // -------------------------------------------------------------------------
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algorithms.graphs
{
    // ----- Graph -------------------------------------------------------------
    //
    // Graph(int v, IEnumerable<int[]> edges, bool directed = false)
    // int AdjV(int v, int ix)
    // int AdjW(int v, int ix)
    // int Deg(int v)
    // int[][] IncidenceMatrix()
    // override string ToString()
    // -------------------------------------------------------------------------
    public class Graph
    {
        public int V { get; private set; }
        // [][ v1, v2 ... ]
        int[][] adj = null;
        // [][ w1, w2 ... ]
        int[][] wt = null;
        // edges: [][u, v, w]
        public Graph(int v, IEnumerable<int[]> edges, bool directed = false)
        {
            V = v;
            adj = new int[V][];
            wt = new int[V][];

            int[][] e2a = edges.ToArray();
            int[] deg = new int[V];
            foreach (int[] e in e2a)
            {
                deg[e[0]]++;
                if (!directed) deg[e[1]]++;
            }
            for (int i = 0; i < V; i++)
            {
                adj[i] = new int[deg[i]];
                wt[i] = new int[deg[i]];
            }
            Array.Clear(deg, 0, V);

            foreach (int[] e in e2a)
            {
                adj[e[0]][deg[e[0]]] = e[1];
                wt[e[0]][deg[e[0]]] = e[2];
                deg[e[0]]++;
                if (!directed)
                {
                    adj[e[1]][deg[e[1]]] = e[0];
                    wt[e[1]][deg[e[1]]] = e[2];
                    deg[e[1]]++;
                }
            }
        }
        public int AdjV(int v, int ix)
        {
            return adj[v][ix];
        }
        public int AdjW(int v, int ix)
        {
            return wt[v][ix];
        }
        public int Deg(int v)
        {
            return adj[v].Length;
        }
        public int[][] IncidenceMatrix()
        {
            int[][] mx = new int[V][];
            for (int v = 0; v < V; v++) mx[v] = new int[V];
            for (int v = 0; v < V; v++)
            {
                for (int i = 0; i < Deg(v); i++)
                {
                    mx[v][AdjV(v, i)] = AdjW(v, i);
                    mx[AdjV(v, i)][v] = -AdjW(v, i);
                }
            }
            return mx;
        }
        public int[] SortByPreorder(int root)
        {
            int[] stack = new int[V];
            int nstack = 0;
            int[] order = new int[V];
            int norder = 0;
            bool[] visited = new bool[V];
            stack[nstack++] = root;
            visited[root] = true;
            while (nstack > 0)
            {
                int v = stack[--nstack];
                order[norder++] = v;
                for (int i = 0; i < Deg(v); i++)
                {
                    int u = AdjV(v, i);
                    if (!visited[u])
                    {
                        visited[u] = true;
                        stack[nstack++] = u;
                    }
                }
            }
            return order;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int v = 0; v < V; v++)
                sb.AppendFormat("{0}:{1}", v, string.Join(",", Enumerable.Range(0, adj[v].Length).Select(p => string.Format("{0}[{1}]", adj[v][p], wt[v][p])).ToArray())).AppendLine();
            return sb.ToString();
        }
    }
    // -------------------------------------------------------------------------
}

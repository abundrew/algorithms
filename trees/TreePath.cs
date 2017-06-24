using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.trees
{
    // ----- Tree Path ---------------------------------------------------------
    //
    // O(N) on building, O(N) on path
    //
    // TreePath(int n, IEnumerable<int[]> edges)
    // int PathNodes(int u, int v, ref int[] nodes)
    // int Adj(int v, int ix)
    // int Deg(int v)
    // int Parent(int v)
    // int Depth(int v)
    // Stack<int> ToStack()
    // -------------------------------------------------------------------------
    public class TreePath
    {
        public int N { get; private set; }
        int[][] adj = null;
        int[] _parent = null;
        int[] _depth = null;
        public TreePath(int n, IEnumerable<int[]> edges)
        {
            N = n;
            adj = new int[N][];
            int[][] e2a = edges.ToArray();
            int[] deg = new int[N];
            foreach (int[] e in e2a)
            {
                deg[e[0]]++;
                deg[e[1]]++;
            }
            for (int i = 0; i < N; i++) adj[i] = new int[deg[i]];
            Array.Clear(deg, 0, N);
            foreach (int[] e in e2a)
            {
                adj[e[0]][deg[e[0]]] = e[1];
                deg[e[0]]++;
                adj[e[1]][deg[e[1]]] = e[0];
                deg[e[1]]++;
            }
            _parent = new int[N];
            _depth = new int[N];
            DFS(0);
        }
        void DFS(int root)
        {
            _parent[root] = root;
            _depth[root] = 0;
            for (int i = 0; i < adj[root].Length; i++)
            {
                int xt = adj[root][i];
                DFS(xt, root, 1);
            }
        }
        void DFS(int root, int parent, int depth)
        {
            _parent[root] = parent;
            _depth[root] = depth;
            while (adj[root].Length == 2)
            {
                int xt = adj[root][0];
                if (xt == parent) xt = adj[root][1];
                parent = root;
                depth++;
                root = xt;
                _parent[root] = parent;
                _depth[root] = depth;
            }
            for (int i = 0; i < adj[root].Length; i++)
            {
                int xt = adj[root][i];
                if (xt == parent) continue;
                DFS(xt, root, depth + 1);
            }
        }
        public int PathNodes(int u, int v, ref int[] nodes)
        {
            int k = 0;
            while (_depth[u] > _depth[v])
            {
                nodes[k++] = u;
                u = _parent[u];
            }
            while (_depth[u] < _depth[v])
            {
                nodes[k++] = v;
                v = _parent[v];
            }
            while (u != v)
            {
                nodes[k++] = u;
                nodes[k++] = v;
                u = _parent[u];
                v = _parent[v];
            }
            nodes[k++] = u;
            return k;
        }
        public int Adj(int v, int ix)
        {
            return adj[v][ix];
        }
        public int Deg(int v)
        {
            return adj[v].Length;
        }
        public int Parent(int v)
        {
            return _parent[v];
        }
        public int Depth(int v)
        {
            return _depth[v];
        }
        public int[] SortByPreorder()
        {
            int[] stack = new int[N];
            int nstack = 0;
            int[] order = new int[N];
            int norder = 0;
            stack[nstack++] = 0;
            while (nstack > 0)
            {
                int v = stack[--nstack];
                order[norder++] = v;
                for (int i = 0; i < Deg(v); i++)
                {
                    int u = adj[v][i];
                    if (u == _parent[v]) continue;
                    stack[nstack++] = u;
                }
            }
            return order;
        }
        public Stack<int> ToStack()
        {
            Stack<int> stack = new Stack<int>();
            Queue<int> que = new Queue<int>();
            que.Enqueue(0);
            while (que.Count > 0)
            {
                int v = que.Dequeue();
                stack.Push(v);
                for (int i = 0; i < Deg(v); i++)
                {
                    int u = adj[v][i];
                    if (u == _parent[v]) continue;
                    que.Enqueue(u);
                }
            }
            return stack;
        }
    }
    // -------------------------------------------------------------------------
}

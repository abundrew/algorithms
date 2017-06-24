using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms.trees
{
    // ----- Heavy Light Decomposition -----------------------------------------
    //
    // Heavy-Light Decomposition algorithm
    // for partitioning the edges of a tree into two groups - heavy and light.
    // Can be used for efficient traversal from any node to the root of the tree,
    // since there are at most log n light edges along that path;
    // hence, we can skip entire chains of heavy edges.
    //
    // O(N) on building, O(logN ^ 2) on Lowest Common Ancestor query.
    //
    // https://github.com/PetarV-/Algorithms/blob/master/Graph%20Algorithms/Heavy-Light%20Decomposition.cpp
    //
    // HeavyLightDecomposition(int n, IEnumerable<int[]> edges)
    // int LCA(int u, int v)
    // int Adj(int v, int ix)
    // int Deg(int v)
    // int Parent(int v)
    // int Depth(int v)
    // int SubTreeSize(int v)
    // -------------------------------------------------------------------------
    public class HeavyLightDecomposition
    {
        public int N { get; private set; }
        int[][] adj = null;
        int[] _parent = null;
        int[] _depth = null;
        int[] _chainTop = null;
        int[] _subTreeSize = null;
        public HeavyLightDecomposition(int n, IEnumerable<int[]> edges)
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
            _chainTop = new int[N];
            _subTreeSize = new int[N];
            DFS(0, 0, 0);
            HLD(0, 0, 0);
        }
        int DFS(int root, int parent, int depth)
        {
            _parent[root] = parent;
            _depth[root] = depth;
            _subTreeSize[root] = 1;
            for (int i = 0; i < adj[root].Length; i++)
            {
                int xt = adj[root][i];
                if (xt == parent) continue;
                _subTreeSize[root] += DFS(xt, root, depth + 1);
            }
            return _subTreeSize[root];
        }
        void HLD(int root, int parent, int chainTop)
        {
            _chainTop[root] = chainTop;
            for (int i = 0; i < adj[root].Length; i++)
            {
                int xt = adj[root][i];
                if (xt == parent) continue;
                if (_subTreeSize[xt] * 1.0 > _subTreeSize[root] * 0.5)
                    HLD(xt, root, chainTop);
                else
                    HLD(xt, root, xt);
            }
        }
        public int LCA(int u, int v)
        {
            while (_chainTop[u] != _chainTop[v])
            {
                if (_depth[_chainTop[u]] < _depth[_chainTop[v]])
                    v = _parent[_chainTop[v]];
                else
                    u = _parent[_chainTop[u]];
            }
            if (_depth[u] < _depth[v])
                return u;
            else
                return v;
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
        public int SubTreeSize(int v)
        {
            return _subTreeSize[v];
        }
    }
    // -------------------------------------------------------------------------
}

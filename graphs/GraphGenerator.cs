using algorithms.structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algorithms.graphs
{
    // ----- Graph Generator ---------------------------------------------------
    //
    // provides methods for creating various graphs
    //
    // Depends on:
    // -- Graph (algorithms.graphs)
    // -- BinaryHeapPQ (algorithms.structures)
    //
    // GraphGenerator(int seed)
    // Graph Simple(int V, int E, bool directed = false)
    // Graph Simple(int V, double p, bool directed = false)
    // Graph Bipartite(int V1, int V2, int E)
    // Graph Bipartite(int V1, int V2, double p)
    // Graph Path(int V, bool directed = false)
    // Graph BinaryTree(int V)
    // Graph Cycle(int V, bool directed = false)
    // Graph EulerianPath(int V, int E, bool directed = false)
    // Graph EulerianCycle(int V, int E, bool directed = false)
    // Graph Wheel(int V, bool directed = false)
    // Graph Star(int V, bool directed = false)
    // Graph Regular(int V, int k, bool directed = false)
    // Graph Tree(int V)
    // -------------------------------------------------------------------------
    public class GraphGenerator
    {
        Random random = null;
        public GraphGenerator(int seed)
        {
            random = new Random(seed);
        }
        public Graph Simple(int V, int E, bool directed = false)
        {
            if (E < 0 || E > (long)V * (V - 1) / 2) return null;
            HashSet<long> hs = new HashSet<long>();
            List<int[]> edges = new List<int[]>();
            while (edges.Count() < E)
            {
                int u = random.Next(V);
                int v = random.Next(V);
                if (u != v)
                {
                    if (!directed && u > v) { int t = u; u = v; v = t; }
                    long key = (long)u * int.MaxValue + v;
                    if (!hs.Contains(key))
                    {
                        hs.Add(key);
                        edges.Add(new int[] { u, v, 1 });
                    }
                }
            }
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph Simple(int V, double p, bool directed = false)
        {
            if (p < 0.0 || p > 1.0) return null;
            List<int[]> edges = new List<int[]>();
            for (int u = 0; u < V; u++)
                for (int v = directed ? 0 : u + 1; v < V; v++)
                    if (u != v && random.NextDouble() < p) edges.Add(new int[] { u, v, 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
        public Graph Bipartite(int V1, int V2, int E)
        {
            if (E < 0 || E > (long)V1 * V2) return null;
            int[] vertices = new int[V1 + V2];
            for (int i = 0; i < V1 + V2; i++) vertices[i] = i;
            Shuffle(vertices);
            HashSet<long> hs = new HashSet<long>();
            List<int[]> edges = new List<int[]>();
            while (edges.Count() < E)
            {
                int u = vertices[random.Next(V1)];
                int v = vertices[V1 + random.Next(V2)];
                long key = (long)u * int.MaxValue + v;
                if (!hs.Contains(key))
                {
                    hs.Add(key);
                    edges.Add(new int[] { u, v, 1 });
                }
            }
            Graph g = new Graph(V1 + V2, edges, false);
            return g;
        }
        public Graph Bipartite(int V1, int V2, double p)
        {
            if (p < 0.0 || p > 1.0) return null;
            int[] vertices = new int[V1 + V2];
            for (int i = 0; i < V1 + V2; i++) vertices[i] = i;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < V1; i++)
                for (int j = 0; j < V2; j++)
                    if (random.NextDouble() < p)
                        edges.Add(new int[] { vertices[i], vertices[V1 + j], 1 });
            Graph g = new Graph(V1 + V2, edges, false);
            return g;
        }
        public Graph Path(int V, bool directed = false)
        {
            int[] vertices = new int[V];
            for (int i = 0; i < V; i++) vertices[i] = i;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < V - 1; i++) edges.Add(new int[] { vertices[i], vertices[i + 1], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph BinaryTree(int V)
        {
            int[] vertices = new int[V];
            for (int i = 0; i < V; i++) vertices[i] = i;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 1; i < V; i++) edges.Add(new int[] { vertices[i], vertices[(i - 1) / 2], 1 });
            Graph g = new Graph(V, edges, false);
            return g;
        }
        public Graph Cycle(int V, bool directed = false)
        {
            int[] vertices = new int[V];
            for (int i = 0; i < V; i++) vertices[i] = i;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < V - 1; i++) edges.Add(new int[] { vertices[i], vertices[i + 1], 1 });
            edges.Add(new int[] { vertices[V - 1], vertices[0], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph EulerianPath(int V, int E, bool directed = false)
        {
            if (E < 0 || V <= 0) return null;
            int[] vertices = new int[E + 1];
            for (int i = 0; i < E + 1; i++) vertices[i] = random.Next(V);
            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < E; i++) edges.Add(new int[] { vertices[i], vertices[i + 1], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph EulerianCycle(int V, int E, bool directed = false)
        {
            if (E <= 0 || V <= 0) return null;
            int[] vertices = new int[E];
            for (int i = 0; i < E; i++) vertices[i] = random.Next(V);
            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < E - 1; i++) edges.Add(new int[] { vertices[i], vertices[i + 1], 1 });
            edges.Add(new int[] { vertices[E - 1], vertices[0], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph Wheel(int V, bool directed = false)
        {
            if (V <= 1) return null;
            int[] vertices = new int[V];
            for (int i = 0; i < V; i++) vertices[i] = i;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 1; i < V - 1; i++) edges.Add(new int[] { vertices[i], vertices[i + 1], 1 });
            edges.Add(new int[] { vertices[V - 1], vertices[1], 1 });
            for (int i = 1; i < V; i++) edges.Add(new int[] { vertices[0], vertices[i], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph Star(int V, bool directed = false)
        {
            if (V <= 0) return null;
            int[] vertices = new int[V];
            for (int i = 0; i < V; i++) vertices[i] = i;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 1; i < V; i++) edges.Add(new int[] { vertices[0], vertices[i], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph Regular(int V, int k, bool directed = false)
        {
            if (V * k % 2 != 0) return null;
            int[] vertices = new int[V * k];
            for (int v = 0; v < V; v++)
                for (int j = 0; j < k; j++)
                    vertices[v + V * j] = v;
            Shuffle(vertices);
            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < V * k / 2; i++)
                edges.Add(new int[] { vertices[2 * i], vertices[2 * i + 1], 1 });
            Graph g = new Graph(V, edges, directed);
            return g;
        }
        public Graph Tree(int V)
        {
            if (V == 1) return new Graph(V, new int[][] { });
            int[] prufer = new int[V - 2];
            for (int i = 0; i < V - 2; i++) prufer[i] = random.Next(V);
            int[] degree = new int[V];
            for (int v = 0; v < V; v++) degree[v] = 1;
            for (int i = 0; i < V - 2; i++) degree[prufer[i]]++;

            BinaryHeapPQ<int> pq = new BinaryHeapPQ<int>();
            for (int v = 0; v < V; v++)
                if (degree[v] == 1)
                    pq.Insert(v);

            List<int[]> edges = new List<int[]>();
            for (int i = 0; i < V - 2; i++)
            {
                int v = pq.ExtractMin();
                edges.Add(new int[] { v, prufer[i], 1 });
                degree[v]--;
                degree[prufer[i]]--;
                if (degree[prufer[i]] == 1) pq.Insert(prufer[i]);
            }
            edges.Add(new int[] { pq.ExtractMin(), pq.ExtractMin(), 1 });

            Graph g = new Graph(V, edges, false);
            return g;
        }
    }
    // -------------------------------------------------------------------------
}

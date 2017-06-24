namespace algorithms.structures
{
    // ----- Disjoint Sets -----------------------------------------------------
    // DisjointSets(int count)
    // void MakeSet(int x)
    // int FindSet(int x)
    // void Union(int x, int y)
    // -------------------------------------------------------------------------
    public class DisjointSets
    {
        int[] parent = null;
        int[] rank = null;
        public DisjointSets(int count)
        {
            parent = new int[count];
            rank = new int[count];
            for (int i = 0; i < count; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }
        }
        public void MakeSet(int x)
        {
            parent[x] = x;
            rank[x] = 0;
        }
        public int FindSet(int x)
        {
            if (parent[x] != x) parent[x] = FindSet(parent[x]);
            return parent[x];
        }
        public void Union(int x, int y)
        {
            int xroot = FindSet(x);
            int yroot = FindSet(y);
            if (rank[xroot] < rank[yroot]) parent[xroot] = yroot;
            else if (rank[xroot] > rank[yroot]) parent[yroot] = xroot;
            else
            {
                parent[yroot] = xroot;
                rank[xroot]++;
            }
        }
    }
    // -------------------------------------------------------------------------
}

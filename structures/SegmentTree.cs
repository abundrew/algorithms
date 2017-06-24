using System;

namespace algorithms.structures
{
    // ----- Segment Tree ------------------------------------------------------
    //
    // A structure for efficient search of cummulative data.
    // Range Minimum (Maximum) Query
    // Range Sum (Multiplication) Query
    //
    // O(nlogn) preprocessing
    // O(logn) a single query
    //
    // LazyPropagation - for range updates, which means when you perform
    // update operations over a range, the update process affects
    // the least nodes as possible so that the bigger the range you want
    // to update the less time it consumes to update it.
    // Eventually those changes will be propagated to the children and
    // the whole array will be up to date.
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SegmentTree.java.html
    //
    // struct Node {
    //   public int Sum { get; set; }
    //   public int Min { get; set; }
    //   public int? PendingValue { get; set; } //Lazily propagated value
    //   ...
    // }
    //
    // SegmentTree(int[] array)
    // int Size
    // int RSumQ(int from, int to)
    // int RMinQ(int from, int to)
    // void Update(int from, int to, int value)
    // -------------------------------------------------------------------------
    public class SegmentTree
    {
        struct Node
        {
            public long Sum { get; set; }
            public int Min { get; set; }
            public int? PendingValue { get; set; } //Lazily propagated value
            public int From { get; set; }
            public int To { get; set; }
            public int Size() { return To - From + 1; }
        }
        Node[] heap = null;
        int[] array = null;
        int size = 0;
        public SegmentTree(int[] array)
        {
            this.array = array;
            //The max size of this array is about 2 * 2 ^ log2(n) + 1
            size = (int)(2 * Math.Pow(2.0, (int)((Math.Log(array.Length) / Math.Log(2.0)) + 1)));
            heap = new Node[size];
            Build(1, 0, array.Length);
        }
        public int Size { get { return array.Length; } }
        void Build(int v, int from, int size)
        {
            heap[v] = new Node();
            heap[v].From = from;
            heap[v].To = from + size - 1;

            if (size == 1)
            {
                heap[v].Sum = array[from];
                heap[v].Min = array[from];
            }
            else
            {
                Build(2 * v, from, size / 2);
                Build(2 * v + 1, from + size / 2, size - size / 2);
                heap[v].Sum = heap[2 * v].Sum + heap[2 * v + 1].Sum;
                heap[v].Min = Math.Min(heap[2 * v].Min, heap[2 * v + 1].Min);
            }
        }
        public long RSumQ(int from, int to)
        {
            return RSumQ(1, from, to);
        }
        long RSumQ(int v, int from, int to)
        {
            Node n = heap[v];
            if (n.PendingValue != null && Contains(n.From, n.To, from, to))
            {
                return (to - from + 1) * (int)n.PendingValue;
            }
            if (Contains(from, to, n.From, n.To))
            {
                return heap[v].Sum;
            }
            if (Intersects(from, to, n.From, n.To))
            {
                Propagate(v);
                long leftSum = RSumQ(2 * v, from, to);
                long rightSum = RSumQ(2 * v + 1, from, to);
                return leftSum + rightSum;
            }
            return 0;
        }
        public int RMinQ(int from, int to)
        {
            return RMinQ(1, from, to);
        }
        int RMinQ(int v, int from, int to)
        {
            Node n = heap[v];
            if (n.PendingValue != null && Contains(n.From, n.To, from, to))
            {
                return (int)n.PendingValue;
            }
            if (Contains(from, to, n.From, n.To))
            {
                return heap[v].Min;
            }
            if (Intersects(from, to, n.From, n.To))
            {
                Propagate(v);
                int leftMin = RMinQ(2 * v, from, to);
                int rightMin = RMinQ(2 * v + 1, from, to);
                return Math.Min(leftMin, rightMin);
            }
            return int.MaxValue;
        }
        public void Update(int from, int to, int value)
        {
            Update(1, from, to, value);
        }
        void Update(int v, int from, int to, int value)
        {
            Node n = heap[v];
            if (Contains(from, to, n.From, n.To))
            {
                Change(n, value);
            }
            if (n.Size() == 1) return;
            if (Intersects(from, to, n.From, n.To))
            {
                Propagate(v);
                Update(2 * v, from, to, value);
                Update(2 * v + 1, from, to, value);
                n.Sum = heap[2 * v].Sum + heap[2 * v + 1].Sum;
                n.Min = Math.Min(heap[2 * v].Min, heap[2 * v + 1].Min);
            }
        }
        void Propagate(int v)
        {
            Node n = heap[v];
            if (n.PendingValue != null)
            {
                Change(heap[2 * v], (int)n.PendingValue);
                Change(heap[2 * v + 1], (int)n.PendingValue);
                n.PendingValue = null;
            }
        }
        void Change(Node n, int value)
        {
            n.PendingValue = value;
            n.Sum = (long)n.Size() * value;
            n.Min = value;
            array[n.From] = value;
        }
        bool Contains(int from1, int to1, int from2, int to2)
        {
            return from2 >= from1 && to2 <= to1;
        }
        bool Intersects(int from1, int to1, int from2, int to2)
        {
            return from1 <= from2 && to1 >= from2   //  (.[..)..] or (.[...]..)
                    || from1 >= from2 && from1 <= to2; // [.(..]..) or [..(..)..
        }
    }
}

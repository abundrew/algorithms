using System;

namespace algorithms.structures
{
    // ----- Binary Heap PQ (Priority Queue) -----------------------------------
    //
    // A heap data structure that takes the form of a binary tree and
    // implements a priority queue
    //
    // O(n) preprocessing
    // O(logn) insert/extract min
    //
    // BinaryHeapPQ<T> where T : IComparable
    //
    // BinaryHeapPQ()
    // BinaryHeapPQ(int capacity)
    // BinaryHeapPQ(T[] a)
    // int Count
    // T Min
    // void Insert(T x)
    // T ExtractMin()
    // -------------------------------------------------------------------------
    public class BinaryHeapPQ<T> where T : IComparable
    {
        int n = 0;
        T[] que = null;
        public BinaryHeapPQ() : this(1) { }
        public BinaryHeapPQ(int capacity)
        {
            que = new T[capacity + 1];
            n = 0;
        }
        public BinaryHeapPQ(T[] a)
        {
            n = a.Length;
            que = new T[n + 1];
            for (int i = 0; i < n; i++) que[i + 1] = a[i];
            for (int i = n / 2; i >= 1; i--) Sink(i);
        }
        public int Count { get { return n; } }
        public T Min { get { return que[1]; } }
        public void Insert(T x)
        {
            if (n == que.Length - 1) Resize(que.Length * 2);
            que[++n] = x;
            Swim(n);
        }
        public T ExtractMin()
        {
            Swap(1, n);
            T min = que[n--];
            Sink(1);
            que[n + 1] = default(T);
            return min;
        }
        void Swim(int k)
        {
            while (k > 1 && que[k/2].CompareTo(que[k]) > 0)
            {
                Swap(k, k / 2);
                k = k / 2;
            }
        }
        void Sink(int k)
        {
            while (2 * k <= n)
            {
                int j = 2 * k;
                if (j < n && que[j].CompareTo(que[j+1]) > 0) j++;
                if (que[k].CompareTo(que[j]) <= 0) break;
                Swap(k, j);
                k = j;
            }
        }
        void Resize(int capacity)
        {
            T[] temp = new T[capacity + 1];
            for (int i = 1; i <= n; i++) temp[i] = que[i];
            que = temp;
        }
        void Swap(int i, int j)
        {
            T temp = que[i];
            que[i] = que[j];
            que[j] = temp;
        }
    }
    // -------------------------------------------------------------------------
}


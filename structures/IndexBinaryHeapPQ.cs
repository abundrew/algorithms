using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.structures
{
    // ----- Index Binary Heap PQ (Priority Queue) -----------------------------
    //
    // http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/IndexMinPQ.java.html
    //
    // Minimum-oriented indexed PQ implementation using a binary heap
    //
    // O(n) preprocessing
    // O(logn) insert/extract min
    //
    // IndexBinaryHeapPQ<T> where T : IComparable
    //
    // IndexBinaryHeapPQ(int maxN)
    // int Count
    // bool Contains(int ix)
    // int Count
    // T KeyOf(int ix)
    // int MinIndex
    // T MinKey
    // bool Insert(int ix, T key)
    // bool Change(int ix, T key)
    // bool Increase(int ix, T key)
    // bool Decrease(int ix, T key)
    // bool Remove(int ix)
    // int ExtractMin()
    // -------------------------------------------------------------------------
    public class IndexBinaryHeapPQ<T> where T : IComparable
    {
        int maxN = 0;
        int n = 0;
        int[] pq = null;
        int[] qp = null;
        T[] keys = null;
        public IndexBinaryHeapPQ(int maxN)
        {
            this.maxN = maxN;
            n = 0;
            keys = new T[maxN + 1];
            pq = new int[maxN + 1];
            qp = new int[maxN + 1];
            for (int i = 0; i <= maxN; i++) qp[i] = -1;
        }
        public int Count { get { return n; } }
        public bool Contains(int ix) { return qp[ix] != -1; }
        public T KeyOf(int ix) { return keys[ix]; }
        public int MinIndex { get { return pq[1]; } }
        public T MinKey { get { return keys[pq[1]]; } }
        public bool Insert(int ix, T key)
        {
            if (Contains(ix)) return false;
            n++;
            qp[ix] = n;
            pq[n] = ix;
            keys[ix] = key;
            Swim(n);
            return true;
        }
        public bool Change(int ix, T key)
        {
            if (!Contains(ix)) return false;
            keys[ix] = key;
            Swim(qp[ix]);
            Sink(qp[ix]);
            return true;
        }
        public bool Increase(int ix, T key)
        {
            if (!Contains(ix)) return false;
            if (keys[ix].CompareTo(key) >= 0) return false;
            keys[ix] = key;
            Sink(qp[ix]);
            return true;
        }
        public bool Decrease(int ix, T key)
        {
            if (!Contains(ix)) return false;
            if (keys[ix].CompareTo(key) <= 0) return false;
            keys[ix] = key;
            Swim(qp[ix]);
            return true;
        }
        public bool Remove(int ix)
        {
            if (!Contains(ix)) return false;
            int index = qp[ix];
            Swap(index, n--);
            Swim(index);
            Sink(index);
            keys[ix] = default(T);
            qp[ix] = -1;
            return true;
        }
        public int ExtractMin()
        {
            int min = pq[1];
            Swap(1, n--);
            Sink(1);
            qp[min] = -1;
            keys[min] = default(T);
            pq[n + 1] = -1;
            return min;
        }
        void Swim(int k)
        {
            while (k > 1 && keys[pq[k / 2]].CompareTo(keys[pq[k]]) > 0)
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
                if (j < n && keys[pq[j]].CompareTo(keys[pq[j + 1]]) > 0) j++;
                if (keys[pq[k]].CompareTo(keys[pq[j]]) <= 0) break;
                Swap(k, j);
                k = j;
            }
        }
        void Swap(int i, int j)
        {
            int swap = pq[i];
            pq[i] = pq[j];
            pq[j] = swap;
            qp[pq[i]] = i;
            qp[pq[j]] = j;
        }
    }
    // -------------------------------------------------------------------------
}

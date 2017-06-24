using System;

namespace algorithms.structures
{
    // ----- Segment Tree RMQ --------------------------------------------------
    //
    // A full binary tree to solve a Range Minimum Query problem
    //
    // O(n) preprocessing
    // O(logn) a single query
    //
    // SegmentTreeRMQ<T> where T: IComparable
    //
    // SegmentTreeRMQ(int n, T maxValue)
    // SegmentTreeRMQ(T[] a, T maxValue)
    // void Update(int ix, T x)
    //
    // -- [left, right)
    //
    // T Min(int left, int right)
    //
    // -- [left
    //
    // int FirstLE(int left, T x)
    //
    // -- right]
    //
    // int LastLE(int right, T x)
    // -------------------------------------------------------------------------
    public class SegmentTreeRMQ<T> where T: IComparable
    {
        int n = 0;
        int half = 0;
        int size = 0;
        T[] segmentTree = null;
        T maxValue = default(T);
        public SegmentTreeRMQ(int n, T maxValue)
        {
            this.n = n;
            half = (int)next_highest_power_of_2((uint)n);
            size = half * 2;
            segmentTree = new T[size];
            for (int i = 0; i < size; i++) segmentTree[i] = maxValue;
        }
        public SegmentTreeRMQ(T[] a, T maxValue)
        {
            n = a.Length;
            half = (int)next_highest_power_of_2((uint)n);
            size = half * 2;
            segmentTree = new T[size];
            this.maxValue = maxValue;
            for (int i = 0; i < n; i++) segmentTree[half + i] = a[i];
            for (int i = half + n; i < size; i++) segmentTree[i] = maxValue;
            for (int i = half - 1; i >= 1; i--) Propagate(i);
        }
        public void Update(int ix, T x)
        {
            segmentTree[half + ix] = x;
            for (int i = (half + ix) / 2; i >= 1; i /= 2) Propagate(i);
        }
        public T Min(int left, int right)
        {
            if (left >= right) return default(T);
            T min = maxValue;
            while (left != 0)
            {
                int f = left & -left;
                if (left + f > right) break;
                T v = segmentTree[(half + left) / f];
                if (v.CompareTo(min) < 0) min = v;
                left += f;
            }
            while (left < right)
            {
                int f = right & -right;
                T v = segmentTree[(half + right) / f - 1];
                if (v.CompareTo(min) < 0) min = v;
                right -= f;
            }
            return min;
        }
        public int FirstLE(int left, T x)
        {
            int current = half + left;
            while (true)
            {
                if (segmentTree[current].CompareTo(x) <= 0)
                {
                    if (current >= half) return current - half;
                    current *= 2;
                } else
                {
                    current++;
                    if ((current & (current - 1)) == 0) return -1;
                    if (current % 2 == 0) current /= 2;
                }
            }
        }
        public int LastLE(int right, T x)
        {
            int current = half + right;
            while (true)
            {
                if (segmentTree[current].CompareTo(x) <= 0)
                {
                    if (current >= half) return current - half;
                    current = current * 2 + 1;
                }
                else
                {
                    if ((current & (current - 1)) == 0) return -1;
                    current--;
                    if (current % 2 != 0) current /= 2;
                }
            }
        }
        void Propagate(int ix)
        {
            segmentTree[ix] = segmentTree[ix * 2].CompareTo(segmentTree[ix * 2 + 1]) < 0 ? segmentTree[ix * 2] : segmentTree[ix * 2 + 1];
        }
        static uint next_highest_power_of_2(uint v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;
        }
    }
    // -------------------------------------------------------------------------
}

using System;

namespace algorithms.math
{
    public static class KthElementInArray
    {
        // ----- Kth Element In Array ------------------------------------------
        //
        // A randomized algorithm based on QuickSort partition
        // ASSUMPTION: ELEMENTS IN A[] ARE DISTINCT
        //
        // O(n)
        //
        // int RandomizedSelect<T>(T[] a, int k)
        // int RandomizedSelect<T>(T[] a, int ix, int len, int k)
        // ---------------------------------------------------------------------
        static Random rnd = new Random();
        public static int RandomizedSelect<T>(T[] a, int k) where T: IComparable
        {
            return RandomizedSelect(a, 0, a.Length, k);
        }
        public static int RandomizedSelect<T>(T[] a, int ix, int len, int k) where T : IComparable
        {
            if (len == 1) return ix;
            int q = RandomizedPartition(a, ix, len);
            int m = q - ix;
            if (k < m) return RandomizedSelect(a, ix, m, k);
            return RandomizedSelect(a, q, len - m, k - m);
        }
        static int RandomizedPartition<T>(T[] a, int ix, int len) where T : IComparable
        {
            int i = rnd.Next(len);
            T temp = a[ix];
            a[ix] = a[ix + i];
            a[ix + i] = temp;
            return Partition(a, ix, len);
        }
        static int Partition<T>(T[]a, int ix, int len) where T : IComparable
        {
            T x = a[ix];
            int i = ix;
            int j = ix + len - 1;
            while (true)
            {
                while (a[j].CompareTo(x) >= 0) { j--; }
                while (a[i].CompareTo(x) < 0) { i++; }
                if (i >= j) return i;
                T temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }
        }
        // ---------------------------------------------------------------------
    }
}

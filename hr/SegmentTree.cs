using System;

namespace algorithms.hr
{
    // ----- Segment Tree ------------------------------------------------------
    //
    // A structure for efficient search of cummulative data.
    //
    // BASE = 128 * 1024
    //
    // O(nlogn) preprocessing
    // O(logn) a single query
    //
    // intervals [l, r) [cl, cr)
    //
    // https://www.hackerrank.com/contests/world-codesprint-9/challenges/box-operations
    // https://www.hackerrank.com/contests/world-codesprint-9/challenges/box-operations/editorial
    //
    // void build(int[] a)
    // void put(int l, int r, int delta, int v = 1, int cl = 0, int cr = BASE)
    // void divide(int l, int r, int x, int v = 1, int cl = 0, int cr = BASE)
    // int getMax(int l, int r, int v = 1, int cl = 0, int cr = BASE)
    // int getMin(int l, int r, int v = 1, int cl = 0, int cr = BASE)
    // long getSum(int l, int r, int v = 1, int cl = 0, int cr = BASE)
    // -------------------------------------------------------------------------
    public class SegmentTree
    {
        const int BASE = 1 << 17;

        long[] sum = new long[BASE * 2];
        int[] vmin = new int[BASE * 2];
        int[] vmax = new int[BASE * 2];
        int[] add = new int[BASE * 2];
        void update(int u)
        {
            vmin[u] = Math.Min(vmin[u * 2], vmin[u * 2 + 1]);
            vmax[u] = Math.Max(vmax[u * 2], vmax[u * 2 + 1]);
            sum[u] = sum[u * 2] + sum[u * 2 + 1];
        }
        void _put(int u, int val, int len)
        {
            add[u] += val;
            vmin[u] += val;
            vmax[u] += val;
            sum[u] += val * len;
        }
        void push(int u, int cl, int cr)
        {
            if (add[u] != 0)
            {
                int len = (cr - cl) / 2;
                _put(u * 2, add[u], len);
                _put(u * 2 + 1, add[u], len);
                add[u] = 0;
            }
        }
        public long getSum(int l, int r, int v = 1, int cl = 0, int cr = BASE)
        {
            if (l <= cl && cr <= r)
                return sum[v];
            if (r <= cl || cr <= l)
                return 0;
            int cc = (cl + cr) / 2;
            push(v, cl, cr);
            return getSum(l, r, v * 2, cl, cc) + getSum(l, r, v * 2 + 1, cc, cr);
        }
        public int getMax(int l, int r, int v = 1, int cl = 0, int cr = BASE)
        {
            if (l <= cl && cr <= r)
                return vmax[v];
            if (r <= cl || cr <= l)
                return int.MinValue;
            int cc = (cl + cr) / 2;
            push(v, cl, cr);
            return Math.Max(getMax(l, r, v * 2, cl, cc), getMax(l, r, v * 2 + 1, cc, cr));
        }
        public int getMin(int l, int r, int v = 1, int cl = 0, int cr = BASE)
        {
            if (l <= cl && cr <= r)
                return vmin[v];
            if (r <= cl || cr <= l)
                return int.MaxValue;
            int cc = (cl + cr) / 2;
            push(v, cl, cr);
            return Math.Min(getMin(l, r, v * 2, cl, cc), getMin(l, r, v * 2 + 1, cc, cr));
        }
        public void put(int l, int r, int delta, int v = 1, int cl = 0, int cr = BASE)
        {
            if (l <= cl && cr <= r)
            {
                _put(v, delta, cr - cl);
                return;
            }
            if (r <= cl || cr <= l)
                return;
            int cc = (cl + cr) / 2;
            push(v, cl, cr);
            put(l, r, delta, v * 2, cl, cc);
            put(l, r, delta, v * 2 + 1, cc, cr);
            update(v);
        }
        int func(int p, int q)
        {
            if (p >= 0) return p / q;
            return -((-p + q - 1) / q);
        }
        public void divide(int l, int r, int x, int v = 1, int cl = 0, int cr = BASE)
        {
            if (x == 1)
                return;
            if (l <= cl && cr <= r)
            {
                int d1 = func(vmin[v], x) - vmin[v];
                int d2 = func(vmax[v], x) - vmax[v];
                if (d1 == d2)
                {
                    _put(v, d1, cr - cl);
                    return;
                }
            }
            if (r <= cl || cr <= l)
                return;
            int cc = (cl + cr) / 2;
            push(v, cl, cr);
            divide(l, r, x, v * 2, cl, cc);
            divide(l, r, x, v * 2 + 1, cc, cr);
            update(v);
        }
        public void build(int[] a)
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
                sum[i + BASE] = vmin[i + BASE] = vmax[i + BASE] = a[i];
            for (int i = 0; i < 2 * BASE; ++i)
                add[i] = 0;
            for (int i = n + BASE; i < 2 * BASE; ++i)
            {
                sum[i] = 0;
                vmin[i] = int.MaxValue;
                vmax[i] = int.MinValue;
            }
            for (int i = BASE - 1; i > 0; --i)
                update(i);
        }
    }
    // -------------------------------------------------------------------------
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.dp
{
    // ----- Convex Hull Trick -------------------------------------------------
    //
    // To find minimum ( A[i] * x + B[i] ) for every x.
    // -- Lines are added with DESCENDING A.
    //
    // ConvexHullTrick(int maxsize)
    // void Add(long a, long b)
    // long Query(long x)
    // -------------------------------------------------------------------------
    public class ConvexHullTrick
    {
        struct Line {
            public long A;
            public long B;
            public Line(long a, long b)
            {
                A = a;
                B = b;
            }
            public long Get(long x) { return A * x + B; }
            public double Get(double x) { return A * x + B; }
        }
        Line[] hull;
        int size;
        public ConvexHullTrick(int maxsize)
        {
            hull = new Line[maxsize + 1];
            size = 0;
        }
        bool IsBad(Line prev, Line curr, Line next)
        {
            return (double)(prev.B - curr.B) * (next.A - curr.A) >= (double)(curr.B - next.B) * (curr.A - prev.A);
        }
        public void Add(long a, long b)
        {
            Line newline = new Line(a, b);
            hull[size++] = newline;
            while (size > 2 && IsBad(hull[size - 3], hull[size - 2], hull[size - 1])) {
                hull[size - 2] = newline;
                size--;
            }
        }
        public long Query(long x)
        {
            int l, r;
            l = 0; r = size - 1;
            while (l < r)
            {
                int m = (l + r) >> 1;
                if (hull[m].Get(x) >= hull[m + 1].Get(x))
                    l = m + 1;
                else
                    r = m;
            }
            if (l >= size) return long.MaxValue;
            return hull[l].Get(x);
        }
    }
    // -------------------------------------------------------------------------
}

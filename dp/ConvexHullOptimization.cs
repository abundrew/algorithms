using System;

namespace algorithms.dp
{
    // ----- Convex Hull Optimization ------------------------------------------
    //
    // To find minimum ( A[i] * x + B[i] ) for every x.
    // -- Lines are added with DESCENDING A.
    // -- Minimums are received with ASCENDING X.
    //
    // ConvexHullOptimization(int maxsize)
    // void AddLine(long a, long b)
    // long MinValue(long x)
    // -------------------------------------------------------------------------
    public class ConvexHullOptimization
    {
        long[] A;
        long[] B;
        int len;
        int ptr;
        public ConvexHullOptimization(int maxsize)
        {
            long[] A = new long[maxsize];
            long[] B = new long[maxsize];
        }
        // a descends
        public void AddLine(long a, long b)
        {
            // intersection of (A[len-2],B[len-2]) with (A[len-1],B[len-1]) must lie to the left of intersection of (A[len-1],B[len-1]) with (a,b)
            while (len >= 2 && (B[len - 2] - B[len - 1]) * (a - A[len - 1]) >= (B[len - 1] - b) * (A[len - 1] - A[len - 2])) len--;
            A[len] = a;
            B[len] = b;
            len++;
        }

        // x ascends
        public long MinValue(long x)
        {
            ptr = Math.Min(ptr, len - 1);
            while (ptr + 1 < len && A[ptr + 1] * x + B[ptr + 1] <= A[ptr] * x + B[ptr]) ptr++;
            return A[ptr] * x + B[ptr];
        }
    }
    // -------------------------------------------------------------------------
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.hr
{
    // ----- MO’s Algorithm (Query square root decomposition) ------------------
    //
    // https://blog.anudeep2011.com/mos-algorithm/
    //
    // int n - array size
    // int[][] qlr - queries [L, R]
    // Action<int> addL - add current L
    // Action<int> addR - add current R
    // Action<int> removeL - remove current L
    // Action<int> removeR - remove current R
    // Action<int> runQ - run current query 
    //
    // MOs(int n, int[][] qlr, Action<int, int> addL, Action<int, int> removeL,
    //     Action<int, int> addR, Action<int, int> removeR, Action<int> runQ)
    // void Run()
    // -------------------------------------------------------------------------
    public class MOs
    {
        int N;
        int[][] QLR;
        Action<int> AddL, RemoveL, AddR, RemoveR, RunQ;
        public MOs(int n, int[][] qlr, Action<int> addL, Action<int> removeL, Action<int> addR, Action<int> removeR, Action<int> runQ)
        {
            N = n;
            QLR = qlr;
            AddL = addL;
            AddR = addR;
            RemoveL = removeL;
            RemoveR = removeR;
            RunQ = runQ;
        }
        public void Run()
        {
            int blockSize = (int)Math.Sqrt(N);
            int[] xQ = new int[QLR.Length];
            for (int i = 0; i < xQ.Length; i++) xQ[i] = i;
            Array.Sort(xQ, (p1, p2) => {
                int cmp = (QLR[p1][0] / blockSize).CompareTo(QLR[p2][0] / blockSize);
                if (cmp == 0) cmp = QLR[p1][1].CompareTo(QLR[p2][1]);
                return cmp;
            });

            int L = 0;
            int R = 0;

            for (int i = 0; i < xQ.Length; i++) {
                while (L < QLR[xQ[i]][0]) { RemoveL(L); L++; }
                while (L > QLR[xQ[i]][0]) { AddL(L); L--; }
                while (R < QLR[xQ[i]][1]) { AddR(R); R++; }
                while (R > QLR[xQ[i]][1]) { RemoveR(R); R--; }
                RunQ(xQ[i]);
            }
        }
    }
    // -------------------------------------------------------------------------
}

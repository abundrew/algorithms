using System.Linq;

namespace algorithms.strings
{
    public static class SuffixArray
    {
        // ----- Suffix Array --------------------------------------------------
        //
        // SAIS - the induced sorting based suffix array construction algorithm
        //
        // "sorted array of all substrings"
        //
        // O(n) worst-case time
        // MAX(2n, 4k) worst-case extra working space
        // n and k are the length of the input string and the size of alphabet
        //
        // https://sites.google.com/site/yuta256/sais
        //
        // int[] SA(byte[] T)
        // int[] SA(char[] T)
        // int[] SA(int[] T, int alphabetSize)
        // int[] SA(string T)
        // ---------------------------------------------------------------------
        interface BaseArray
        {
            int this[int index] { get; set; }
        }
        class ByteArray : BaseArray
        {
            byte[] _arr;
            int _ofs;
            public ByteArray(byte[] arr, int ofs)
            {
                _arr = arr;
                _ofs = ofs;
            }
            public int this[int index]
            {
                get { return (int)_arr[_ofs + index]; }
                set { _arr[_ofs + index] = (byte)value; }
            }
        }
        class CharArray : BaseArray
        {
            char[] _arr;
            int _ofs;
            public CharArray(char[] arr, int ofs)
            {
                _arr = arr;
                _ofs = ofs;
            }
            public int this[int index]
            {
                get { return (int)_arr[_ofs + index]; }
                set { _arr[_ofs + index] = (char)value; }
            }
        }
        class IntArray : BaseArray
        {
            int[] _arr;
            int _ofs;
            public IntArray(int[] arr, int ofs)
            {
                _arr = arr;
                _ofs = ofs;
            }
            public int this[int index]
            {
                get { return _arr[_ofs + index]; }
                set { _arr[_ofs + index] = value; }
            }
        }
        const int MINBUCKETSIZE = 256;
        static void GetCounts(BaseArray T, BaseArray C, int n, int k)
        {
            int i;
            for (i = 0; i < k; ++i) { C[i] = 0; }
            for (i = 0; i < n; ++i) { C[T[i]] = C[T[i]] + 1; }
        }
        static void GetBuckets(BaseArray C, BaseArray B, int k, bool end)
        {
            int i, sum = 0;
            if (end != false) { for (i = 0; i < k; ++i) { sum += C[i]; B[i] = sum; } }
            else { for (i = 0; i < k; ++i) { sum += C[i]; B[i] = sum - C[i]; } }
        }
        static void LMSSort(BaseArray T, int[] SA, BaseArray C, BaseArray B, int n, int k)
        {
            int b, i, j;
            int c0, c1;
            if (C == B) { GetCounts(T, C, n, k); }
            GetBuckets(C, B, k, false);
            j = n - 1;
            b = B[c1 = T[j]];
            --j;
            SA[b++] = (T[j] < c1) ? ~j : j;
            for (i = 0; i < n; ++i)
            {
                if (0 < (j = SA[i]))
                {
                    if ((c0 = T[j]) != c1) { B[c1] = b; b = B[c1 = c0]; }
                    --j;
                    SA[b++] = (T[j] < c1) ? ~j : j;
                    SA[i] = 0;
                }
                else if (j < 0)
                {
                    SA[i] = ~j;
                }
            }
            if (C == B) { GetCounts(T, C, n, k); }
            GetBuckets(C, B, k, true);
            for (i = n - 1, b = B[c1 = 0]; 0 <= i; --i)
            {
                if (0 < (j = SA[i]))
                {
                    if ((c0 = T[j]) != c1) { B[c1] = b; b = B[c1 = c0]; }
                    --j;
                    SA[--b] = (T[j] > c1) ? ~(j + 1) : j;
                    SA[i] = 0;
                }
            }
        }
        static int LMSPostProc(BaseArray T, int[] SA, int n, int m)
        {
            int i, j, p, q, plen, qlen, name;
            int c0, c1;
            bool diff;
            for (i = 0; (p = SA[i]) < 0; ++i) { SA[i] = ~p; }
            if (i < m)
            {
                for (j = i, ++i; ; ++i)
                {
                    if ((p = SA[i]) < 0)
                    {
                        SA[j++] = ~p; SA[i] = 0;
                        if (j == m) { break; }
                    }
                }
            }
            i = n - 1; j = n - 1; c0 = T[n - 1];
            do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) >= c1));
            for (; 0 <= i;)
            {
                do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) <= c1));
                if (0 <= i)
                {
                    SA[m + ((i + 1) >> 1)] = j - i; j = i + 1;
                    do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) >= c1));
                }
            }
            for (i = 0, name = 0, q = n, qlen = 0; i < m; ++i)
            {
                p = SA[i]; plen = SA[m + (p >> 1)]; diff = true;
                if ((plen == qlen) && ((q + plen) < n))
                {
                    for (j = 0; (j < plen) && (T[p + j] == T[q + j]); ++j) { }
                    if (j == plen) { diff = false; }
                }
                if (diff != false) { ++name; q = p; qlen = plen; }
                SA[m + (p >> 1)] = name;
            }
            return name;
        }
        static void InduceSA(BaseArray T, int[] SA, BaseArray C, BaseArray B, int n, int k)
        {
            int b, i, j;
            int c0, c1;
            if (C == B) { GetCounts(T, C, n, k); }
            GetBuckets(C, B, k, false);
            j = n - 1;
            b = B[c1 = T[j]];
            SA[b++] = ((0 < j) && (T[j - 1] < c1)) ? ~j : j;
            for (i = 0; i < n; ++i)
            {
                j = SA[i]; SA[i] = ~j;
                if (0 < j)
                {
                    if ((c0 = T[--j]) != c1) { B[c1] = b; b = B[c1 = c0]; }
                    SA[b++] = ((0 < j) && (T[j - 1] < c1)) ? ~j : j;
                }
            }
            if (C == B) { GetCounts(T, C, n, k); }
            GetBuckets(C, B, k, true);
            for (i = n - 1, b = B[c1 = 0]; 0 <= i; --i)
            {
                if (0 < (j = SA[i]))
                {
                    if ((c0 = T[--j]) != c1) { B[c1] = b; b = B[c1 = c0]; }
                    SA[--b] = ((j == 0) || (T[j - 1] > c1)) ? ~j : j;
                }
                else
                {
                    SA[i] = ~j;
                }
            }
        }
        static int ComputeBWT(BaseArray T, int[] SA, BaseArray C, BaseArray B, int n, int k)
        {
            int b, i, j, pidx = -1;
            int c0, c1;
            if (C == B) { GetCounts(T, C, n, k); }
            GetBuckets(C, B, k, false);
            j = n - 1;
            b = B[c1 = T[j]];
            SA[b++] = ((0 < j) && (T[j - 1] < c1)) ? ~j : j;
            for (i = 0; i < n; ++i)
            {
                if (0 < (j = SA[i]))
                {
                    SA[i] = ~(c0 = T[--j]);
                    if (c0 != c1) { B[c1] = b; b = B[c1 = c0]; }
                    SA[b++] = ((0 < j) && (T[j - 1] < c1)) ? ~j : j;
                }
                else if (j != 0)
                {
                    SA[i] = ~j;
                }
            }
            if (C == B) { GetCounts(T, C, n, k); }
            GetBuckets(C, B, k, true);
            for (i = n - 1, b = B[c1 = 0]; 0 <= i; --i)
            {
                if (0 < (j = SA[i]))
                {
                    SA[i] = (c0 = T[--j]);
                    if (c0 != c1) { B[c1] = b; b = B[c1 = c0]; }
                    SA[--b] = ((0 < j) && (T[j - 1] > c1)) ? ~((int)T[j - 1]) : j;
                }
                else if (j != 0)
                {
                    SA[i] = ~j;
                }
                else
                {
                    pidx = i;
                }
            }
            return pidx;
        }
        static int SAISMain(BaseArray T, int[] SA, int fs, int n, int k, bool isbwt)
        {
            BaseArray C, B, RA;
            int i, j, b, m, p, q, name, pidx = 0, newfs;
            int c0, c1;
            uint flags = 0;
            if (k <= MINBUCKETSIZE)
            {
                C = new IntArray(new int[k], 0);
                if (k <= fs) { B = new IntArray(SA, n + fs - k); flags = 1; }
                else { B = new IntArray(new int[k], 0); flags = 3; }
            }
            else if (k <= fs)
            {
                C = new IntArray(SA, n + fs - k);
                if (k <= (fs - k)) { B = new IntArray(SA, n + fs - k * 2); flags = 0; }
                else if (k <= (MINBUCKETSIZE * 4)) { B = new IntArray(new int[k], 0); flags = 2; }
                else { B = C; flags = 8; }
            }
            else
            {
                C = B = new IntArray(new int[k], 0);
                flags = 4 | 8;
            }
            GetCounts(T, C, n, k); GetBuckets(C, B, k, true);
            for (i = 0; i < n; ++i) { SA[i] = 0; }
            b = -1; i = n - 1; j = n; m = 0; c0 = T[n - 1];
            do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) >= c1));
            for (; 0 <= i;)
            {
                do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) <= c1));
                if (0 <= i)
                {
                    if (0 <= b) { SA[b] = j; }
                    b = --B[c1]; j = i; ++m;
                    do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) >= c1));
                }
            }
            if (1 < m)
            {
                LMSSort(T, SA, C, B, n, k);
                name = LMSPostProc(T, SA, n, m);
            }
            else if (m == 1)
            {
                SA[b] = j + 1;
                name = 1;
            }
            else
            {
                name = 0;
            }
            if (name < m)
            {
                if ((flags & 4) != 0) { C = null; B = null; }
                if ((flags & 2) != 0) { B = null; }
                newfs = (n + fs) - (m * 2);
                if ((flags & (1 | 4 | 8)) == 0)
                {
                    if ((k + name) <= newfs) { newfs -= k; }
                    else { flags |= 8; }
                }
                for (i = m + (n >> 1) - 1, j = m * 2 + newfs - 1; m <= i; --i)
                {
                    if (SA[i] != 0) { SA[j--] = SA[i] - 1; }
                }
                RA = new IntArray(SA, m + newfs);
                SAISMain(RA, SA, newfs, m, name, false);
                RA = null;
                i = n - 1; j = m * 2 - 1; c0 = T[n - 1];
                do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) >= c1));
                for (; 0 <= i;)
                {
                    do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) <= c1));
                    if (0 <= i)
                    {
                        SA[j--] = i + 1;
                        do { c1 = c0; } while ((0 <= --i) && ((c0 = T[i]) >= c1));
                    }
                }

                for (i = 0; i < m; ++i) { SA[i] = SA[m + SA[i]]; }
                if ((flags & 4) != 0) { C = B = new IntArray(new int[k], 0); }
                if ((flags & 2) != 0) { B = new IntArray(new int[k], 0); }
            }
            if ((flags & 8) != 0) { GetCounts(T, C, n, k); }
            if (1 < m)
            {
                GetBuckets(C, B, k, true);
                i = m - 1; j = n; p = SA[m - 1]; c1 = T[p];
                do
                {
                    q = B[c0 = c1];
                    while (q < j) { SA[--j] = 0; }
                    do
                    {
                        SA[--j] = p;
                        if (--i < 0) { break; }
                        p = SA[i];
                    } while ((c1 = T[p]) == c0);
                } while (0 <= i);
                while (0 < j) { SA[--j] = 0; }
            }
            if (isbwt == false) { InduceSA(T, SA, C, B, n, k); }
            else { pidx = ComputeBWT(T, SA, C, B, n, k); }
            C = null; B = null;
            return pidx;
        }
        static int SufSort(byte[] T, int[] SA, int n)
        {
            if ((T == null) || (SA == null) || (T.Length < n) || (SA.Length < n)) { return -1; }
            if (n <= 1) { if (n == 1) { SA[0] = 0; } return 0; }
            return SAISMain(new ByteArray(T, 0), SA, 0, n, 256, false);
        }
        static int SufSort(char[] T, int[] SA, int n)
        {
            if ((T == null) || (SA == null) || (T.Length < n) || (SA.Length < n)) { return -1; }
            if (n <= 1) { if (n == 1) { SA[0] = 0; } return 0; }
            return SAISMain(new CharArray(T, 0), SA, 0, n, 128, false);
        }
        static int SufSort(int[] T, int[] SA, int n, int alphabetSize)
        {
            if ((T == null) || (SA == null) || (T.Length < n) || (SA.Length < n)) { return -1; }
            if (n <= 1) { if (n == 1) { SA[0] = 0; } return 0; }
            return SAISMain(new IntArray(T, 0), SA, 0, n, alphabetSize, false);
        }
        public static int[] SA(byte[] T)
        {
            int n = T.Length;
            int[] sa = new int[n];
            SufSort(T, sa, n);
            return sa;
        }
        public static int[] SA(char[] T)
        {
            int n = T.Length;
            int[] sa = new int[n];
            SufSort(T, sa, n);
            return sa;
        }
        public static int[] SA(int[] T, int alphabetSize)
        {
            int n = T.Length;
            int[] sa = new int[n];
            SufSort(T, sa, n, alphabetSize);
            return sa;
        }
        public static int[] SA(string T)
        {
            return SA(T.ToArray());
        }
        // ---------------------------------------------------------------------
    }
}

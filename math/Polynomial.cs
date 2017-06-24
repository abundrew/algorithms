using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.math
{
    // ----- Polynomial --------------------------------------------------------
    //
    // polynomial = sum c_i * x^e_i
    //
    // static Polynomial Build(params Term[] t)
    // static Polynomial Build(params long[] c)
    // long Substitute(int v)
    // static Polynomial Add(params Polynomial[] a)
    // static Polynomial Mul(Polynomial a, long v)
    // Polynomial Mul(long v)
    // static Polynomial Mul(Polynomial a, Polynomial b)
    // static Polynomial Pow(Polynomial a, int n)
    // string ToString()
    // -------------------------------------------------------------------------
    public class Polynomial
    {
        public struct Term
        {
            public long C { get; set; }
            public int E { get; set; }
            public Term(long c, int e) { C = c; E = e; }
        }
        public Term[] terms { get; private set; }

        private Polynomial() : base() { }
        public static Polynomial Build(params Term[] t)
        {
            Polynomial p = new Polynomial();
            p.terms = t;
            return p.Simplify();
        }
        // x^0, x^1, ...
        public static Polynomial Build(params long[] c)
        {
            Polynomial p = new Polynomial();
            int n = c.Length;
            p.terms = new Term[n];
            for (int i = 0; i < n; i++)
            {
                p.terms[i] = new Term(c[i], i);
            }
            return p.Simplify();
        }

        public long Substitute(int v)
        {
            long ret = 0;
            foreach (Term t in terms)
            {
                long lret = t.C;
                for (int j = 0; j < t.E; j++) lret *= v;
                ret += lret;
            }
            return ret;
        }

        public static Polynomial Add(params Polynomial[] a)
        {
            Polynomial p = new Polynomial();
            int len = 0;
            foreach (Polynomial spm in a) len += spm.terms.Length;
            p.terms = new Term[len];
            int q = 0;
            foreach (Polynomial spm in a)
                foreach (Term t in spm.terms)
                    p.terms[q++] = new Term(t.C, t.E);
            return p.Simplify();
        }

        public static Polynomial Mul(Polynomial a, long v)
        {
            Polynomial p = new Polynomial();
            int m = a.terms.Length;
            p.terms = new Term[m];
            for (int i = 0; i < m; i++)
                p.terms[i] = new Term(a.terms[i].C * v, a.terms[i].E);
            return p.Simplify();
        }

        public Polynomial Mul(long v)
        {
            for (int i = 0; i < terms.Length; i++) terms[i].C *= v;
            return v == 0 ? Simplify() : this;
        }

        public static Polynomial Mul(Polynomial a, Polynomial b)
        {
            Polynomial p = new Polynomial();
            int m = a.terms.Length, n = b.terms.Length;
            p.terms = new Term[m * n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Term t = new Term(
                        a.terms[i].C * b.terms[j].C,
                        a.terms[i].E + b.terms[j].E
                                );
                    p.terms[i * n + j] = t;
                }
            }
            return p.Simplify();
        }

        static int numberOfLeadingZeros(long i)
        {
            if (i == 0) return 64;
            int n = 1;
            uint x = (uint)((ulong)i >> 32);
            if (x == 0) { n += 32; x = (uint)i; }
            if (x >> 16 == 0) { n += 16; x <<= 16; }
            if (x >> 24 == 0) { n += 8; x <<= 8; }
            if (x >> 28 == 0) { n += 4; x <<= 4; }
            if (x >> 30 == 0) { n += 2; x <<= 2; }
            n -= (int)(x >> 31);
            return n;
        }

        public static Polynomial Pow(Polynomial a, int n)
        {
            if (a.terms.Length == 0) return a;
            Polynomial ret = new Polynomial();
            ret.terms = new Term[] { new Term(1L, 0) };
            int x = 63 - numberOfLeadingZeros(n);
            for (; x >= 0; x--)
            {
                ret = Mul(ret, ret);
                if (n << 63 - x < 0) ret = Mul(ret, a);
            }
            return ret;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (Term t in terms)
            {
                if (t.C >= 0 && !first) sb.Append('+');
                first = false;
                if (t.C == -1 || t.C == 1)
                {
                    if (t.E == 0)
                        sb.Append(t.C);
                    else
                        if (t.C == -1)
                        sb.Append('-');
                }
                else
                    sb.Append(t.C);
                if (t.E > 0)
                {
                    sb.Append('x');
                    if (t.E >= 2) sb.Append("^" + t.E);
                }
            }
            return sb.ToString();
        }

        Polynomial Simplify()
        {
            if (terms.Length == 0) return this;
            Array.Sort(terms, (a, b) => -(a.E - b.E));
            int n = terms.Length;
            int p = 1;
            for (int i = 1; i < n; i++)
            {
                if (terms[i].E == terms[p - 1].E)
                    terms[p - 1].C += terms[i].C;
                else
                {
                    if (terms[p - 1].C == 0L) p--;
                    terms[p++] = terms[i];
                }
            }
            if (terms[p - 1].C == 0L) p--;
            if (terms.Length != p)
            {
                Term[] t = new Term[p];
                Array.Copy(terms, t, p);
                terms = t;
            }
            return this;
        }
    }
    // -------------------------------------------------------------------------
}

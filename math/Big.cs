using System;
using System.Collections.Generic;
using System.Text;

namespace algorithms.math
{
    // ----- Big (Long Arithmetics) --------------------------------------------
    // Big()
    // Big(string s)
    // Big(int n), n < RADIX
    // Big(Big big)
    // int Length
    // string ToString()
    // void Abs()
    // int CompareTo(Big big)
    // Big Add(Big big)
    // Big Sub(Big big)
    // Big Mul(int k), k < RADIX
    // Big Quotient(int k), k < RADIX
    // int Remainder(int k), k < RADIX
    // Big Power(int k), 0 <= k < RADIX
    // Big Mul(Big big)
    // static int Compare(Big big1, Big big2)
    // static Big Add(Big big1, Big big2)
    // static Big Sub(Big big1, Big big2)
    // static Big Mul(Big big, int k), k < RADIX
    // static Big Quotient(Big big, int k), k < RADIX
    // static int Remainder(Big big, int k), k < RADIX
    // static Big Power(Big big, int k), 0 <= k < RADIX
    // static Big Mul(Big big1, Big big2)
    // static Big Fact(int n), n < RADIX
    // -------------------------------------------------------------------------
    public class Big
    {
        const int RADIX = 1000 * 1000 * 1000;
        const int RADIX_WIDTH = 9;
        bool Neg { get; set; }
        List<int> Num { get; set; }
        public Big()
        {
            Neg = false;
            Num = new List<int> { 0 };
        }
        public Big(string s)
        {
            Neg = s[0] == '-';
            Num = new List<int>();
            if (Neg) s = s.Substring(1);
            s = s.TrimStart('0');
            if (string.IsNullOrEmpty(s)) s = "0";
            int end = s.Length;
            while (end > 0)
            {
                int w = Math.Min(RADIX_WIDTH, end);
                Num.Add(int.Parse(s.Substring(end - w, w)));
                end -= w;
            }
        }
        public Big(int n)
        {
            Neg = n < 0;
            Num = new List<int>() { n };
        }
        public Big(Big big)
        {
            Neg = big.Neg;
            Num = new List<int>(big.Num);
        }
        public int Length { get { return Num.Count; } }
        public override string ToString()
        {
            string s = NumToString(Num);
            if (Neg) s = "-" + s;
            return s;
        }
        public void Abs()
        {
            Neg = false;
        }
        public int CompareTo(Big big)
        {
            return Compare(this, big);
        }
        public Big Add(Big big)
        {
            return Add(this, big);
        }
        public Big Sub(Big big)
        {
            return Sub(this, big);
        }
        public Big Mul(int k)
        {
            return Mul(this, k);
        }
        public Big Quotient(int k)
        {
            return Quotient(this, k);
        }
        public int Remainder(int k)
        {
            return Remainder(this, k);
        }
        public Big Power(int k)
        {
            return Power(this, k);
        }
        public Big Mul(Big big)
        {
            return Mul(this, big);
        }
        public static int Compare(Big big1, Big big2)
        {
            if (big1.Neg != big2.Neg) return big1.Neg ? -1 : 1;
            if (big1.Neg) return Compare(big2.Num, big1.Num);
            return Compare(big1.Num, big2.Num);
        }
        static Big AddSub(Big big1, Big big2, bool add)
        {
            if (add)
                return new Big() { Neg = big1.Neg, Num = Big.Sum(big1.Num, big2.Num) };
            else
            {
                int cmp = Big.Compare(big1.Num, big2.Num);
                switch (cmp)
                {
                    case -1:
                        return new Big() { Neg = !big1.Neg, Num = Big.Dif(big2.Num, big1.Num) };
                    case 1:
                        return new Big() { Neg = big1.Neg, Num = Big.Dif(big1.Num, big2.Num) };
                }
                return new Big(0);
            }
        }
        public static Big Add(Big big1, Big big2)
        {
            return AddSub(big1, big2, big1.Neg == big2.Neg);
        }
        public static Big Sub(Big big1, Big big2)
        {
            return AddSub(big1, big2, big1.Neg != big2.Neg);
        }
        public static Big Mul(Big big, int k)
        {
            return new Big() { Neg = big.Neg != (k < 0), Num = Mul(big.Num, Math.Abs(k)) };
        }
        public static Big Quotient(Big big, int k)
        {
            return new Big() { Neg = big.Neg != (k < 0), Num = Quotient(big.Num, Math.Abs(k)) };
        }
        public static int Remainder(Big big, int k)
        {
            return Remainder(big.Num, Math.Abs(k));
        }
        public static Big Power(Big big, int k)
        {
            return new Big() { Neg = big.Neg && (k % 2 != 0), Num = Power(big.Num, k) };
        }
        public static Big Mul(Big big1, Big big2)
        {
            return new Big() { Neg = big1.Neg != big2.Neg, Num = Mul(big1.Num, big2.Num) };
        }
        public static Big Fact(int n)
        {
            Big big = new Big(1);
            for (int i = 2; i <= n; i++) big = Mul(big, i);
            return big;
        }
        static string NumToString(List<int> num)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int b in num)
                sb.Insert(0, b.ToString().PadLeft(RADIX_WIDTH, '0'));
            string s = sb.ToString().TrimStart('0');
            if (string.IsNullOrEmpty(s)) s = "0";
            return s;
        }
        static int Compare(List<int> num1, List<int> num2)
        {
            Norm(num1);
            Norm(num2);
            int cmp = num1.Count.CompareTo(num2.Count);
            int ix = num1.Count;
            while (cmp == 0 && ix > 0)
            {
                ix--;
                cmp = num1[ix].CompareTo(num2[ix]);
            }
            return cmp;
        }
        static List<int> Sum(List<int> num1, List<int> num2)
        {
            int len = Math.Max(num1.Count, num2.Count);
            List<int> sum = new List<int>(new int[len]);
            int ix = 0;
            bool carry = false;
            while (ix < len || carry)
            {
                if (ix == sum.Count) sum.Add(0);
                int b1 = ix < num1.Count ? num1[ix] : 0;
                int b2 = ix < num2.Count ? num2[ix] : 0;
                int b = b1 + b2 + (carry ? 1 : 0);
                carry = b >= RADIX;
                if (carry) b -= RADIX;
                sum[ix] = b;
                ix++;
            }
            return sum;
        }
        static List<int> Dif(List<int> num1, List<int> num2)
        {
            List<int> dif = new List<int>(new int[num1.Count]);
            int ix = 0;
            bool carry = false;
            while (ix < num2.Count || carry)
            {
                int b1 = num1[ix];
                int b2 = ix < num2.Count ? num2[ix] : 0;
                int b = b1 - b2 - (carry ? 1 : 0);
                carry = b < 0;
                if (carry) b += RADIX;
                dif[ix] = b;
                ix++;
            }
            while (ix < num1.Count)
            {
                dif[ix] = num1[ix];
                ix++;
            }
            Norm(dif);
            return dif;
        }
        static List<int> Mul(List<int> num, int k)
        {
            List<int> product = new List<int>(new int[num.Count]);
            int ix = 0;
            int carry = 0;
            while (ix < num.Count || carry > 0)
            {
                if (ix == product.Count) product.Add(0);
                long nx = ix < num.Count ? num[ix] : 0;
                long cur = nx * k + carry;
                product[ix] = (int)(cur % RADIX);
                carry = (int)(cur / RADIX);
                ix++;
            }
            Norm(product);
            return product;
        }
        static List<int> Quotient(List<int> num, int k)
        {
            List<int> quotient = new List<int>(new int[num.Count]);
            int carry = 0;
            for (int ix = num.Count - 1; ix >= 0; ix--)
            {
                long cur = num[ix] + (long)carry * RADIX;
                quotient[ix] = (int)(cur / k);
                carry = (int)(cur % k);
            }
            Norm(quotient);
            return quotient;
        }
        static int Remainder(List<int> num, int k)
        {
            int carry = 0;
            for (int ix = num.Count - 1; ix >= 0; ix--)
            {
                carry = (int)((num[ix] + (long)carry * RADIX) % k);
            }
            return carry;
        }
        static List<int> Power(List<int> num, int k)
        {
            List<int> power = new List<int>(new int[] { 1 });
            while (k > 0)
            {
                if ((k & 1) == 1) power = Mul(power, num);
                num = Mul(num, num);
                k >>= 1;
            }
            Norm(power);
            return power;
        }
        static List<int> Mul(List<int> num1, List<int> num2)
        {
            List<int> c = new List<int>(new int[num1.Count + num2.Count]);
            for (int ix = 0; ix < num1.Count; ix++)
            {
                int iy = 0;
                int carry = 0;
                while (iy < num2.Count || carry > 0)
                {
                    int b = iy < num2.Count ? num2[iy] : 0;
                    long cur = c[ix + iy] + (long)num1[ix] * b + carry;
                    c[ix + iy] = (int)(cur % RADIX);
                    carry = (int)(cur / RADIX);
                    iy++;
                }
            }
            Norm(c);
            return c;
        }
        static void Norm(List<int> num)
        {
            while (num.Count > 1 && num[num.Count - 1] == 0) num.RemoveAt(num.Count - 1);
        }
    }
    // -------------------------------------------------------------------------
}

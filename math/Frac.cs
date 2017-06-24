using System;
using System.Numerics;

namespace algorithms.math
{
    // ----- Frac (Fraction) ---------------------------------------------------
    // Frac()
    // Frac(long num)
    // Frac(long num, long den)
    // Frac(BigInteger num)
    // Frac(BigInteger num, BigInteger den)
    // Frac Reduce()
    // static Frac Add(Frac a, Frac b)
    // static Frac Sub(Frac a, Frac b) 
    // static Frac Mul(Frac a, Frac b)
    // static Frac Div(Frac a, Frac b)
    // static Frac Inv(Frac a)
    // Frac Add(Frac b)
    // Frac Sub(Frac b)
    // Frac Mul(Frac b)
    // Frac Div(Frac b)
    // Frac Inv()
    // string ToString()
    // string ToStringSimple()
    // int CompareTo(Frac f)
    // bool SimpleEquals(Frac f)
    // bool Equals(Object o)
    // int GetHashCode() 
    // -------------------------------------------------------------------------
    public class Frac: IComparable<Frac>
    {
        public BigInteger num { get; private set; }
        public BigInteger den { get; private set; }

        public Frac() { this.num = BigInteger.Zero; this.den = BigInteger.One; }
        public Frac(long num) { this.num = new BigInteger(num); this.den = BigInteger.One; }
        public Frac(long num, long den) { this.num = new BigInteger(num); this.den = new BigInteger(den); Reduce(); }
        public Frac(BigInteger num) { this.num = num; this.den = BigInteger.One; }
        public Frac(BigInteger num, BigInteger den) { this.num = num; this.den = den; Reduce(); }

        public Frac Reduce()
        {
            if (den.Sign == 0) { }
            else if (num.Sign == 0) { den = BigInteger.One; }
            else
            {
                if (den.Sign < 0)
                {
                    num = BigInteger.Negate(num);
                    den = BigInteger.Negate(den);
                }
                BigInteger g = BigInteger.GreatestCommonDivisor(num, den);
                num = BigInteger.Divide(num, g);
                den = BigInteger.Divide(den, g);
            }
            return this;
        }

        public static Frac Add(Frac a, Frac b) { return new Frac(BigInteger.Add(BigInteger.Multiply(a.num, b.den), BigInteger.Multiply(a.den, b.num)), BigInteger.Multiply(a.den, b.den)); }
        public static Frac Sub(Frac a, Frac b) { return new Frac(BigInteger.Subtract(BigInteger.Multiply(a.num, b.den), BigInteger.Multiply(a.den, b.num)), BigInteger.Multiply(a.den, b.den)); }
        public static Frac Mul(Frac a, Frac b) { return new Frac(BigInteger.Multiply(a.num, b.num), BigInteger.Multiply(a.den, b.den)); }
        public static Frac Div(Frac a, Frac b) { return new Frac(BigInteger.Multiply(a.num, b.den), BigInteger.Multiply(a.den, b.num)); }
        public static Frac Inv(Frac a) { return new Frac(a.den, a.num); }

        public Frac Add(Frac b) { num = BigInteger.Add(BigInteger.Multiply(num, b.den), BigInteger.Multiply(den, b.num)); den = BigInteger.Multiply(den, b.den); return Reduce(); }
        public Frac Sub(Frac b) { num = BigInteger.Subtract(BigInteger.Multiply(num, b.den), BigInteger.Multiply(den, b.num)); den = BigInteger.Multiply(den, b.den); return Reduce(); }
        public Frac Mul(Frac b) { num = BigInteger.Multiply(num, b.num); den = BigInteger.Multiply(den, b.den); return Reduce(); }
        public Frac Div(Frac b) { num = BigInteger.Multiply(num, b.den); den = BigInteger.Multiply(den, b.num); return Reduce(); }
        public Frac Inv() { BigInteger d = num; num = den; den = d; return Reduce(); }

        public override string ToString() { return num.ToString() + "/" + den.ToString(); }
        public string ToStringSimple() { return den.Equals(BigInteger.One) ? num.ToString() : num.ToString() + "/" + den.ToString(); }
        public int CompareTo(Frac f) { return BigInteger.Multiply(num, f.den).CompareTo(BigInteger.Multiply(f.num, den)); }

        public bool SimpleEquals(Frac f) { return num.Equals(f.num) && den.Equals(f.den); }

        public override bool Equals(Object o)
        {
            if (o == null) return false;
            Frac f = (Frac)o;
            return BigInteger.Multiply(num, f.den).Equals(BigInteger.Multiply(f.num, den));
        }
        public override int GetHashCode() { return (num.GetHashCode() ^ den.GetHashCode()); }
    }
    // -------------------------------------------------------------------------
}

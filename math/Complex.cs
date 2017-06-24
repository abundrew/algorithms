using System;

namespace algorithms.math
{
    // ----- Complex -----------------------------------------------------------
    // double Re
    // double Im
    // Complex(double re, double im)
    // Complex(double re)
    // double Abs()
    // double Abs2()
    // Complex Copy()
    // double Arg()
    // Complex Conjugate()
    // Complex Unit()
    // int CompareTo(object o)
    // bool Equals(object o)
    // int GetHashCode()
    // string ToString()
    // static Complex Zero
    // static Complex I
    // static Complex MaxValue
    // static Complex MinValue
    // static Complex FromAbsArg(double abs, double arg)
    // static explicit operator Complex(double f)
    // static explicit operator double(Complex c)
    // static bool operator ==(Complex a, Complex b)
    // static bool operator !=(Complex a, Complex b)
    // static Complex operator +(Complex a)
    // static Complex operator -(Complex a)
    // static Complex operator +(Complex a, double f)
    // static Complex operator +(double f, Complex a)
    // static Complex operator +(Complex a, Complex b)
    // static Complex operator -(Complex a, double f)
    // static Complex operator -(double f, Complex a)
    // static Complex operator -(Complex a, Complex b)
    // static Complex operator *(Complex a, double f)
    // static Complex operator *(double f, Complex a)
    // static Complex operator *(Complex a, Complex b)
    // static Complex operator /(Complex a, double f)
    // static Complex operator /(Complex a, Complex b)
    // static bool IsEqual(Complex a, Complex b, double tolerance)
    // -------------------------------------------------------------------------
    public struct Complex: IComparable
    {
        public double Re { get; private set; }
        public double Im { get; private set; }
        public Complex(double re, double im) { Re = re; Im = im; }
        public Complex(double re) { Re = re; Im = 0; }
        public double Abs() { return Math.Sqrt(Re * Re + Im * Im); }
        public double Abs2() { return Re * Re + Im * Im; }
        public Complex Copy() { return new Complex(Re, Im); }
        public double Arg() { return Math.Atan2(Im, Re); }
        public Complex Conjugate() { return new Complex(Re, -Im); }
        public Complex Unit() { return this / Abs(); }
        public int CompareTo(object o) { if (o is Complex) { return Abs().CompareTo(((Complex)o).Abs()); } if (o is double) { return Abs().CompareTo((double)o); } return 0; }
        public override bool Equals(object o) { if (o is Complex) { Complex c = (Complex)o; return (this == c); } return false; }
        public override int GetHashCode() { return (Re.GetHashCode() ^ Im.GetHashCode()); }
        public override string ToString() { return string.Format("({0}, {1})", Re, Im); }
        public static Complex Zero { get { return new Complex(0, 0); } }
        public static Complex I { get { return new Complex(0, 1); } }
        public static Complex MaxValue { get { return new Complex(double.MaxValue, double.MaxValue); } }
        public static Complex MinValue { get { return new Complex(double.MinValue, double.MinValue); } }
        public static Complex FromAbsArg(double abs, double arg) { return new Complex(abs * Math.Cos(arg), abs * Math.Sin(arg)); }
        public static explicit operator Complex(double f) { return new Complex(f, 0); }
        public static explicit operator double(Complex c) { return c.Re; }
        public static bool operator ==(Complex a, Complex b) { return (a.Re == b.Re) && (a.Im == b.Im); }
        public static bool operator !=(Complex a, Complex b) { return (a.Re != b.Re) || (a.Im != b.Im); }
        public static Complex operator +(Complex a) { return new Complex(a.Re, a.Im); }
        public static Complex operator -(Complex a) { return new Complex(-a.Re, -a.Im); }
        public static Complex operator +(Complex a, double f) { return new Complex(a.Re + f, a.Im); }
        public static Complex operator +(double f, Complex a) { return new Complex(a.Re + f, a.Im); }
        public static Complex operator +(Complex a, Complex b) { return new Complex(a.Re + b.Re, a.Im + b.Im); }
        public static Complex operator -(Complex a, double f) { return new Complex(a.Re - f, a.Im); }
        public static Complex operator -(double f, Complex a) { return new Complex(f - a.Re, a.Im); }
        public static Complex operator -(Complex a, Complex b) { return new Complex(a.Re - b.Re, a.Im - b.Im); }
        public static Complex operator *(Complex a, double f) { return new Complex(a.Re * f, a.Im * f); }
        public static Complex operator *(double f, Complex a) { return new Complex(a.Re * f, a.Im * f); }
        public static Complex operator *(Complex a, Complex b) { return new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re); }
        public static Complex operator /(Complex a, double f) { return new Complex(a.Re / f, a.Im / f); }
        public static Complex operator /(Complex a, Complex b) {
            double denom = b.Re * b.Re + b.Im * b.Im;
            return new Complex((a.Re * b.Re + a.Im * b.Im) / denom, (a.Im * b.Re - a.Re * b.Im) / denom);
        }
        public static bool IsEqual(Complex a, Complex b, double tolerance)
        {
            return (Math.Abs(a.Re - b.Re) < tolerance) && (Math.Abs(a.Im - b.Im) < tolerance);
        }
    }
    // -------------------------------------------------------------------------
}


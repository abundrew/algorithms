using System;

namespace algorithms.geometry2D
{
    // ----- P2D ---------------------------------------------------------------
    // double X
    // double Y
    // P2D(double x, double y)
    // P2D(P2D p)
    // double Abs()
    // double Abs2()
    // P2D Copy()
    // override bool Equals(object o)
    // override int GetHashCode()
    // double Angle()
    // P2D Rot()
    // P2D Rot(P2D O, double theta)
    // P2D Unit()
    // static bool operator ==(P2D a, P2D b)
    // static bool operator !=(P2D a, P2D b)
    // static bool operator <(P2D a, P2D b)
    // static bool operator >(P2D a, P2D b)
    // static P2D operator +(P2D a)
    // static P2D operator -(P2D a)
    // static P2D operator +(P2D a, P2D b)
    // static P2D operator -(P2D a, P2D b)
    // static double operator *(P2D a, P2D b) - dot, scalar, inner product
    // static double operator %(P2D a, P2D b) - cross product
    // static P2D operator *(P2D a, double f)
    // static P2D operator *(double f, P2D a)
    // static P2D operator /(P2D a, double f)
    // -------------------------------------------------------------------------
    public struct P2D
    {
        const double eps = 1e-9;
        static bool eq(double a, double b) { return Math.Abs(a - b) < eps; }
        static bool ge(double a, double b) { return a - b > -eps; }
        static bool le(double a, double b) { return b - a > -eps; }
        static bool gt(double a, double b) { return a - b > eps; }
        static bool lt(double a, double b) { return b - a < eps; }
        public double X { get; set; }
        public double Y { get; set; }
        public P2D(double x, double y) { X = x; Y = y; }
        public double Abs() { return Math.Sqrt(X * X + Y * Y); }
        public double Abs2() { return X * X + Y * Y; }
        public P2D Copy() { return new P2D(X, Y); }
        public override bool Equals(object o) { if (o is P2D) { P2D c = (P2D)o; return (this == c); } return false; }
        public override int GetHashCode() { return (X.GetHashCode() ^ Y.GetHashCode()); }
        public double Angle() { return Math.Atan2(Y, X); }
        public P2D Rot() { return new P2D(-Y, X); }
        public P2D Rot(P2D O, double theta) {
            return new P2D(Math.Cos(theta) * (X - O.X) - Math.Sin(theta) * (Y - O.Y) + O.X, Math.Sin(theta) * (X - O.X) + Math.Cos(theta) * (Y - O.Y) + O.Y);
        }
        public P2D Unit() { return this / Abs(); }
        public static bool operator ==(P2D a, P2D b) { return eq(a.X, b.X) && eq(a.Y, b.Y); }
        public static bool operator !=(P2D a, P2D b) { return !(a == b); }
        public static bool operator <(P2D a, P2D b) { if (eq(a.X, b.X)) return lt(a.Y, b.Y); return a.X < b.X; }
        public static bool operator >(P2D a, P2D b) { if (eq(a.X, b.X)) return gt(a.Y, b.Y); return a.X > b.X; }
        public static P2D operator +(P2D a) { return new P2D(a.X, a.Y); }
        public static P2D operator -(P2D a) { return new P2D(-a.X, -a.Y); }
        public static P2D operator +(P2D a, P2D b) { return new P2D(a.X + b.X, a.Y + b.Y); }
        public static P2D operator -(P2D a, P2D b) { return new P2D(a.X - b.X, a.Y - b.Y); }
        public static double operator *(P2D a, P2D b) { return a.X * b.X + a.Y * b.Y; }
        public static double operator %(P2D a, P2D b) { return a.X * b.Y - a.Y * b.X; }
        public static P2D operator *(P2D a, double f) { return new P2D(a.X * f, a.Y * f); }
        public static P2D operator *(double f, P2D a) { return new P2D(a.X * f, a.Y * f); }
        public static P2D operator /(P2D a, double f) { return new P2D(a.X / f, a.Y / f); }
    }
    // -------------------------------------------------------------------------
}

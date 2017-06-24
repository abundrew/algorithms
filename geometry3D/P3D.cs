using System;

namespace algorithms.geometry3D
{
    // ----- P3D ---------------------------------------------------------------
    // double X
    // double Y
    // double Z
    // P3D(double x, double y, double z)
    // double Abs()
    // double Abs2()
    // P3D Copy()
    // override bool Equals(object o)
    // override int GetHashCode()
    // P3D Unit()
    // static bool operator ==(P3D a, P3D b)
    // static bool operator !=(P3D a, P3D b)
    // static bool operator <(P3D a, P3D b)
    // static bool operator >(P3D a, P3D b)
    // static P3D operator +(P3D a)
    // static P3D operator -(P3D a)
    // static P3D operator +(P3D a, P3D b)
    // static P3D operator -(P3D a, P3D b)
    // static double operator *(P3D a, P3D b) - dot, scalar, inner product
    // static P3D operator %(P3D a, P3D b) - cross product
    // static P3D operator *(P3D a, double f)
    // static P3D operator *(double f, P3D a)
    // static P3D operator /(P3D a, double f)
    // ---------------------------------------------------------------------
    public struct P3D
    {
        const double eps = 1e-9;
        static bool eq(double a, double b) { return Math.Abs(a - b) < eps; }
        static bool ge(double a, double b) { return a - b > -eps; }
        static bool le(double a, double b) { return b - a > -eps; }
        static bool gt(double a, double b) { return a - b > eps; }
        static bool lt(double a, double b) { return b - a < eps; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public P3D(double x, double y, double z) { X = x; Y = y; Z = z; }
        public double Abs() { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        public double Abs2() { return X * X + Y * Y + Z * Z; }
        public P3D Copy() { return new P3D(X, Y, Z); }
        public override bool Equals(object o) { if (o is P3D) { P3D c = (P3D)o; return (this == c); } return false; }
        public override int GetHashCode() { return (X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode()); }
        public P3D Unit() { return this / Abs(); }
        public static bool operator ==(P3D a, P3D b) { return eq(a.X, b.X) && eq(a.Y, b.Y) && eq(a.Z, b.Z); }
        public static bool operator !=(P3D a, P3D b) { return !(a == b); }
        public static bool operator <(P3D a, P3D b) { if (eq(a.X, b.X)) { if (eq(a.Y, b.Y)) return lt(a.Z, b.Z); return a.Y < b.Y; } return a.X < b.X; }
        public static bool operator >(P3D a, P3D b) { if (eq(a.X, b.X)) { if (eq(a.Y, b.Y)) return gt(a.Z, b.Z); return a.Y > b.Y; } return a.X > b.X; }
        public static P3D operator +(P3D a) { return new P3D(a.X, a.Y, a.Z); }
        public static P3D operator -(P3D a) { return new P3D(-a.X, -a.Y, -a.Z); }
        public static P3D operator +(P3D a, P3D b) { return new P3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
        public static P3D operator -(P3D a, P3D b) { return new P3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
        public static double operator *(P3D a, P3D b) { return a.X * b.X + a.Y * b.Y + a.Z * b.Z; }
        public static P3D operator %(P3D a, P3D b) { return new P3D(a.Y * b.Z - b.Y * a.Z, (a.X * b.Z - b.X * a.Z) * -1, a.X * b.Y - b.X * a.Y); }
        public static P3D operator *(P3D a, double f) { return new P3D(a.X * f, a.X * f, a.Z * f); }
        public static P3D operator *(double f, P3D a) { return new P3D(a.X * f, a.X * f, a.Z * f); }
        public static P3D operator /(P3D a, double f) { return new P3D(a.X / f, a.Y / f, a.Z / f); }
    }
    // -------------------------------------------------------------------------
}

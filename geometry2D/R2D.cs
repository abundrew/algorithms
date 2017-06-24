using System;

namespace algorithms.geometry2D
{
    // ----- R2D ---------------------------------------------------------------
    //
    // Depends on:
    // -- P2D (algorithms.geometry2D)
    //
    // P2D A
    // P2D AB
    // P2D AD
    // R2D(double xa, double ya, double xb, double yb, double xd, double yd)
    // R2D(P2D a, P2D b, P2D d)
    // R2D Copy()
    // bool Inside(P2D p)
    // double Area()
    // -------------------------------------------------------------------------
    public struct R2D
    {
        const double eps = 1e-9;
        static bool eq(double a, double b) { return Math.Abs(a - b) < eps; }
        static bool ge(double a, double b) { return a - b > -eps; }
        static bool le(double a, double b) { return b - a > -eps; }
        static bool gt(double a, double b) { return a - b > eps; }
        static bool lt(double a, double b) { return b - a < eps; }
        public P2D A { get; set; }
        public P2D AB { get; set; }
        public P2D AD { get; set; }
        public R2D(double xa, double ya, double xb, double yb, double xd, double yd) { A = new P2D(xa, ya); AB = new P2D(xb, yb) - A; AD = new P2D(xd, yd) - A; }
        public R2D(P2D a, P2D b, P2D d) { A = a; AB = b - a; AD = d - a; }
        public R2D Copy() { return new R2D(A.Copy(), AB.Copy(), AD.Copy()); }
        public bool Inside(P2D p)
        {
            P2D ap = p - A;
            double apab = ap * AB;
            double apad = ap * AD;
            return le(0, apab) && le(apab, AB * AB) && le(0, apad) && le(apad, AD * AD);
        }
        public double Area() { return AB.Abs() * AD.Abs(); }
    }
    // -------------------------------------------------------------------------
}

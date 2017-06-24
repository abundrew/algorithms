using System;

namespace algorithms.geometry2D
{
    // ----- C2D ---------------------------------------------------------------
    //
    // Depends on:
    // -- P2D (algorithms.geometry2D)
    //
    // P2D C
    // double R
    // C2D(double x, double y, double r)
    // C2D(P2D c, double r)
    // C2D Copy()
    // bool Inside(P2D p)
    // double Area()
    // P2D[] ToPolygon(int n)
    // -------------------------------------------------------------------------
    public struct C2D
    {
        const double eps = 1e-9;
        static bool eq(double a, double b) { return Math.Abs(a - b) < eps; }
        static bool ge(double a, double b) { return a - b > -eps; }
        static bool le(double a, double b) { return b - a > -eps; }
        static bool gt(double a, double b) { return a - b > eps; }
        static bool lt(double a, double b) { return b - a < eps; }
        public P2D C { get; set; }
        public double R { get; set; }
        public C2D(double x, double y, double r) { C = new P2D(x, y); R = r; }
        public C2D(P2D c, double r) { C = c; R = r; }
        public C2D Copy() { return new C2D(C.Copy(), R); }
        public bool Inside(P2D p) { return le((p - C).Abs(), R); }
        public double Area() { return Math.PI * R * R; }
        public P2D[] ToPolygon(int n)
        {
            P2D[] pgon = new P2D[n];
            for (int i = 0; i < n; i++)
                pgon[i] = new P2D(C.X + Math.Cos(Math.PI * 2 * i / n), C.Y + Math.Sin(Math.PI * 2 * i / n));
            return pgon;
        }
    }
    // -------------------------------------------------------------------------
}

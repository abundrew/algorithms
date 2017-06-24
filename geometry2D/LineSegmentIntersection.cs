using System;

namespace algorithms.geometry2D
{
    public static class LineSegmentIntersection
    {
        // ----- Line Segment Intersection -------------------------------------
        //
        // -- a relative orientation of points [p1], [p2], [p3]
        //
        // -- 0: colinear
        // -- 1: clock wise
        // -- 2: counterclock wise
        //
        // int Orientation(P2D p1, P2D p2, P2D p3)
        //
        // -- do line segments [a1, b1] and [a2, b2] intersect
        //
        // bool Intersect(P2D a1, P2D b1, P2D a2, P2D b2)
        // ---------------------------------------------------------------------
        static int Orientation(P2D p1, P2D p2, P2D p3)
        {
            double val = (p2.Y - p1.Y) * (p3.X - p2.X) - (p2.X - p1.X) * (p3.Y - p2.Y);
            if (val == 0) return 0;
            return (val > 0) ? 1 : 2;
        }
        public static bool Intersect(P2D a1, P2D b1, P2D a2, P2D b2)
        {
            int o1 = Orientation(a1, b1, a2);
            int o2 = Orientation(a1, b1, b2);
            int o3 = Orientation(a2, b2, a1);
            int o4 = Orientation(a2, b2, b1);
            if (o1 != o2 && o3 != o4) return true;
            if (o1 == 0 && a2.X <= Math.Max(a1.X, b1.X) && a2.X >= Math.Min(a1.X, b1.X) && a2.Y <= Math.Max(a1.Y, b1.Y) && a2.Y >= Math.Min(a1.Y, b1.Y)) return true;
            if (o2 == 0 && b2.X <= Math.Max(a1.X, b1.X) && b2.X >= Math.Min(a1.X, b1.X) && b2.Y <= Math.Max(a1.Y, b1.Y) && b2.Y >= Math.Min(a1.Y, b1.Y)) return true;
            if (o3 == 0 && a1.X <= Math.Max(a2.X, b2.X) && a1.X >= Math.Min(a2.X, b2.X) && a1.Y <= Math.Max(a2.Y, b2.Y) && a1.Y >= Math.Min(a2.Y, b2.Y)) return true;
            if (o4 == 0 && b1.X <= Math.Max(a2.X, b2.X) && b1.X >= Math.Min(a2.X, b2.X) && b1.Y <= Math.Max(a2.Y, b2.Y) && b1.Y >= Math.Min(a2.Y, b2.Y)) return true;
            return false;
        }
        // ---------------------------------------------------------------------
    }
}

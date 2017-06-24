using System.Collections.Generic;

namespace algorithms.geometry2D
{
    public static class ConvexHull
    {
        // ----- Convex Hull ---------------------------------------------------
        //
        // -- find a convex hull (envelop) of a set of points [points]
        //
        // IEnumerable<P2D> GetConvexHull(IEnumerable<P2D> points)
        // ---------------------------------------------------------------------
        public static IEnumerable<P2D> GetConvexHull(IEnumerable<P2D> points)
        {
            List<P2D> plist = new List<P2D>(points);
            plist.Sort((a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : (a.X > b.X ? 1 : -1));
            List<P2D> hull = new List<P2D>();
            int L = 0, U = 0;
            for (int i = plist.Count - 1; i >= 0; i--)
            {
                P2D p = plist[i], p1;
                while (L >= 2 && ((p1 = hull[hull.Count - 1]) - hull[hull.Count - 2]) % (p - p1) >= 0)
                {
                    hull.RemoveAt(hull.Count - 1);
                    L--;
                }
                hull.Add(p);
                L++;
                while (U >= 2 && ((p1 = hull[0]) - hull[1]) % (p - p1) <= 0)
                {
                    hull.RemoveAt(0);
                    U--;
                }
                if (U != 0) hull.Insert(0, p);
                U++;
            }
            hull.RemoveAt(hull.Count - 1);
            return hull;
        }
        // ---------------------------------------------------------------------
    }
}

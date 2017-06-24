using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.geometry2D
{
    public static class SutherlandHodgman
    {
        // ----- Sutherland Hodgman --------------------------------------------
        //
        // Depends on:
        // -- P2D (algorithms.geometry2D)
        //
        // P2D[] GetIntersectedPolygon(P2D[] subjectPoly, P2D[] clipPoly)
        // ---------------------------------------------------------------------
        private class Edge
        {
            public readonly P2D From;
            public readonly P2D To;
            public Edge(P2D from, P2D to)
            {
                From = from;
                To = to;
            }
        }
        // This clips the subject polygon against the clip polygon (gets the intersection of the two polygons)
        public static P2D[] GetIntersectedPolygon(P2D[] subjectPoly, P2D[] clipPoly)
        {
            List<P2D> outputList = subjectPoly.ToList();
            //	Make sure it's clockwise
            bool? cw = IsClockwise(subjectPoly);
            if (cw == false) outputList.Reverse();
            //	Walk around the clip polygon clockwise
            foreach (Edge clipEdge in IterateEdgesClockwise(clipPoly))
            {
                // clone it
                List<P2D> inputList = outputList.ToList();
                outputList.Clear();
                if (inputList.Count == 0)
                {
                    // Sometimes when the polygons don't intersect, this list goes to zero.  Jump out to avoid an index out of range exception
                    break;
                }
                P2D S = inputList[inputList.Count - 1];
                foreach (P2D E in inputList)
                {
                    if (IsInside(clipEdge, E))
                    {
                        if (!IsInside(clipEdge, S))
                        {
                            P2D point = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                            if (point.X == double.MaxValue)
                            {
                                // may be colinear, or may be a bug
                                throw new ApplicationException("Line segments don't intersect");
                            }
                            else outputList.Add(point);
                        }
                        outputList.Add(E);
                    }
                    else if (IsInside(clipEdge, S))
                    {
                        P2D point = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                        if (point.X == double.MaxValue)
                        {
                            // may be colinear, or may be a bug
                            throw new ApplicationException("Line segments don't intersect");
                        }
                        else outputList.Add(point);
                    }
                    S = E;
                }
            }
            int ix = 0;
            while (ix < outputList.Count - 1)
            {
                if (outputList[ix].X == outputList[ix + 1].X && outputList[ix].Y == outputList[ix + 1].Y) outputList.RemoveAt(ix + 1); else ix++;
            }
            return outputList.ToArray();
        }
        // This iterates through the edges of the polygon, always clockwise
        private static IEnumerable<Edge> IterateEdgesClockwise(P2D[] polygon)
        {
            bool? cw = IsClockwise(polygon);
            if (cw == null || cw == true)
            {
                for (int cntr = 0; cntr < polygon.Length - 1; cntr++)
                    yield return new Edge(polygon[cntr], polygon[cntr + 1]);
                yield return new Edge(polygon[polygon.Length - 1], polygon[0]);
            }
            else
            {
                for (int cntr = polygon.Length - 1; cntr > 0; cntr--)
                    yield return new Edge(polygon[cntr], polygon[cntr - 1]);
                yield return new Edge(polygon[0], polygon[polygon.Length - 1]);
            }
        }
        // Returns the intersection of the two lines (line segments are passed in, but they are treated like infinite lines)
        private static P2D GetIntersect(P2D line1From, P2D line1To, P2D line2From, P2D line2To)
        {
            P2D direction1 = line1To - line1From;
            P2D direction2 = line2To - line2From;
            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);
            // If it's 0, it means the lines are parallel so have infinite intersection points
            if (IsNearZero(dotPerp)) return new P2D(double.MaxValue, double.MaxValue);
            P2D c = line2From - line1From;
            double t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;
            //	Return the intersection point
            return line1From + (direction1 * t);
        }
        private static bool IsInside(Edge edge, P2D test)
        {
            bool? isLeft = IsLeftOf(edge, test);
            if (isLeft == null) return true;
            return !isLeft.Value;
        }
        private static bool? IsClockwise(P2D[] polygon)
        {
            for (int cntr = 2; cntr < polygon.Length; cntr++)
            {
                bool? isLeft = IsLeftOf(new Edge(polygon[0], polygon[1]), polygon[cntr]);
                // some of the points may be colinear.  That's ok as long as the overall is a polygon
                if (isLeft != null)
                {
                    return !isLeft.Value;
                }
            }
            // All the points in the polygon are colinear
            return null;
        }
        // Tells if the test point lies on the left side of the edge line
        private static bool? IsLeftOf(Edge edge, P2D test)
        {
            P2D tmp1 = edge.To - edge.From;
            P2D tmp2 = test - edge.To;
            //dot product of perpendicular?
            double x = (tmp1.X * tmp2.Y) - (tmp1.Y * tmp2.X);
            if (x < 0) return false;
            else
                if (x > 0) return true;
            else
                //Colinear points;
                return null;
        }
        private static bool IsNearZero(double testValue)
        {
            return Math.Abs(testValue) <= .000000001d;
        }
        // ---------------------------------------------------------------------
    }
}

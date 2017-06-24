using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.geometry2D
{
    public static class Polygon
    {
        // ----- Polygon -------------------------------------------------------
        //
        // Depends on:
        // -- P2D (algorithms.geometry2D)
        //
        // double SimplePolygonArea(IEnumerable<P2D> polygon)
        // ---------------------------------------------------------------------
        public static double SimplePolygonArea(IEnumerable<P2D> polygon)
        {
            double area = 0.0;
            P2D[] vertices = polygon.ToArray();
            int j = vertices.Length - 1;
            for (int i = 0; i < vertices.Length; i++)
            {
                area += (vertices[j].X + vertices[i].X) * (vertices[j].Y - vertices[i].Y);
                j = i;
            }
            return Math.Abs(area / 2.0);
        }
        // ---------------------------------------------------------------------
    }
}

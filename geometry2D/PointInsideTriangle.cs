namespace algorithms.geometry2D
{
    public static class PointInsideTriangle
    {
        // ----- Point Inside Triangle -----------------------------------------
        //
        // -- is a point [p0] inside a triangle [p1, p2, p3]
        //
        // bool IsPointInsideTriangle(P2D p0, P2D p1, P2D p2, P2D p3)
        // ---------------------------------------------------------------------
        public static bool IsPointInsideTriangle(P2D p0, P2D p1, P2D p2, P2D p3)
        {
            double area2 = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            double alpha = ((p2.Y - p3.Y) * (p0.X - p3.X) + (p3.X - p2.X) * (p0.Y - p3.Y)) / area2;
            double beta = ((p3.Y - p1.Y) * (p0.X - p3.X) + (p1.X - p3.X) * (p0.Y - p3.Y)) / area2;
            double gamma = 1.0 - alpha - beta;
            return alpha > 0 && beta > 0 && gamma > 0;
        }
        // ---------------------------------------------------------------------
    }
}

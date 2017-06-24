using System;

namespace algorithms.geometry3D
{
    public static class DistanceFromPointToTriangle
    {
        // ----- Distance From Point To Triangle -------------------------------
        //
        // -- distance from a point [p0] to a triangle [p1, p2, p3] in 3D
        //
        // double GetDistanceFromPointToTriangle(P3D p0, P3D p1, P3D p2, P3D p3)
        // ---------------------------------------------------------------------
        public static double GetDistanceFromPointToTriangle(P3D p0, P3D p1, P3D p2, P3D p3)
        {
            P3D B = p1.Copy();
            P3D E0 = B - p2;
            P3D E1 = B - p3;

            P3D D = p0 - B;
            double a = E0 * E0;
            double b = E0 * E1;
            double c = E1 * E1;
            double d = E0 * D;
            double e = E1 * D;
            double f = D * D;

            double det = a * c - b * b;
            double s = b * e - c * d;
            double t = b * d - a * e;

            double sqrDistance = 0;

            if ((s + t) <= det)
            {
                if (s < 0)
                {
                    if (t < 0)
                    {
                        // region4
                        if (d < 0)
                        {
                            t = 0;
                            if (-d >= a)
                            {
                                s = 1;
                                sqrDistance = a + 2 * d + f;
                            }
                            else
                            {
                                s = -d / a;
                                sqrDistance = d * s + f;
                            }
                        }
                        else
                        {
                            s = 0;
                            if (e >= 0)
                            {
                                t = 0;
                                sqrDistance = f;
                            }
                            else
                            {
                                if (-e >= c)
                                {
                                    t = 1;
                                    sqrDistance = c + 2 * e + f;
                                }
                                else
                                {
                                    t = -e / c;
                                    sqrDistance = e * t + f;
                                }
                            }
                        }
                        // region 4
                    }
                    else
                    {
                        // region 3
                        s = 0;
                        if (e >= 0)
                        {
                            t = 0;
                            sqrDistance = f;
                        }
                        else
                        {
                            if (-e >= c)
                            {
                                t = 1;
                                sqrDistance = c + 2 * e + f;
                            }
                            else
                            {
                                t = -e / c;
                                sqrDistance = e * t + f;
                            }
                        }
                        // region 3
                    }
                }
                else
                {
                    if (t < 0)
                    {
                        // region 5
                        t = 0;
                        if (d >= 0)
                        {
                            s = 0;
                            sqrDistance = f;
                        }
                        else
                        {
                            if (-d >= a)
                            {
                                s = 1;
                                sqrDistance = a + 2 * d + f;
                            }
                            else
                            {
                                s = -d / a;
                                sqrDistance = d * s + f;
                            }
                        }
                        // region 5
                    }
                    else
                    {
                        // region 0
                        double invDet = 1 / det;
                        s = s * invDet;
                        t = t * invDet;
                        sqrDistance = s * (a * s + b * t + 2 * d) + t * (b * s + c * t + 2 * e) + f;
                        // region 0
                    }
                }
            }
            else
            {
                if (s < 0)
                {
                    // region 2
                    double tmp0 = b + d;
                    double tmp1 = c + e;
                    if (tmp1 > tmp0)
                    {
                        double numer = tmp1 - tmp0;
                        double denom = a - 2 * b + c;
                        if (numer >= denom)
                        {
                            s = 1;
                            t = 0;
                            sqrDistance = a + 2 * d + f;
                        }
                        else
                        {
                            s = numer / denom;
                            t = 1 - s;
                            sqrDistance = s * (a * s + b * t + 2 * d) + t * (b * s + c * t + 2 * e) + f;
                        }
                    }
                    else
                    {
                        s = 0;
                        if (tmp1 <= 0)
                        {
                            t = 1;
                            sqrDistance = c + 2 * e + f;
                        }
                        else
                        {
                            if (e >= 0)
                            {
                                t = 0;
                                sqrDistance = f;
                            }
                            else
                            {
                                t = -e / c;
                                sqrDistance = e * t + f;
                            }
                        }
                    }
                    // region 2
                }
                else
                {
                    if (t < 0)
                    {
                        // region6
                        double tmp0 = b + e;
                        double tmp1 = a + d;
                        if (tmp1 > tmp0)
                        {
                            double numer = tmp1 - tmp0;
                            double denom = a - 2 * b + c;
                            if (numer >= denom)
                            {
                                t = 1;
                                s = 0;
                                sqrDistance = c + 2 * e + f;
                            }
                            else
                            {
                                t = numer / denom;
                                s = 1 - t;
                                sqrDistance = s * (a * s + b * t + 2 * d) + t * (b * s + c * t + 2 * e) + f;
                            }
                        }
                        else
                        {
                            t = 0;
                            if (tmp1 <= 0)
                            {
                                s = 1;
                                sqrDistance = a + 2 * d + f;
                            }
                            else
                            {
                                if (d >= 0)
                                {
                                    s = 0;
                                    sqrDistance = f;
                                }
                                else
                                {
                                    s = -d / a;
                                    sqrDistance = d * s + f;
                                }
                            }
                        }
                        // region 6
                    }
                    else
                    {
                        // region 1
                        double numer = c + e - b - d;
                        if (numer <= 0)
                        {
                            s = 0;
                            t = 1;
                            sqrDistance = c + 2 * e + f;
                        }
                        else
                        {
                            double denom = a - 2 * b + c;
                            if (numer >= denom)
                            {
                                s = 1;
                                t = 0;
                                sqrDistance = a + 2 * d + f;
                            }
                            else
                            {
                                s = numer / denom;
                                t = 1 - s;
                                sqrDistance = s * (a * s + b * t + 2 * d) + t * (b * s + c * t + 2 * e) + f;
                            }
                        }
                        // region 1
                    }
                }
            }

            if (sqrDistance < 0) sqrDistance = 0;

            double dist = Math.Sqrt(sqrDistance);

            //if nargout > 1
            //  PP0 = B + s * E0 + t * E1;
            //end

            return dist;
        }
        // ---------------------------------------------------------------------
    }
}

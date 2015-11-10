using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

namespace o3DLib.Helpers
{
    public static class AnalyticGeometryHelper
    {
        public static Point GetCircleCenterBy3Points(Point p1,Point p2,Point p3)
        {
            double ma = (p2.Y - p1.Y) / (p2.X - p1.X);
            double mb = (p3.Y - p2.Y) / (p3.X - p2.X);
            double x = (ma * mb * (p1.Y - p3.Y) + mb * (p1.X + p2.X) - ma * (p2.X + p3.X)) / 2 * (mb - ma);
            double y = -1 / ma * (x - (p1.X + p2.X) / 2) + (p1.Y + p2.Y) / 2;
            return new Point(x, y);
        }

        // Code adapted from Paul Bourke:
        // http://local.wasp.uwa.edu.au/~pbourke/geometry/sphereline/raysphere.c
        public static bool GetCircleLineIntersect(double x1, double y1, double x2, double y2, double cx, double cy, double cr)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double a = dx * dx + dy * dy;
            double b = 2 * (dx * (x1 - cx) + dy * (y1 - cy));
            double c = cx * cx + cy * cy;
            c += x1 * x1 + y1 * y1;
            c -= 2 * (cx * x1 + cy * y1);
            c -= cr * cr;
            double bb4ac = b * b - 4 * a * c;

            //println(bb4ac);

            if (bb4ac < 0)
            {  // Not intersecting
                return false;
            }
            else
            {

                double mu = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double ix1 = x1 + mu * (dx);
                double iy1 = y1 + mu * (dy);
                mu = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double ix2 = x1 + mu * (dx);
                double iy2 = y1 + mu * (dy);

                // The intersection points
                //ellipse(ix1, iy1, 10, 10);
                //ellipse(ix2, iy2, 10, 10);

                double testX;
                double testY;

                // Figure out which point is closer to the circle
                if (dist(x1, y1, cx, cy) < dist(x2, y2, cx, cy))
                {
                    testX = x2;
                    testY = y2;
                }
                else
                {
                    testX = x1;
                    testY = y1;
                }

                if (dist(testX, testY, ix1, iy1) < dist(x1, y1, x2, y2) || dist(testX, testY, ix2, iy2) < dist(x1, y1, x2, y2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static double dist(double x1,double y1, double x2, double y2)
        {
            return (new Point(x2, y2) - new Point(x1, y1)).Length;
        }

        // Find the points where the two circles intersect.
        public static int GetCircleCircleIntersections(
            double cx0, double cy0, double radius0,
            double cx1, double cy1, double radius1,
            out Point intersection1, out Point intersection2)
        {
            // Find the distance between the centers.
            double dx = cx0 - cx1;
            double dy = cy0 - cy1;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            // See how many solutions there are.
            if (dist > radius0 + radius1)
            {
                // No solutions, the circles are too far apart.
                intersection1 = new Point(double.NaN, double.NaN);
                intersection2 = new Point(double.NaN, double.NaN);
                return 0;
            }
            else if (dist < Math.Abs(radius0 - radius1))
            {
                // No solutions, one circle contains the other.
                intersection1 = new Point(double.NaN, double.NaN);
                intersection2 = new Point(double.NaN, double.NaN);
                return 0;
            }
            else if ((dist == 0) && (radius0 == radius1))
            {
                // No solutions, the circles coincide.
                intersection1 = new Point(double.NaN, double.NaN);
                intersection2 = new Point(double.NaN, double.NaN);
                return 0;
            }
            else
            {
                // Find a and h.
                double a = (radius0 * radius0 -
                    radius1 * radius1 + dist * dist) / (2 * dist);
                double h = Math.Sqrt(radius0 * radius0 - a * a);

                // Find P2.
                double cx2 = cx0 + a * (cx1 - cx0) / dist;
                double cy2 = cy0 + a * (cy1 - cy0) / dist;

                // Get the points P3.
                intersection1 = new Point(
                    (double)(cx2 + h * (cy1 - cy0) / dist),
                    (double)(cy2 - h * (cx1 - cx0) / dist));
                intersection2 = new Point(
                    (double)(cx2 - h * (cy1 - cy0) / dist),
                    (double)(cy2 + h * (cx1 - cx0) / dist));

                // See if we have 1 or 2 solutions.
                if (dist == radius0 + radius1) return 1;
                return 2;
            }
        }
    }
}

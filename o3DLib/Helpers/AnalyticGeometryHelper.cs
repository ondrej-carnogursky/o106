using o3DLib.Extensions;
using o3DLib.Sketching;
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
        public static bool DoesCircleLineIntersect(double x1, double y1, double x2, double y2, double cx, double cy, double cr)
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


        public static IList<Point> GetCircleFullLineIntersection(Circle2D circle, FullLine2D line)
        {
            IList<Point> list = new List<Point>();

            double a = Math.Pow(line.Vector.X, 2) + Math.Pow(line.Vector.Y, 2);
            double b = 2 * (line.Vector.X * (line.Point.X - circle.Center.X)
                + line.Vector.Y * (line.Point.Y - circle.Center.Y));

            double c = Math.Pow(circle.Center.X, 2) + Math.Pow(circle.Center.Y, 2);
            c += Math.Pow(line.Point.X, 2) + Math.Pow(line.Point.Y, 2);
            c -= 2 * (circle.Center.X * line.Point.X + circle.Center.Y * line.Point.Y);
            c -= Math.Pow(circle.Radius, 2);
            double bb4ac = b * b - 4 * a * c;

            if (bb4ac < 0)
                return list;
            else if (bb4ac == 0)
            {
                double mu = -b / (2 * a);
                Point intersectionPoint = new Point(line.Point.X + mu * line.Vector.X,
                                                    line.Point.Y + mu * line.Vector.Y);
                list.Add(intersectionPoint);
            }
            else
            {

                double mu = -b + Math.Sqrt(bb4ac) / (2 * a);
                Point intersectionPoint1 = new Point(line.Point.X + mu * line.Vector.X,
                                                    line.Point.Y + mu * line.Vector.Y);
                mu = -b - Math.Sqrt(bb4ac) / (2 * a);
                Point intersectionPoint2 = new Point(line.Point.X + mu * line.Vector.X,
                                                    line.Point.Y + mu * line.Vector.Y);

                list.Add(intersectionPoint1);
                list.Add(intersectionPoint2);
            }

            return list;
        }


        public static IList<Point> GetLineFullLineIntersection(Line2D line1, FullLine2D line2)
        {
            IList<Point> list = new List<Point>();

            Point2D startPoint = line1.GetKeyPoint(KeyPointType.Start);
            Point2D endPoint = line1.GetKeyPoint(KeyPointType.End);

            FullLine2D fullLine = new FullLine2D(startPoint, endPoint);

            IList<Point> intersect = line2.Intersection(fullLine);
            if (intersect.Count > 0 && (
                                       intersect[0].X > Math.Min(startPoint.X, endPoint.X)
                                    && intersect[0].X < Math.Max(startPoint.X, endPoint.X)
                                    && intersect[0].Y > Math.Min(startPoint.Y, endPoint.Y)
                                    && intersect[0].Y < Math.Max(startPoint.Y, endPoint.Y)
                                 )
                )
            {
                list.Add(intersect[0]);
            }

            return list;
        }

        public static IList<Point> GetFullLinePointIntersection(FullLine2D line, Point2D point)
        {
            IList<Point> list = new List<Point>();

            double c = -(line.Vector.X * line.Point.X) - (line.Vector.Y * line.Point.Y);

            if ((line.Vector.X * point.X + line.Vector.Y * point.Y + c).IsEqual(0))
            {
                list.Add(new Point(point.X, point.Y));
            }

            return list;
        }


        public static IList<Point> GetCirclePointIntersection(Circle2D circle, Point2D point)
        {
            IList<Point> list = new List<Point>();

            if((Math.Pow((point.X - circle.Center.X), 2) + Math.Pow((point.Y - circle.Center.Y), 2)).IsEqual(circle.Radius))
            {
                list.Add(new Point(point.X, point.Y));
            }

            return list;
        }

        public static IList<Point> GetCircleLineIntersection(Circle2D circle, Line2D line)
        {
            IList<Point> list = new List<Point>();

            Point2D startPoint = line.GetKeyPoint(KeyPointType.Start);
            Point2D endPoint = line.GetKeyPoint(KeyPointType.End);


            IList<Point> fullLineIntersections = circle.Intersection(
                new FullLine2D(startPoint, endPoint)
            );


            foreach (Point p in fullLineIntersections)
            {
                if (p.X < Math.Max(startPoint.X, endPoint.X)
                    && p.X > Math.Min(startPoint.X, endPoint.X)
                    && p.Y < Math.Max(startPoint.Y, endPoint.Y)
                    && p.Y > Math.Min(startPoint.Y, endPoint.Y))
                {
                    list.Add(p);
                }
            }

            return list;
        }

    public static Point get_line_intersection(o3DLib.Sketching.FullLine2D l,Point p,Vector v)
        {
            double p0_x = l.Point.X;
            double p0_y = l.Point.Y;
            double p1_x = p0_x + l.Vector.X;
            double p1_y = p0_x + l.Vector.X;
            double p2_x = p.X;
            double p2_y = p.Y;
            double p3_x = p2_x + v.X;
            double p3_y = p2_y + v.Y;
            double s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            double s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            //if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            //{
            //   // Collision detected
            //    if (i_x != NULL)
                    var i_x = p0_x + (t * s1_x);
            //   if (i_y != NULL)
                    var i_y = p0_y + (t * s1_y);
            return new Point(i_x, i_y);
            //    return 1;
            //}

            //return 0; // No collision
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

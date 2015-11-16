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
        public static Point GetCircleCenterBy3Points(Point p1, Point p2, Point p3)
        {
            double ma = (p2.Y - p1.Y) / (p2.X - p1.X);
            double mb = (p3.Y - p2.Y) / (p3.X - p2.X);
            double x = (ma * mb * (p1.Y - p3.Y) + mb * (p1.X + p2.X) - ma * (p2.X + p3.X)) / 2 * (mb - ma);
            double y = -1 / ma * (x - (p1.X + p2.X) / 2) + (p1.Y + p2.Y) / 2;
            return new Point(x, y);
        }

        public static Point? GetNearest(this IIntersectable i,Point p)
        {
            dynamic ii = i;
            return GetNearest(ii,p);
        }


        public static Point? GetNearest(this IList<Point> points, Point p)
        {
            if (points.Count() > 0)
            {
                Point minPoint = points.First();
                double minDist = GetDistance(p, minPoint);
                for (int i = 1; i < points.Count() - 2; i++)
                {
                    double dist = GetDistance(p, points[i]);
                    if (dist < minDist)
                    {
                        minPoint = points[i];
                        minDist = dist;
                    }
                }

                return minPoint;
            }

            return null;
        }

        public static Point? GetNearest(IRay i, Point p)
        {
            var ps = GetIntersections(i, new Ray2D(p, p + i.Vector.GetNormal()));
            return ps.Count == 1 ? new Point?(ps[0]) : null;
        }


        public static Point? GetNearest(Point2D p1, Point p2)
        {
            return p1.Point;
        }

        public static Vector GetNormal(this Vector v)
        {
            return new Vector(v.Y,-v.X);
        }

        public static IList<Point> GetIntersections(IIntersectable e1, IIntersectable e2)
        {
            //todo Preco sa nemozu volat rovnako GetIntersections a GetIntersects??
            dynamic o1 = e1;
            dynamic o2 = e2;

            try {
                return GetIntersects(o1, o2);
            } catch(Exception e)
            {
                return GetIntersects(o2, o1);
            }

        }

        public static IList<Point> GetIntersects(IRay line1, IRay line2)
        {
            IList<Point> list = new List<Point>();


            Point p1 = new Point(line1.Point.X, line1.Point.Y);
            Point p2 = new Point(line1.Point.X + line1.Vector.X, line1.Point.Y + line1.Vector.Y);

            Point q1 = new Point(line2.Point.X, line2.Point.Y);
            Point q2 = new Point(line2.Point.X + line2.Vector.X, line2.Point.Y + line2.Vector.Y);

            double x21 = p2.X - p1.X;
            double y21 = p2.Y - p1.Y;
            double x31 = q1.X - p1.X;
            double y31 = q1.Y - p1.Y;
            double x43 = q2.X - q1.X;
            double y43 = q2.Y - q1.Y;

            double paramDenominator = x43 * y21 - x21 * y43;

            double s = (x43 * y31 - x31 * y43) / paramDenominator;

            double x = p1.X + (p2.X - p1.X) * s;
            double y = p1.Y + (p2.Y - p1.Y) * s;


            list.Add(new Point(x, y));

            return list;
        }

        public static IList<Point> GetIntersects(ICircle circle, IRay line)
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

        public static IList<Point> GetIntersects(ISegment line1, IRay line2)
        {
            IList<Point> list = new List<Point>();

            Ray2D Ray = new Ray2D(line1.StartPoint, line1.EndPoint);

            IList<Point> intersect = line2.Intersection(Ray);
            if (intersect.Count > 0 && (
                                       intersect[0].X > Math.Min(line1.StartPoint.X, line1.EndPoint.X)
                                    && intersect[0].X < Math.Max(line1.StartPoint.X, line1.EndPoint.X)
                                    && intersect[0].Y > Math.Min(line1.StartPoint.Y, line1.EndPoint.Y)
                                    && intersect[0].Y < Math.Max(line1.StartPoint.Y, line1.EndPoint.Y)
                                 )
                )
            {
                list.Add(intersect[0]);
            }

            return list;
        }

        public static IList<Point> GetIntersects(IRay line, IPoint point)
        {
            IList<Point> list = new List<Point>();

            double c = -(line.Vector.X * line.Point.X) - (line.Vector.Y * line.Point.Y);

            if ((line.Vector.X * point.X + line.Vector.Y * point.Y + c).IsEqual(0))
            {
                list.Add(new Point(point.X, point.Y));
            }

            return list;
        }


        public static IList<Point> GetIntersects(ICircle circle, IPoint point)
        {
            IList<Point> list = new List<Point>();

            if ((Math.Pow((point.X - circle.Center.X), 2) + Math.Pow((point.Y - circle.Center.Y), 2)).IsEqual(circle.Radius))
            {
                list.Add(new Point(point.X, point.Y));
            }

            return list;
        }


        public static IList<Point> GetIntersects(ICircle circle, ISegment line)
        {
            IList<Point> list = new List<Point>();

            IPoint startPoint = line.GetKeyPoint(KeyPointType.Start);
            IPoint endPoint = line.GetKeyPoint(KeyPointType.End);


            IList<Point> RayIntersections = circle.Intersection(
                new Ray2D(new Point(startPoint.X, startPoint.Y), new Point(endPoint.X, endPoint.Y))
            );


            foreach (Point p in RayIntersections)
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

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

    }
}

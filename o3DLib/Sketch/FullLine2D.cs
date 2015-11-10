using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace o3DLib.Sketching
{
    public class FullLine2D
    {
        //todo Rename FullLine2D to Ray2D
        public Point Point { get; set; }

        public Vector Vector { get; set; }
        

        public FullLine2D(Point point, Vector vector)
        {
            this.Point = point;
            this.Vector = vector;
        }

        public FullLine2D(Point2D point, Vector vector)
        {
            this.Point = new Point(point.X, point.Y);
            this.Vector = vector;
        }

        public FullLine2D(Point point, double angle)
        {
            this.Point = point;
            this.Vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public FullLine2D(Point2D point, double angle)
        {
            this.Point = new Point(point.X, point.Y);
            this.Vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public FullLine2D(Point2D point1, Point2D point2)
        {
            this.Point = new Point(point1.X, point1.Y);
            this.Vector = new Vector(point2.X - point1.X, point2.Y - point1.Y);
        }

        public FullLine2D(Point point1, Point point2)
        {
            this.Point = point1;
            this.Vector = new Vector(point2.X - point1.X, point2.Y - point1.Y);
        }

        public IList<Point> Intersection(FullLine2D line)
        {

            IList<Point> list = new List<Point>();

            double denominator = (this.Vector.X * line.Vector.X - this.Vector.Y * line.Vector.Y);

            if (denominator != (double)0)
            {
                double t1 = ((this.Point.X - line.Point.X) * line.Vector.Y + (line.Point.Y - this.Point.Y) * line.Vector.Y) / denominator;

                Point p = new Point(this.Point.X + this.Vector.X * t1, this.Point.Y + this.Vector.Y * t1);
                list.Add(p);

            }

            return list;
        }


        public IList<Point> Intersection(Circle2D circle)
        {
            return Helpers.AnalyticGeometryHelper.GetCircleFullLineIntersection(circle, this);
        }

        public IList<Point> Intersection(Line2D line)
        {
            return Helpers.AnalyticGeometryHelper.GetLineFullLineIntersection(line, this);
        }

        public IList<Point> Intersection(Point2D point)
        {
            return Helpers.AnalyticGeometryHelper.GetFullLinePointIntersection(this, point);
        }
       
    }
}
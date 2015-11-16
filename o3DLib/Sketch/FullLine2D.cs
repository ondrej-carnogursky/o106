using o3DLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace o3DLib.Sketching
{
    public class FullLine2D : Entity2D, IRay
    {
        //todo Rename FullLine2D to Ray2D
        public Point Point { get { return GetKeyPoint(KeyPointType.Start).Point; } } //; set; }

        public Vector Vector { get; set; }
        

        public FullLine2D(Point point, Vector vector)
        {
            //this.Point = point;
            this.Points2D.Add(new Point2D(point));
            this.Vector = vector;
        }

        public FullLine2D(Point2D point, Vector vector)
        {
            //this.Point = new Point(point.X, point.Y);
            this.Points2D.Add(point);
            this.Vector = vector;
        }

        public FullLine2D(Point point, double angle)
        {
            //this.Point = point;
            this.Points2D.Add(new Point2D(point));
            this.Vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public FullLine2D(Point2D point, double angle)
        {
            //this.Point = new Point(point.X, point.Y);
            this.Points2D.Add(point);
            this.Vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public FullLine2D(Point2D point1, Point2D point2)
        {
            //this.Point = new Point(point1.X, point1.Y);
            this.Points2D.Add(point1);
            this.Vector = point2.Point - point1.Point; //new Vector(point2.X - point1.X, point2.Y - point1.Y);
        }

        public FullLine2D(Point point1, Point point2)
        {
            //this.Point = point1;
            this.Points2D.Add(new Point2D(point1));
            this.Vector = point2 - point1; //new Vector(point2.X - point1.X, point2.Y - point1.Y);
        }

        public IList<Point> Intersection(FullLine2D line)
        {
            IList<Point> list = new List<Point>();

            double denominator = (this.Vector.Y * line.Vector.X - this.Vector.X * line.Vector.Y);

            if (!denominator.IsEqual(0))
            {
                double c1 = this.Vector.Y * this.Point.Y + this.Vector.X * this.Point.X;
                double c2 = line.Vector.Y * line.Point.Y + line.Vector.X * line.Vector.X;

                //double t1 = ((this.Point.X - line.Point.X) * line.Vector.Y + (line.Point.Y - this.Point.Y) * line.Vector.Y) / denominator;

                Point p = new Point((line.Vector.X * c1 - this.Vector.X * c2) / denominator,
                    (this.Vector.Y * c2 - line.Vector.Y * c1) / denominator);
                list.Add(p);

            }

            return list;
        }

        public override Point2D GetKeyPoint(KeyPointType type)
        {
            switch(type)
            {
                case KeyPointType.Start: return Points2D[0];
                default: return null;
            }
        }


        //public IList<Point> Intersection(Circle2D circle)
        //{
        //    return Helpers.AnalyticGeometryHelper.GetCircleFullLineIntersection(circle, this);
        //}

        //public IList<Point> Intersection(Line2D line)
        //{
        //    return Helpers.AnalyticGeometryHelper.GetLineFullLineIntersection(line, this);
        //}

        //public IList<Point> Intersection(Point2D point)
        //{
        //    return Helpers.AnalyticGeometryHelper.GetFullLinePointIntersection(this, point);
        //}

        //public override IList<Point> Intersection(IIntersectable shape)
        //{
        //    if(shape is FullLine2D)
        //    {
        //        return Intersection(shape as FullLine2D);
        //    }

        //    return null;
        //}
    }
}
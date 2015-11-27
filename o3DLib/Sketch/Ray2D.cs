using o3DLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace o3DLib.Sketching
{
    public class Ray2D : Entity2D, IRay
    {
        public Point Point { get; set; } //GetKeyPoint(KeyPointType.Start).Point; } } //; set; }

        public Vector Vector { get; set; }
        

        public Ray2D(Point point, Vector vector)
        {
            this.Point = point;
            this.Vector = vector;
        }

        public Ray2D(Point2D point, Vector vector)
        {
            this.Point = point.Point;
            this.Vector = vector;
        }

        public Ray2D(Point point, double angle)
        {
            this.Point = point;
            this.Vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public Ray2D(Point2D point, double angle)
        {
            this.Point = point.Point;
            this.Vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public Ray2D(Point2D point1, Point2D point2)
        {
            this.Point = point1.Point;
            this.Vector = point2.Point - point1.Point;
        }

        public Ray2D(Point point1, Point point2)
        {
            this.Point = point1;
            this.Vector = point2 - point1;
        }

        public Ray2D(IPoint point1, IPoint point2)
        {
            this.Point = new Point(point1.X, point1.Y);
            this.Vector = new Vector(point2.X - point1.X, point2.Y - point2.Y);
        }


        public override Point2D GetKeyPoint(KeyPointType type)
        {
            switch(type)
            {
                case KeyPointType.Start: return new Point2D(Point);
                // Just in case you need 3 points
                case KeyPointType.Middle: return new Point2D(this.Point + (this.Vector / 2));
                case KeyPointType.End: return new Point2D(this.Point + this.Vector);
                default: return null;
            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sketching
{
    class FullLine2D
    {

        public Point point;

        public Vector vector;
        

        public FullLine2D(Point point, Vector vector)
        {
            this.point = point;
            this.vector = vector;
        }

        public FullLine2D(Point2D point, Vector vector)
        {
            this.point = new Point(point.X, point.Y);
            this.vector = vector;
        }

        public FullLine2D(Point point, double angle)
        {
            this.point = point;
            this.vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public FullLine2D(Point2D point, double angle)
        {
            this.point = new Point(point.X, point.Y);
            this.vector = new Vector(Math.Cos(angle), Math.Sin(angle));
        }



        public IList<Point> intersection(FullLine2D line)
        {

            IList<Point> list = new List<Point>();

            double denominator = (this.vector.X * line.vector.X - this.vector.Y * line.vector.Y);

            if (denominator != (double)0)
            {
                double t1 = ((this.point.X - line.point.X) * line.vector.Y + (line.point.Y - this.point.Y) * line.vector.Y) / denominator;

                Point p = new Point(this.point.X + this.vector.X * t1, this.point.Y + this.vector.Y * t1);
                list.Add(p);

            }

            return list;
        }





       
    }
}
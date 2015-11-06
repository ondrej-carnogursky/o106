using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace o3DLib.Sketch
{
    class Point2D : IRelatable, RelatableTypes.IMovable
    {


        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }


        public IList<IRelation2D> Relations { get; set; } = new List<IRelation2D>();

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Point2D), new PropertyMetadata(0));





        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Point2D), new PropertyMetadata(0));


        public void moveTo(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public IList<Point2D> getRelatingPoints()
        {
            IList<Point2D> relatingPoints = new List<Point2D>() { this };
            return relatingPoints;
        }
    }
}

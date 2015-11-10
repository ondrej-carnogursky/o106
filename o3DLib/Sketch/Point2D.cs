﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace o3DLib.Sketching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Relations2D;
    using System.Windows;
    using System.Windows.Media.Media3D;

    public class Point2D : HelixToolkit.Wpf.PointsVisual3D, IRelatable
	{
        public Point2D():base() { this.Size = 5; }

        public Point2D(double X, double Y) : base()
        {
            this.X = X;
            this.Y = Y;
        }

        public Point2D(Entity2D parent):this()
        {
            this.Parent = parent;
            this.Points = new List<Point3D>() { this.Sketch.RefPlane.GetPoint3D(new Point(X, Y)) };
        }

        public Point2D(Entity2D parent, Point point) : this(parent)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        protected override void UpdateGeometry()
        {
            base.UpdateGeometry();
        }

        public Sketch Sketch
        {
            get
            {
                return this.Parent.Parent;
            }
        }

        public Entity2D Parent { get; set; }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Point2D), new PropertyMetadata(0.0,OnDPropertyChanged));

        private static void OnDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var point2D = d as Point2D;
            point2D.Points[0] = point2D.Sketch.RefPlane.GetPoint3D(new Point(point2D.X, point2D.Y));
            point2D.UpdateGeometry();
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Point2D), new PropertyMetadata(0.0, OnDPropertyChanged));
        private Point point;

        public virtual bool IsDriven
		{
			get;
			set;
		}

        public IList<Relation2D> Relations2D { get; set; } = new List<Relation2D>();

        public virtual IList<Point2D> GetRelatingPoints()
		{
            //todo WTF??
            IList<Point2D> points = new List<Point2D>();
            points.Add(this);
            return points;
		}
        

	}
}


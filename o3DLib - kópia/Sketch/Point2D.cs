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
    using Extensions;
    using Helpers;

    public class Point2D : HelixToolkit.Wpf.PointsVisual3D, IRelatable, IIntersectable, IPoint
	{
        public Point2D():base() { this.Size = 10; }

        public Point2D(double X, double Y) : base()
        {
            this.Point = new Point(X, Y);
        }

        public Point2D(Entity2D parent):this()
        {
            this.Parent = parent;
            this.Points = new List<Point3D>() { this.Sketch.RefPlane.GetPoint3D(Point) };
        }

        public Point2D(Entity2D parent, Point point) : this(parent)
        {
            this.Point = point;
        }

        public Point2D(Point point)
        {
            Point = point;
        }

        public string Name
        {
            get
            {
                return Math.Round(this.Point.X, 0) + "," + Math.Round(this.Point.Y, 0);
            }
        }

        public Sketch Sketch
        {
            get
            {
                return this.Parent.Parent;
            }
        }

        public Entity2D Parent { get; set; }

        public Point Point
        {
            get { return (Point)GetValue(PointProperty); }
            set
            {
                if (this.IsSatisfied || this.Parent == null)
                    SetValue(PointProperty, value);
                else
                {
                    var nvalue = this.SatisfyRelations(value);
                    if (nvalue != null)
                    {
                        SetValue(PointProperty, nvalue.GetNearest(value));
                        this.UpdateRelations();
                    }
                }
                this.IsSatisfied = false;
            }
        }
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(Point), typeof(Point2D), new PropertyMetadata(new Point(0, 0), OnDPropertyChanged));

        private static void OnDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var point2D = d as Point2D;
            if (point2D.Parent != null)
            {
                point2D.Points[0] = point2D.Sketch.RefPlane.GetPoint3D(point2D.Point);
                point2D.UpdateGeometry();
                point2D.Parent.Update();
            }
        }

        public virtual bool IsSatisfied
		{
			get;
			set;
		}

        public IList<Relation2D> Relations2D { get; set; } = new List<Relation2D>();

        public virtual IList<Point2D> GetRelatingPoints()
		{
            return new List<Point2D>() { this };
		}

        public IIntersectable SatisfyRelations(Point? p = null)
        {            
            IList<IIntersectable> possibles = new List<IIntersectable>();

            foreach (Relation2D rel in this.Relations2D)
            {
                if(!rel.IsDriven(this)) {
                    IIntersectable temp = rel.Satisfy();
                    possibles.Add(temp);
                }
            }


            switch(possibles.Count())
            {
                case 0:
                    return p == null ? null : new Point2D(p.Value);
                case 1:
                    return possibles[0];
                default:
                    for (int i = 1; i < possibles.Count; i++)
                    {
                        // TODO: Intersection of all possibles
                    }
                    return null;
            }
        }


        public void UpdateRelations()
        {
            foreach (Relation2D rel in this.Relations2D)
            {
                rel.Relatables.Except<IRelatable>(new List<IRelatable>() { this });

                if(!rel.IsDriven(this))
                {
                    Point2D driven = rel.GetDriven();
                    driven.Point = new Point(driven.X, driven.Y);
                }
            }
        }


        public IList<Point> Intersection(IIntersectable shape)
        {
            return shape.Intersection(this);
        }
        

        public double X { get { return Point.X; } }
        public double Y { get { return Point.Y; } }
    }
}


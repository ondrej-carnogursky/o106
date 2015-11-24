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
    using o3DLib.Extensions;
    using System.Windows.Media.Media3D;
    using System.Windows.Media;
    using System.Windows;
    using System.Collections.ObjectModel;

    public abstract class Entity2D : HelixToolkit.Wpf.LinesVisual3D, IRelatable, IEntity, IMovable
	{

        #region Constructors
        public Entity2D():base()
        {
            this.Points2D.CollectionChanged += Points2D_CollectionChanged;
            this.Color = Colors.Blue;
            this.Thickness = 5;
        }

        public Entity2D(Sketch parent):this()
        {
            Parent = parent;
        }


        public Entity2D(Sketch parent, params Point2D[] points) : this(parent)
        {
            foreach (var point2D in points)
                this.Points2D.Add(point2D);
        }

        public Entity2D(Sketch parent, params Point3D[] points) : this(parent)
        {
            foreach (var point3D in points)
                this.Points2D.Add(new Point2D(this, parent.RefPlane.GetPoint(point3D)));
        }
        #endregion

        #region Properties
        public virtual System.Collections.ObjectModel.ObservableCollection<Point2D> Points2D
        {
            get;
            set;
        } = new System.Collections.ObjectModel.ObservableCollection<Point2D>();

        public Sketch Parent { get; set; }


        public ObservableCollection<Relation2D> Relations2D { get; set; } = new ObservableCollection<Relation2D>();

        public string Name
        {
            get
            {
                var s = string.Empty;
                foreach (var aPoint in this.Points2D)
                    s += Math.Round(aPoint.Point.X,0) + "," + Math.Round(aPoint.Point.Y,0) + " ; ";
                return s;
            }
        }
        #endregion

        #region Abstract Methods
        public abstract Point2D GetKeyPoint(KeyPointType type);
        #endregion

        #region Methods
        public virtual IList<Point2D> GetRelatingPoints()
        {
            return new List<Point2D>() { GetKeyPoint(KeyPointType.Start), GetKeyPoint(KeyPointType.End) };
        }


        public virtual IList<Point> Intersection(IIntersectable shape)
        {
            return Helpers.AnalyticGeometryHelper.GetIntersections(this, shape);
        }


        public void Update()
        {
            UpdateGeometry();
        }

        protected override void UpdateGeometry()
        {
            for (int i = 0; i < Points2D.Count; i++)
                Points[i] = this.Parent.RefPlane.GetPoint3D(Points2D[i].Point);
            base.UpdateGeometry();
        }

        public IIntersectable SatisfyRelations(Point? p = null)
        {
            IIntersectable e = this;

            foreach (Relation2D rel in this.Relations2D)
            {
                rel.Satisfy();
            }
            return null;
        }

        public bool Move(double dx, double dy)
        {
            // Is able to move?
            bool isAble = true;

            // Store the points
            IList<Point> storedPoints = new List<Point>();
            foreach (Point2D p in this.Points2D)
            {
                storedPoints.Add(p.Point);

                //Try to move
                // If point did not change it's position
                if (!p.Move(dx, dy))
                    isAble = false;
            }

            // If is not able to move all points, return them back to their previous position
            if(!isAble)
            {
                for (int i = 0; i < this.Points2D.Count; i++)
                {
                    this.Points2D[i].Point = new Point(storedPoints[i].X, storedPoints[i].Y);
                }
                return false;
            }

            return true;
        }
        #endregion

        #region Events
        private void Points2D_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //todo Nebude fungovat pre Circle/Arc. Zdedime zo ScreenSpaceVisual??
            if (e.OldItems != null)
                foreach (Point2D child in e.OldItems)
                {
                    if (child.Parent == null) continue;
                    this.Points.RemoveAt(e.OldStartingIndex);
                    this.Children.Remove(child);
                }
            if (e.NewItems != null)
                foreach (Point2D child in e.NewItems)
                {
                    if (child.Parent == null || child.Parent != this) continue;
                    this.Children.Add(child);
                    this.Points.Add(child.Points[0]);
                }

        }
        #endregion
    }
}


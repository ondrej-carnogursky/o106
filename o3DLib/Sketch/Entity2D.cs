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
    using Sketching.Relations2D;

    public class Entity2D : HelixToolkit.Wpf.LinesVisual3D, IRelatable
	{
        public Entity2D(Sketch parent):base()
        {
            Parent = parent;
            this.Points2D.CollectionChanged += Points2D_CollectionChanged;
            this.Color = Colors.Blue;
        }

        public string Name
        {
            get
            {
                var s = string.Empty;
                foreach (var aPoint in this.Points2D)
                    s += aPoint.X + "," + aPoint.Y + " ; ";
                return s; //base.ToString();
            }
        }

        public Entity2D(Sketch parent, params Point2D[] points):this(parent)
        {
            foreach (var point2D in points)
                this.Points2D.Add(point2D);
        }

        public Entity2D(Sketch parent, params Point3D[] points) : this(parent)
        {
            foreach (var point3D in points)
                this.Points2D.Add(new Point2D(this,parent.RefPlane.GetPoint(point3D)));
        }

        private void Points2D_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (Point2D child in e.OldItems)
                {
                    this.Points.RemoveAt(e.OldStartingIndex);
                    this.Children.Remove(child);
                }
            if (e.NewItems != null)
                foreach (Point2D child in e.NewItems)
                {
                    this.Children.Add(child);
                    this.Points.Add(child.Points[0]);
                }

        }

        public virtual System.Collections.ObjectModel.ObservableCollection<Point2D> Points2D
		{
			get;
			set;
        } = new System.Collections.ObjectModel.ObservableCollection<Point2D>();

        public Sketch Parent { get; set; }


        public IList<Relation2D> Relations2D { get; set; }

        public virtual Point2D GetKeyPoint(KeyPointType type)
		{
			switch(type)
            {
                case KeyPointType.Start:
                    return this.Points2D.First();
                case KeyPointType.Middle:
                    return new Point2D((this.Points2D.Last().X - this.Points2D.First().X) / 2,
                        (this.Points2D.Last().Y - this.Points2D.First().Y) / 2);
                case KeyPointType.End:
                    return this.Points2D.Last();
            }
            return null;
		}

		public virtual bool SatisfyRelations()
		{
            return true;
		}

		public virtual IList<Point2D> GetRelatingPoints()
		{
            return this.Points2D;
		}

	}
}


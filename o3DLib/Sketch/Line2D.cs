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
    using System.Windows.Media.Media3D;
    using System.Collections.ObjectModel;

	public class Line2D : Entity2D
	{
        public Line2D(Sketch parent, Point3D start, Point3D end) : base(parent,start,end) {}
        public Line2D(Sketch parent, Point2D start, Point2D end) : base(parent, start, end) { }

        public virtual void moveTo(double x, double y)
		{
			throw new System.NotImplementedException();
		}

		public virtual void rotate(double angle)
		{
			throw new System.NotImplementedException();
		}

		public virtual void stretch(double newLength)
		{
			throw new System.NotImplementedException();
		}

	}
}


﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Sketching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Relations2D;

    public class Point2D : IRelatable
	{
		public virtual bool IsDriven
		{
			get;
			set;
		}

        public IEnumerable<IRelation2D> IRelation2D
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Relation2D> Relation2D
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual void moveTo(double x, double y)
		{
			throw new System.NotImplementedException();
		}

		public virtual IList<Point2D> GetRelatingPoints()
		{
			throw new System.NotImplementedException();
		}

	}
}


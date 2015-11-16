﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace o3DLib.Sketching.Relations2D
{
	using o3DLib.Sketching;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Lock : Relation2D
	{



        public Lock(params IRelatable[] entities) : base(entities) { }
        public Lock(params Point2D[] points):base(points) { }


        public override IList<Relation2D> ByPass(params Point2D[] points)
        {
            IList<Relation2D> rels = new List<Relation2D>();

            foreach(Point2D point in points)
                rels.Add(new Lock(point));

            return  rels;
        }

        

        public override IIntersectable Satisfy()
        {
            return (this.Relatables.First() as Point2D);
        }

    }
}


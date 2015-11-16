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

    public class Collinear : Relation2D
    {

        public Collinear(params IRelatable[] entities) : base(entities) { }
        public Collinear(params Point2D[] points) : base(points) { }


        public override IList<Relation2D> ByPass(params Point2D[] points)
        {
            IList <Relation2D> rels = new List<Relation2D>();

            for (int i = 2; i < points.Count(); i++)
            {
                rels.Add(new Collinear(points[0], points[1], points[i]));
            }
            return rels;
        }


        public override IIntersectable Satisfy()
        {
            // Applyable only on 3 points
            if (this.Relatables.Count() == 3)
            {
                return new Ray2D((Point2D)this.Relatables[0], (this.Relatables[1] as Point2D).Point - (this.Relatables[0] as Point2D).Point);
            }

            return null;
        }


    }
}


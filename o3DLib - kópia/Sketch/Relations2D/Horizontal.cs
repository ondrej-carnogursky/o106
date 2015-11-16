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
    using System.Windows;

    using Extensions;

    public class Horizontal : Relation2D
    {

        public Horizontal(Entity2D entity) : base(entity) { }
        public Horizontal(params Point2D[] points):base(points) { }


        public override IList<Relation2D> ByPass(params Point2D[] points)
        {
            Horizontal rel = new Horizontal(points);
            return new List<Relation2D>() { rel };
        }


        public override IIntersectable Satisfy()
        {
            return new Ray2D((Point2D)this.Relatables[0], new Vector(1, 0));
        }

        
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketching.Relations2D
{
    class PointOn : Relation2D
    {
        public PointOn(params IRelatable[] entities) : base(entities) { }
        public PointOn(params Point2D[] points) : base(points) { }


        public override IList<Relation2D> ByPass(params Point2D[] points)
        {
            Connect rel = new Connect(points);
            return new List<Relation2D>() { rel };
        }


        public override IIntersectable Satisfy()
        {
            // Applyable only on 3 or 2 points
            if (this.Relatables.Count() == 3)
            {
                return new Line2D(this.Relatables[0] as Point2D, this.Relatables[1] as Point2D);
            }
            else if (this.Relatables.Count() == 2)
            {
                return (this.Relatables[0] as Point2D);
            }

            return null;
        }


    }
}

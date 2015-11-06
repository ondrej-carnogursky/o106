using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch
{
    interface IRelatable
    {


        IList<IRelation2D> Relations { get; set; }

        IList<Point2D> getRelatingPoints();


    }
}

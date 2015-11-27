using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace o3DLib.Sketching
{
    public class Circle2D : Entity2D, ICircle
    {
        public Point Center { get { return GetKeyPoint(KeyPointType.Center).Point; } }
        public double Radius { get; set; }
        //public override IList<Point> Intersection(IIntersectable shape)
        //{
        //    throw new NotImplementedException();
        //}

        public override Point2D GetKeyPoint(KeyPointType type)
        {
            switch(type)
            {
                case KeyPointType.Center: return Points2D[0]; 
                default: return null;
            }
        }
    }
}

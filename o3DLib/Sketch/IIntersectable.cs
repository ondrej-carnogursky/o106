using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace o3DLib.Sketching
{
    public interface IIntersectable
    {

        IList<Point> Intersection(IIntersectable shape);

    }
}

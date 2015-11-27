using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketching
{
    public enum IntersectableType : int
    {
        Point = 0,
        Segment = 1,
        Ray = 2,
        Arc = 3,
        Circle = 4
    }
}

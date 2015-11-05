using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch.RelatableTypes
{
    interface IMovable
    {

        double X { get; }

        double Y { get; }

        void moveTo(double x, double y);

    }
}

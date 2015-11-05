using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch.RelatableTypes
{
    interface IRotatable
    {


        double Angle { get; }

        void rotate(double angle);

    }
}

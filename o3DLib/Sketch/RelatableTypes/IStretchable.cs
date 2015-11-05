using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch.RelatableTypes
{
    interface IStretchable
    {

        double Length { get; }

        void stretch(double newLength);

    }
}

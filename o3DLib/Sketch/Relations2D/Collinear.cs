using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch.Relations2D
{
    class Collinear : IRelation2D
    {
        public bool Satisfy()
        {
            return true;
        }
    }
}

using o3DLib.Sketch.RelatableTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch.Relations2D
{
    class Lock : IRelation2D
    {

        private IMovable entity;

        public Lock(IMovable entity)
        {

            this.entity = entity;

        }

        public bool Satisfy()
        {
            entity.moveTo();
        }

    }
}

using o3DLib.Sketch.RelatableTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Sketch.Relations2D
{
    class Lock : IRelation2D
    {

        private IRelatable target;

        private IList<Point2D> storedPoints = new List<Point2D>();

        public Lock(IRelatable target)
        {

            this.target = target;

            storeCurrentPoints();

        }

        public bool Satisfy()
        {

            foreach (Point2D point in this.target.getRelatingPoints())
            {

            }

            return true;
        }


        private void storeCurrentPoints()
        {

            IList<Point2D> points = target.getRelatingPoints();


            foreach (Point2D point in points)
            {
                storedPoints.Add(new Point2D(point.X, point.Y));
            }
        }

    }
}

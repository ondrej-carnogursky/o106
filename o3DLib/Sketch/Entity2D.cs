using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace o3DLib.Sketch
{
    abstract class Entity2D : IRelatable
    {

        public IList<Point2D> Points { get; set; } = new List<Point2D>();

        public IList<IRelation2D> Relations { get; set; } = new List<IRelation2D>();
    }
}

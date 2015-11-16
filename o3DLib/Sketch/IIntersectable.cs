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
        //Point? GetNearest(Point point);
    }

    public interface IEntity:IIntersectable
    {
        Point2D GetKeyPoint(KeyPointType type);
    }

    public interface IPoint
    {
        double X { get; }
        double Y { get; }
    }

    public interface ISegment:IEntity
    {
        IPoint StartPoint { get; }
        IPoint EndPoint { get; }
    }

    public interface IRay:IEntity
    {
        Point Point { get; set; }
        Vector Vector { get; set; }
    }

    public interface IArc: ICircle
    {
        double StartAngle { get; set; }
        double EndAngle { get; set; }
        bool Clockwise { get; set; }

    }

    public interface ICircle:IEntity
    {
        Point Center { get; }
        double Radius { get; set; }
    }
}

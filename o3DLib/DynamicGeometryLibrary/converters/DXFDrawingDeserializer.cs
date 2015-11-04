using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using netDxf;
using System.Collections.ObjectModel;
using netDxf.Entities;
using System.Windows.Controls;
using netDxf.Blocks;
using System.Windows.Media;

namespace DynamicGeometry
{
    public class DXFDrawingDeserializer
    {

        Drawing drawing;
        DxfDocument doc;
        private double _delta;
        public void setDelta(double delta)
        {
            this._delta = delta;
        }


        public Drawing ReadDrawing(string dxfFileName, Canvas canvas)
        {
            doc = new DxfDocument();
            doc.Load(dxfFileName);
            //doc = netDxf.DxfDocument.Load(dxfFileName);

            drawing = new Drawing(canvas);

            //ReadLines();
            //ReadPolylines();
            //ReadArcs();
            //ReadCircles();
            //ReadInserts();
            ReadEntities();

            drawing.Recalculate();
            return drawing;
        }

        void ReadEntities()
        {
            foreach(IEntityObject aEntity in doc.Entities)
                switch (aEntity.Type)
                {
                    case EntityType.Arc:
                        ReadArc(aEntity as netDxf.Entities.Arc, 0, 0);
                        (drawing.Figures[drawing.Figures.Count - 1] as FigureBase).Tag = aEntity;
                        break;
                    case EntityType.Circle:
                        ReadCircle(aEntity as netDxf.Entities.Circle, 0, 0);
                        (drawing.Figures[drawing.Figures.Count - 1] as FigureBase).Tag = aEntity;
                        break;
                    //case EntityType.Ellipse:
                    //    ReadEllipse(aEntity as netDxf.Entities.Ellipse, 0, 0);
                    //    break;
                    case EntityType.Line:
                        ReadLine(aEntity as netDxf.Entities.Line, 0, 0);
                        (drawing.Figures[drawing.Figures.Count - 1] as FigureBase).Tag = aEntity;
                        break;
                    case EntityType.Polyline:
                        netDxf.Entities.Polyline polyline = CastPolyline(aEntity);
                        if (polyline != null)
                        {
                            ReadPolyline(polyline.Vertexes, polyline.IsClosed, 0, 0);
                        }
                        (drawing.Figures[drawing.Figures.Count - 1] as FigureBase).Tag = aEntity;
                        break;
                    case EntityType.Insert:
                        netDxf.Entities.Insert insert = aEntity as netDxf.Entities.Insert;
                        foreach (netDxf.Entities.Line aLine in insert.Block.Entities)
                        {
                            ReadLine(aLine, insert.InsertionPoint.X, insert.InsertionPoint.Y);
                            (drawing.Figures[drawing.Figures.Count - 1] as FigureBase).Tag = aLine;
                        }
                        break;
                    default:
                        break;
                }
        }

        FreePoint CreateHiddenPoint(double x, double y)
        {
            return new FreePoint()
            {
                Drawing = drawing,
                X = x,
                Y = y,
                //Visible = false
            };
        }

        Segment CreateSegment(IPoint p1, IPoint p2)
        {
            return Factory.CreateSegment(drawing, new[] { p1, p2 });
        }

        netDxf.Entities.Polyline CastPolyline(IEntityObject item) //IPolyline item)
        {
            netDxf.Entities.Polyline polyline = null;
            //if (item is LightWeightPolyline)
            //{
            //    polyline = ((LightWeightPolyline)item).ToPolyline();
            //}
            //else 
            if (item is Polyline)
            {
                polyline = (netDxf.Entities.Polyline)item;
            }
            else
            {
                polyline = null;
            }
            return polyline;
        }

        void ReadLines()
        {
            foreach (var line in doc.Lines)
            {
                ReadLine(line, 0, 0);
            }
        }

        void ReadLine(Line line, double x, double y)
        {
            var aPoint1 = CreateHiddenPoint(line.StartPoint.X + x, line.StartPoint.Y + y);
            var point1 =compareFreePoints(this.drawing.Figures,aPoint1);
            if (point1 == null)
            {
                point1 = aPoint1;
                this.drawing.Figures.Add(point1);
            }

            var aPoint2 = CreateHiddenPoint(line.EndPoint.X + x, line.EndPoint.Y + y);
            var point2 = compareFreePoints(this.drawing.Figures, aPoint2);
            if (point2 == null)
            {
                point2 = aPoint2;
                this.drawing.Figures.Add(point2);
            }
            var segment = CreateSegment(point1, point2);
            Actions.Add(drawing, segment);
        }

        void ReadPolylines()
        {
            foreach (var item in doc.Polylines)
            {
                netDxf.Entities.Polyline polyline = CastPolyline(item);
                if (polyline != null)
                {
                    ReadPolyline(polyline.Vertexes, polyline.IsClosed, 0, 0);
                }
            }
        }

        void ReadPolyline(IList<PolylineVertex> vertices, bool isClosed, double x, double y)
        {
            IPoint firstPoint = null;
            IPoint previousPoint = null;
            var figures = new List<IFigure>();
            var segments = new List<IFigure>();

            foreach (var vertex in vertices)
            {
                var point = CreateHiddenPoint(vertex.Location.X + x, vertex.Location.Y + y);
                if (firstPoint == null)
                {
                    firstPoint = point;
                }
                if (previousPoint != null)
                {
                    var segment = CreateSegment(previousPoint, point);
                    figures.Add(segment);
                    segments.Add(segment);
                }
                previousPoint = point;
                figures.Add(point);
            }


            if (previousPoint != null && isClosed)
            {
                var segment = CreateSegment(previousPoint, firstPoint);
                figures.Add(segment);
                segments.Add(segment);

                var polygon = Factory.CreatePolygon(drawing, figures);
                Actions.Add(drawing, polygon);
            }

            Actions.AddMany(drawing, segments.ToArray());
        }

        void ReadArcs()
        {
            foreach (var item in doc.Arcs)
            {
                ReadArc(item, 0, 0);
            }
        }

        void ReadArc(netDxf.Entities.Arc arc, double x, double y)
        {
            // TODO :  
            var point0 = CreateHiddenPoint(arc.Center.X + x, arc.Center.Y + y);

            var aPoint1 = CreateHiddenPoint(point0.X + arc.Radius * System.Math.Cos(System.Math.PI / 180 * arc.StartAngle), point0.Y + arc.Radius * System.Math.Sin(System.Math.PI / 180 * arc.StartAngle));
            var point1 = compareFreePoints(this.drawing.Figures, aPoint1);
            if (point1 == null)
            {
                point1 = aPoint1;
                this.drawing.Figures.Add(point1);
            }
            var aPoint2 = CreateHiddenPoint(point0.X + arc.Radius * System.Math.Cos(System.Math.PI / 180 * arc.EndAngle), point0.Y + arc.Radius * System.Math.Sin(System.Math.PI / 180 * arc.EndAngle));
            var point2 = compareFreePoints(this.drawing.Figures, aPoint2);
            if (point2 == null)
            {
                point2 = aPoint2;
                this.drawing.Figures.Add(point2);
            }
            var figure = Factory.CreateArc(drawing, new[] { point0, point1, point2 });

            Actions.Add(drawing, figure);
        }

        void ReadCircles()
        {
            foreach (var item in doc.Circles)
            {
                ReadCircle(item, 0, 0);
            }
        }

        void ReadCircle(netDxf.Entities.Circle circle, double x, double y)
        {
            var figures = new List<IFigure>();

            figures.Add(CreateHiddenPoint(circle.Center.X + x, circle.Center.Y + y));
            figures.Add(CreateHiddenPoint(circle.Center.X + x + circle.Radius, circle.Center.Y + y));

            this.drawing.Add(figures);

            var figure = Factory.CreateCircleByRadius(drawing, figures);

            Actions.Add(drawing, figure);
        }

        void ReadInserts()
        {
            foreach (var item in doc.Inserts)
            {
                ReadInsert(item);
            }
        }


        private FreePoint compareFreePoints(RootFigureList figureList, FreePoint aPoint)
        {
            foreach (IFigure aFigure in figureList)
                if (aFigure is FreePoint && Math.Abs((aFigure as FreePoint).X - aPoint.X)<_delta && Math.Abs((aFigure as FreePoint).Y - aPoint.Y)<_delta)
                    return aFigure as FreePoint;
            
            return null;
        }

        void ReadInsert(netDxf.Entities.Insert insert)
        {
            List<netDxf.Entities.IEntityObject> entities = insert.Block.Entities;
            //netDxf.Collections.EntityCollection entities = insert.Block.Entities;
            netDxf.Entities.IEntityObject entity = null;
            //netDxf.Entities.EntityObject entity = null;

            for (int index = 1; index < entities.Count; index++)
            {
                entity = entities[index];

                if (entity is Line)
                    ReadLine((Line)entity, insert.InsertionPoint.X, insert.InsertionPoint.Y);
                    //ReadLine((Line)entity, insert.Position.X, insert.Position.Y);
                else if (entity is netDxf.Entities.Arc)
                    ReadArc((netDxf.Entities.Arc)entity, insert.InsertionPoint.X, insert.InsertionPoint.Y);
                    //ReadArc((netDxf.Entities.Arc)entity, insert.Position.X, insert.Position.Y);
                else if (entity is netDxf.Entities.Circle)
                    ReadCircle((netDxf.Entities.Circle)entity, insert.InsertionPoint.X, insert.InsertionPoint.Y);
                    //ReadCircle((netDxf.Entities.Circle)entity, insert.Position.X, insert.Position.Y);
                else if (entity is netDxf.Entities.Polyline) //IPolyline)
                {
                    netDxf.Entities.Polyline polyline = CastPolyline((netDxf.Entities.Polyline)entity); //IPolyline)entity);
                    if (polyline != null)
                    {
                        ReadPolyline(polyline.Vertexes, polyline.IsClosed, 0, 0);
                    }
                }

            }

        }

    }
}

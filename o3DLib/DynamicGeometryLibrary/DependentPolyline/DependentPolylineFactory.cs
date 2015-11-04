using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MoreLinq;

namespace DynamicGeometry
{
    
    public static class DependentPolylineFactory
    {

        public static void getDependentPolylineFromFigure(IEnumerable<IFigure> allFigures, IFigure figure, ref List<MyPath> figurePaths)
        {
            if (figure is CartesianGrid) return;
            if (figurePaths.Any<MyPath>((aPath) => { return aPath.Figures.Contains(figure); }))
                return;
            List<IFigure> nList = new List<IFigure>();
            getDependenciesList(allFigures, figure, ref nList);
            if (getCrossNodes(nList).Count > 0)
            {
                var aVertices = getVertices(nList); //.OfType<FreePoint>().Where<FreePoint>((aFig) => { return !isCenter(aFig); });
                var aRightVertex = aVertices.MaxBy<FreePoint, double>((aPoint) => { return aPoint.X; });
                var aNextFigure = aRightVertex.Dependents.Intersect<IFigure>(nList).MaxBy<IFigure, double>((aFig) => { return getAngle(aFig, aRightVertex); });
                var aOuterPath = new List<IFigure>();
                while (aNextFigure != null && (aOuterPath.Count == 0 || aNextFigure != aOuterPath[0]))
                {
                    aOuterPath.Add(aNextFigure);
                    aRightVertex = getNextVertex(aNextFigure, aRightVertex);
                    var aNextFigures = aRightVertex.Dependents.Where<IFigure>((aFig) => { return nList.Contains<IFigure>(aFig); }).Without<IFigure>(aNextFigure);
                    aNextFigure = aNextFigures.Count<IFigure>() == 0 ? null : aNextFigures.MinBy<IFigure, double>((aFig) => { return getAngle(aNextFigure, aFig); });
                }
                if (aOuterPath.Count == nList.Count)
                {
                    figurePaths.Add(new MyPath(aOuterPath) {Name = "Path" + (figurePaths.Count + 1) });
                }
                else
                {
                    figurePaths.Add(new MyPath(aOuterPath) {Name = "Path" + (figurePaths.Count + 1) });
                    //figurePaths.Add(new MyPath(nList.Except<IFigure>(aOuterPath)) {Name = "Path" + figurePaths.Count + 1 });
                    var remainFigures = nList.Except<IFigure>(aOuterPath);
                    foreach (FigureBase aFig in remainFigures)
                        DynamicGeometry.DependentPolylineFactory.getDependentPolylineFromFigure(remainFigures, aFig, ref figurePaths);
                }
            }
            else
            {
                figurePaths.Add(new MyPath(nList.Count == 0 ? new List<IFigure>(new IFigure[] {figure}) : nList ) {Name = "Path" + (figurePaths.Count + 1) });
            }
        }

        private static bool isCenter(FreePoint aPoint)
        {
            if(aPoint.Dependents.Count == 1)
            {
                var aDependencies = aPoint.Dependents[0].Dependencies.Without<IFigure>(aPoint).ToList<IFigure>();
                if (aDependencies.Count == 2)
                    return Math.Round((aPoint.Coordinates - (aDependencies[0] as FreePoint).Coordinates).Length, 2) == Math.Round((aPoint.Coordinates - (aDependencies[1] as FreePoint).Coordinates).Length, 2);
                else
                    return false;
            }
            else
                return false;
        }

        private static FreePoint getNextVertex(IFigure aFig, FreePoint aPoint)
        {
            if(aFig is CircleArcBase)
                return Math.Round(((aFig.Dependencies[1] as FreePoint).Coordinates - aPoint.Coordinates).Length, 2) == 0 ? aFig.Dependencies[2] as FreePoint : aFig.Dependencies[1] as FreePoint;
            else
                return Math.Round(((aFig.Dependencies[0] as FreePoint).Coordinates - aPoint.Coordinates).Length, 2) == 0 ? aFig.Dependencies[1] as FreePoint : aFig.Dependencies[0] as FreePoint;
            //var aDependents = aFig.Dependencies.Without<IFigure>(aPoint); //.Where<IFigure>((aaFig)=>{return aaFig.Dependents.Without<IFigure>(aaFig.Center).Count<IFigure>()>0;});
            //return aDependents.Count<IFigure>() == 0 ? null : aDependents.First<IFigure>() as FreePoint;
        }

        private static double getAngle(IFigure aFig, FreePoint aEndPoint)
        {
            if (aFig is CircleArcBase)
            {
                var aAngle = aEndPoint.Coordinates.AngleTo((aFig as CircleArcBase).Center);
                var aAngle2 = getNextVertex(aFig,aEndPoint).Coordinates.AngleTo((aFig as CircleArcBase).Center);
                if(aAngle2<aAngle)
                    return aEndPoint.Coordinates.AngleTo((aFig as CircleArcBase).Center) + Math.PI / 2;
                else
                    return aEndPoint.Coordinates.AngleTo((aFig as CircleArcBase).Center) - Math.PI / 2;
            }
            else
                return aEndPoint.Coordinates.AngleTo(getNextVertex(aFig, aEndPoint).Coordinates);

        }

        private static double getAngle(IFigure aFig1, IFigure aFig2)
        {
            var aCommonVertex = aFig1.Dependencies.Union<IFigure>(aFig2.Dependencies).First<IFigure>((aFig) => { return aFig1.Dependencies.Contains(aFig) && aFig2.Dependencies.Contains(aFig); }) as FreePoint;
            var aAngle = 2 * Math.PI - (getAngle(aFig2,aCommonVertex) - getAngle(aFig1, aCommonVertex));
            if (aAngle > 2 * Math.PI)
                return aAngle - 2 * Math.PI;
            else
                return aAngle; //< 0 ? aAngle + 2 * Math.PI : aAngle;
        }

        private static IFigure getNextFigure(IFigure aFig, ref FreePoint aPoint)
        {
            if (aFig.Dependencies.Count < 2)
                return null;
            else
                if (aFig.Dependencies[0] == aPoint)
                    aPoint = aFig.Dependencies[1] as FreePoint;
                else
                    aPoint = aFig.Dependencies[0] as FreePoint;
            return aPoint.Dependents.Count != 2 ? null : aPoint.Dependents[0] == aFig ? aPoint.Dependents[1] : aPoint.Dependents[0];
        }

        private static List<FreePoint> getPoints(List<IFigure> aPath, FreePoint sPoint)
        {
            var aPoints = new List<FreePoint>();
            var aFig = sPoint.Dependents.Where<IFigure>((aaFig) => { return aPath.Contains(aaFig); }).First<IFigure>(); //aPath[0];
            FreePoint aPoint = sPoint; //aFig.Dependencies[0].Dependents.Count > 2 ? aFig.Dependencies[0] as FreePoint : aFig.Dependencies[1] as FreePoint;
            while (aFig != null)
            {
                aPoints.Add(aPoint);
                aFig = getNextFigure(aFig, ref aPoint);
            }
            return aPoints;
        }

        private static List<FreePoint> getVertices(List<IFigure> aPath)
        {
            var aPoints = new List<FreePoint>();
            aPath.ForEach<IFigure>((aFig) => { aFig.Dependencies.ForEach<IFigure>((aPoint) => { if (aPoint is FreePoint && !isCenter(aPoint as FreePoint) && !aPoints.Contains(aPoint)) aPoints.Add(aPoint as FreePoint); }); });
            return aPoints;
        }

        private static List<FreePoint> getCrossNodes(List<IFigure> aPath)
        {
            var aList = new List<FreePoint>();
            aPath.ForEach<IFigure>((aFig) =>
            {
                foreach (var aDep in aFig.Dependencies)
                    if (aDep is FreePoint && aDep.Dependents.Count > 2 && !aList.Contains(aDep))
                        aList.Add(aDep as FreePoint);
            });
            return aList;
        }

        private static void getDependenciesList(IEnumerable<IFigure> allFigures, IFigure figure, ref List<IFigure> depList)
        {
            foreach(IFigure dependency in figure.Dependencies)
            {
                foreach (IFigure fig in allFigures)
                {
                    if (fig != figure && fig.Dependencies.Contains(dependency))
                        if (!(depList.Contains(fig)))
                        {
                            depList.Add(fig);
                            getDependenciesList(allFigures, fig, ref depList);
                        }
                        else if(depList.Last() == fig)
                            return;
                }
            }
        }

    }

}

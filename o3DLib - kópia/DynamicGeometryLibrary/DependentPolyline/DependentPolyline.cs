using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicGeometry
{
    public class DependentPolyline : Object
    {

        public string Name {get; set;}

        private List<IFigure> _figuresList;

        public object Tag { get; set; }

        public DependentPolyline()
        {
            this._figuresList = new List<IFigure>();
        }

        public DependentPolyline(string name, List<IFigure> figuresList)
        {
            this.Name = name;
            this._figuresList = figuresList;
        }

        //public void FillFiguresList(List<IFigure> figuresList)
        //{
        //    this._figuresList = figuresList;
        //}
        
        public void AddFigure(IFigure figure)
        {
            this._figuresList.Add(figure);
        }

        public List<IFigure> GetFigures()
        {
            return this._figuresList;
        }

        public bool Contains(IFigure figure)
        {
            return this._figuresList.Contains(figure);
        }

        public override string ToString()
        {
            return this.Name;
        }

        //public void ToString(string name)
        //{
        //    this.Name = name;
        //}

    }
}

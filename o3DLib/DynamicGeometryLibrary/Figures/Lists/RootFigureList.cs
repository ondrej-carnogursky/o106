using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows;

namespace DynamicGeometry
{
    
    public partial class RootFigureList : FigureList
    {
        public RootFigureList(Drawing drawing)
            : base(drawing)
        {
            FigureDictionary = new Dictionary<FrameworkElement, IFigure>();
        }

        public Dictionary<FrameworkElement, IFigure> FigureDictionary { get; set; }

        protected override void OnItemAdded(IFigure item)
        {
            item.RegisterWithDependencies();
            item.OnAddingToDrawing(Drawing);
            if (Drawing.Canvas != null)
            {
                item.OnAddingToCanvas(Drawing.Canvas);
                item.RecalculateAndUpdateVisual();
            }
        }

        protected override void OnItemRemoved(IFigure item)
        {
            item.OnRemovingFromDrawing(Drawing);
            if (Drawing.Canvas != null)
            {
                item.OnRemovingFromCanvas(Drawing.Canvas);
            }
            item.UnregisterFromDependencies();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicGeometry
{
    [PropertyChanged.ImplementPropertyChanged]
    public class MyPath
    {
        public string Name { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<IFigure> Figures { get; set; }
        [PropertyChanged.AlsoNotifyFor("IsSelected", "IsVisible")]
        public bool NeedRefresh { get; set; }

        public MyPath()
        {
            Figures = new System.Collections.ObjectModel.ObservableCollection<IFigure>();
            Figures.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Figures_CollectionChanged);
        }

        public MyPath(IEnumerable<IFigure> aFigures)
            : this()
        {
            Figures.AddRange<IFigure>(aFigures);
        }

        void Figures_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move) return;
            if(e.OldItems != null)
                foreach (FigureBase aFig in e.OldItems)
                    aFig.Path = null;
            if (e.NewItems != null)
                foreach (FigureBase aFig in e.NewItems)
                    aFig.Path = this;

        }

        public bool IsSelected
        {
            get
            {
                NeedRefresh = false;
                return Figures.Where<IFigure>((fig) => fig.Selected == true).Count<IFigure>() == Figures.Count;
            }
            set
            {
                Figures.ForEach<IFigure>((aFig)=> aFig.Selected = value);
            }
        }

        public bool IsVisible
        {
            get
            {
                NeedRefresh = false;
                return Figures.Where<IFigure>((fig) => fig.Visible == true).Count<IFigure>() == Figures.Count;
            }
            set
            {
                Figures.ForEach<IFigure>((aFig) => aFig.Visible = value);
            }
        }

        //public void SetPath()
        //{
        //    foreach (FigureBase aFig in Figures)
        //        aFig.Path = this;
        //}
    }
}

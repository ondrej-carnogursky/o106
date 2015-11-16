﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace o3DLib.Sketching
{
    using o3DLib.Sketching.Relations2D;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Collections.ObjectModel;

    public interface IRelatable 
	{
		ObservableCollection<Relation2D> Relations2D { get;set; }

		IList<Point2D> GetRelatingPoints();

        IIntersectable SatisfyRelations(Point? p = null);

        string Name { get; }

    }
}


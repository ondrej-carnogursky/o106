﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace o3DLib.Sketching.Relations2D
{
	using o3DLib.Sketching;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IRelation2D 
	{
        IIntersectable Satisfy();
        IList<IRelatable> Relatables { get; set; }
        IList<IRelation2D> ChildRelations { get; set; }
    }
}


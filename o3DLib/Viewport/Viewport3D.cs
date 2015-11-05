using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib//.Viewport
{
    public class Viewport3D: HelixToolkit.Wpf.HelixViewport3D
    {

        public Viewport3D():base()
        {
            Background = System.Windows.Media.Brushes.SkyBlue;
            ZoomExtentsWhenLoaded = true;
        }
        
    }
}

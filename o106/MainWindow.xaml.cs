using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using o3DLib.Sketching;
using HT = HelixToolkit.Wpf;

namespace o106
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point? lastClick = null;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        public o3DLib.Sketching.Entity2D Entity { get; set; }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var line2d = new Line2D(sketch);
            sketch.Entities.Add(line2d);
            line2d.Points2D.Add(new Point2D(line2d) { X = 0, Y = 0 });
            line2d.Points2D.Add(new Point2D(line2d) { X = 100, Y = 100 });
            var line2 = new Line2D(sketch);
            sketch.Entities.Add(line2);
            line2.Points2D.Add(new Point2D(line2) { X = 0, Y = 0 });
            line2.Points2D.Add(new Point2D(line2) { X = 100, Y = 100 });
            //line2.Relations2D.Add(new o3DLib.Sketching.Relations2D.Lock(line2));
            viewport.Viewport.MouseLeftButtonUp += viewport_MouseLeftButtonUp;


        }

        private void viewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lastClick == null)
                lastClick = e.GetPosition(viewport.Viewport);
            else
            {
                var p1Ray = HT.Viewport3DHelper.GetRay(viewport.Viewport, lastClick.Value);
                var p1 = HT.Viewport3DHelper.UnProject(viewport.Viewport, lastClick.Value, refPlane.Plane.Position, refPlane.Plane.Normal);
                var p = e.GetPosition(viewport.Viewport);
                var p2Ray = HT.Viewport3DHelper.GetRay(viewport.Viewport, p);
                var p2 = HT.Viewport3DHelper.UnProject(viewport.Viewport, p, refPlane.Plane.Position, refPlane.Plane.Normal);
                var line2d = new Line2D(sketch,p1.Value,p2.Value);
                sketch.Entities.Add(line2d);
                lastClick = null;
            }
        }
    }
}

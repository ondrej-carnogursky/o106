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

namespace o106
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Point2D p1 = new Point2D() { X = 0, Y = 0 };
            //Point2D p2 = new Point2D() { X = 100, Y = 100 };
            //line2D.Points2D.Add(p1);
            //line2D.Points2D.Add(p2);

            sketch.Entities.Add(new Line2D(sketch, new Point2D() { X = 0, Y = 0 }, new Point2D() { X = 100, Y = 100 }));
        }
    }
}

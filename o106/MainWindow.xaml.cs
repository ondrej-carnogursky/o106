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
using System.Collections.ObjectModel;
using o3DLib.Extensions;
using o3DLib.Sketching.Relations2D;
using System.Windows.Controls.Primitives;

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
        public ObservableCollection<IRelatable> Selected { get; private set; } = new ObservableCollection<IRelatable>();

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Selected.CollectionChanged += Selected_CollectionChanged;
        }

        private void Selected_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems !=null)
                foreach (HT.ScreenSpaceVisual3D ent in e.OldItems)
                    ent.Color = ent is Point2D ? Colors.Black : Colors.Blue;
            if (e.NewItems != null)
                foreach (HT.ScreenSpaceVisual3D ent in e.NewItems)
                    ent.Color = Colors.Yellow;
        }

        private void AddLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lastClick == null)
                lastClick = e.GetPosition(viewport.Viewport);
            else
            {
                //var p1Ray = HT.Viewport3DHelper.GetRay(viewport.Viewport, lastClick.Value);
                var p1 = HT.Viewport3DHelper.UnProject(viewport.Viewport, lastClick.Value, refPlane.Plane.Position, refPlane.Plane.Normal);
                var p = e.GetPosition(viewport.Viewport);
                //var p2Ray = HT.Viewport3DHelper.GetRay(viewport.Viewport, p);
                var p2 = HT.Viewport3DHelper.UnProject(viewport.Viewport, p, refPlane.Plane.Position, refPlane.Plane.Normal);
                var line2d = new Line2D(sketch,p1.Value,p2.Value);
                sketch.Entities.Add(line2d);
                lastClick = null;
                viewport.Viewport.MouseLeftButtonUp -= AddLine_MouseLeftButtonUp;
                activeControl.IsChecked = false; activeControl = null;
            }
        }

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            activeControl = sender as ToggleButton;
            viewport.Viewport.MouseLeftButtonUp += AddLine_MouseLeftButtonUp;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            activeControl = sender as ToggleButton;
            viewport.Viewport.MouseLeftButtonUp += Select_MouseLeftButtonUp;
        }

        private void Select_Unclick(object sender, RoutedEventArgs e)
        {
            viewport.Viewport.MouseLeftButtonUp -= Select_MouseLeftButtonUp;
            if (activeControl != null)
            {
                activeControl.IsChecked = false;
                activeControl = null;
            }
        }

        ToggleButton activeControl = null;
        private void Select_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
                var hits = HT.Viewport3DHelper.FindHits(viewport.Viewport, e.GetPosition(viewport.Viewport));
                if (hits.Count > 0)
                {
                    //var ent = hits.OfType<o3DLib.Sketching.Entity2D>().First<o3DLib.Sketching.Entity2D>();
                    IRelatable ent = null;
                    foreach (var hit in hits)
                        if (this.filter.Text == "Line" && hit.Visual is Line2D || this.filter.Text == "Point" && hit.Visual is Point2D)
                        {
                            ent = hit.Visual as IRelatable;
                            break;
                        }
                    if (ent != null)
                    {
                        if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                            this.Selected.NotIdioticClear<IRelatable>();
                        this.Selected.Add(ent);
                    }
                    else
                        Selected.NotIdioticClear<IRelatable>();
                }
                else
                    Selected.NotIdioticClear<IRelatable>();
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            activeControl = sender as ToggleButton;
            viewport.Viewport.MouseLeftButtonDown += Move_MouseLeftButtonDown;
        }

        private void Move_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewport.Viewport.MouseLeftButtonDown -= Move_MouseLeftButtonDown;
            viewport.Viewport.MouseMove -= Move_MouseMove;
            viewport.Viewport.MouseLeftButtonUp -= Move_MouseLeftButtonUp;
            if (activeControl != null)
            {
                activeControl.IsChecked = false;
                activeControl = null;
            }
        }

        private void Move_MouseMove(object sender, MouseEventArgs e)
        {
            var newPos = e.GetPosition(viewport.Viewport);
            if (moving is Point2D)
            {
                var p = moving as Point2D;
                var p3D = HT.Viewport3DHelper.UnProject(viewport.Viewport, newPos, refPlane.Plane.Position, refPlane.Plane.Normal);
                var move = p.Sketch.RefPlane.GetPoint(p3D.Value) - p.Point;
                p.Move(move.X, move.Y);
                p.Point = p.Sketch.RefPlane.GetPoint(p3D.Value);
            }
            else
            {
                var l = moving as Line2D;
                var l3D = HT.Viewport3DHelper.UnProject(viewport.Viewport, lastPos, refPlane.Plane.Position, refPlane.Plane.Normal);
                var l2D = l.Parent.RefPlane.GetPoint(l3D.Value);
                var p3D = HT.Viewport3DHelper.UnProject(viewport.Viewport, newPos, refPlane.Plane.Position, refPlane.Plane.Normal);
                var p2D = l.Parent.RefPlane.GetPoint(p3D.Value);

                var lp = (p2D - l2D);
                l.Move(lp.X, lp.Y);
            }
            lastPos = newPos;
        }

        private IRelatable moving = null;
        private Point lastPos;

        private void Move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hits = HT.Viewport3DHelper.FindHits(viewport.Viewport, e.GetPosition(viewport.Viewport));
            if (hits.Count > 0)
            {
                IRelatable ent = null;
                foreach (var hit in hits)
                    if (hit.Visual is IRelatable)
                    {
                        ent = hit.Visual as IRelatable;
                        break;
                    }
                if (ent != null)
                {
                    this.moving = ent;
                    lastPos = e.GetPosition(viewport.Viewport);
                    viewport.Viewport.MouseMove += Move_MouseMove;
                    viewport.Viewport.MouseLeftButtonUp += Move_MouseLeftButtonUp;
                }
                else
                {
                    viewport.Viewport.MouseLeftButtonDown -= Move_MouseLeftButtonDown;
                    viewport.Viewport.MouseMove -= Move_MouseMove;
                    viewport.Viewport.MouseLeftButtonUp -= Move_MouseLeftButtonUp;
                }
            }
        }

        private void AddHorizontal_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count == 0) return;
            if (Selected.Count == 1 && Selected[0] is Line2D)
            {
                Line2D l = Selected[0] as Line2D;
                new Horizontal(l);
            }
            else
            {
                var ps = Selected.OfType<Point2D>();
                ps.Last<Point2D>().Relations2D.Add(new Horizontal(ps.ToArray<Point2D>()));
            }
        }

        private void AddParallel_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count != 2) return;
            new Parallel(Selected[0] as Entity2D, Selected[1] as Entity2D);
        }

        private void AddVertical_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count == 0) return;
            if (Selected.Count == 1)
            {
                Line2D l = Selected[0] as Line2D;
                new Vertical(l);
            }
            else
            {
                var ps = Selected.OfType<Point2D>();
                new Vertical(ps.ToArray<Point2D>());
            }
        }

        private void AddPerpendicular_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count != 2) return;
            new Perpendicular(Selected[0] as Entity2D, Selected[1] as Entity2D);
        }

        private void AddCollinear_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count != 2) return;
            new Collinear(Selected[0] as Entity2D, Selected[1] as IRelatable);
        }

        private void AddLock_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count != 1) return;
            new Lock(Selected[0] as IRelatable);
        }

        private void AddConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count != 2) return;
            if (Selected[0] is Point2D)
                new Connect(Selected[0] as Point2D, Selected[1] as Point2D);
            else if (Selected[0] is Entity2D)
                new Connect(Selected[0] as Entity2D, Selected[1] as Point2D);
        }

        private void AddEqual_Click(object sender, RoutedEventArgs e)
        {
            if (Selected.Count != 2) return;
            new Equal(Selected[0] as Entity2D, Selected[1] as Entity2D);
        }
    }
}

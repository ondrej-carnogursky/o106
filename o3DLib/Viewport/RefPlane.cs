using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using o3DLib.Extensions;


namespace o3DLib.Viewport
{
    public class RefPlane: LinesVisual3D
    {
        //todo Visible,Name and/or Parent

        HelixToolkit.Wpf.QuadVisual3D quad = new QuadVisual3D() { BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.DeepSkyBlue) {Opacity=0.1}),Material=null };
        public RefPlane():base()
        {
            Color = Colors.DeepSkyBlue;
            Children.Add(quad);
        }




        public bool ReverseSide
        {
            get { return (bool)GetValue(ReverseSideProperty); }
            set { SetValue(ReverseSideProperty, value); }
        }
        public static readonly DependencyProperty ReverseSideProperty =
            DependencyProperty.Register("ReverseSide", typeof(bool), typeof(RefPlane), new PropertyMetadata(false,OnDPropertyChanged));



        public Vector3D XAxis
        {
            get { return (Vector3D)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }
        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register("XAxis", typeof(Vector3D), typeof(RefPlane), new PropertyMetadata(OnDPropertyChanged));


        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(RefPlane), new PropertyMetadata(500.0, OnDPropertyChanged));

        public Plane3D Plane
        {
            get { return (Plane3D)GetValue(PlaneProperty); }
            set { SetValue(PlaneProperty, value); }
        }
        //todo Zabezpecit binding
        public static readonly DependencyProperty PlaneProperty =
            DependencyProperty.Register("Plane", typeof(Plane3D), typeof(RefPlane), new PropertyMetadata(null,OnDPropertyChanged));

        private static void OnDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var refPlane = d as RefPlane;
            if (refPlane.Plane == null) return;
            var center = refPlane.Plane.Position;
            var normal = refPlane.ReverseSide ? -refPlane.Plane.Normal : refPlane.Plane.Normal;
            var xVector = refPlane.ReverseSide ? -refPlane.XAxis : refPlane.XAxis; //refPlane.Plane.Normal.FindAnyPerpendicular();
            var yVector = Vector3D.CrossProduct(xVector, normal);
            var poly3D = new Polygon3D(new List<Point3D>() { center - refPlane.Size * (xVector - yVector) / 2,
                center - refPlane.Size * (xVector + yVector) / 2, center + refPlane.Size * (xVector - yVector) / 2,
                center + refPlane.Size * (xVector + yVector) / 2 });
            var points = new List<Point3D>() { poly3D.Points[0] };
            for (int i = 1; i < 7; i++)
                points.Add(!i.IsEven() ? poly3D.Points[(i + 1) / 2] : points[i - 1]);
            points.Add(poly3D.Points[0]);
            points.AddMany(poly3D.Points[0] - refPlane.Size / 16 * yVector, poly3D.Points[0] - refPlane.Size / 16 * yVector + refPlane.Size / 4 * xVector);
            points.AddMany(points.Last<Point3D>(), poly3D.Points[0] + refPlane.Size / 4 * xVector);
            refPlane.Points = points;
            //todo Doplnit Fill
            refPlane.quad.Point1 = poly3D.Points[0];
            refPlane.quad.Point2 = poly3D.Points[1];
            refPlane.quad.Point3 = poly3D.Points[2];
            refPlane.quad.Point4 = poly3D.Points[3];
        }



    }
}
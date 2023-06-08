using DevExpress.Mvvm;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nav_View_Airplanes.ViewModels
{
    public class mMainPageViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public GMapControl gMapControl { get; set; }

        public mMainPageViewModel(PageService pageService)
        {
            _pageService = pageService;
            gMapControl = new GMapControl();
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 17;
            gMapControl.Zoom = 4;
            gMapControl.ShowCenter = false;
            gMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gMapControl.CanDragMap = true;
            gMapControl.ClipToBounds= true;

            gMapControl.DragButton = MouseButton.Left;
            gMapControl.SetPositionByKeywords("Russia");


            //// Установите позицию центра карты на Россию
            //PointLatLng center = new PointLatLng(55.7558, 37.6173); // Координаты центра России
            //gMapControl.Position = center;

            //// Создайте полигон, который охватывает только Россию
            //List<PointLatLng> points = new List<PointLatLng>();
            //points.Add(new PointLatLng(58.525698, 29.900456)); // Верхний левый угол
            //points.Add(new PointLatLng(58.525698, 166.301865)); // Верхний правый угол
            //points.Add(new PointLatLng(41.186737, 166.301865)); // Нижний правый угол
            //points.Add(new PointLatLng(41.186737, 29.900456)); // Нижний левый угол
            //GMapPolygon polygon = new GMapPolygon(points );

            // Создаем полигон, охватывающий только Россию, и добавляем его на карту
            GMapPolygon russiaPolygon = new GMapPolygon(
                new List<PointLatLng>
                {
                    new PointLatLng(82.968537, 18.666245),
                    new PointLatLng(82.968537, 179.999977),
                    new PointLatLng(41.185471, 179.999977),
                    new PointLatLng(41.185471, 18.666245)
                } 
            );
            gMapControl.MouseDoubleClick += Click;
            //gMapControl.EmptyMapBackground = new SolidColorBrush(Color.FromRgb(170, 211, 223));
        }
        void Click(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(gMapControl);
            MessageBox.Show(clickPoint.X.ToString() + " " + clickPoint.Y.ToString());
            PointLatLng point = gMapControl.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
            GMapMarker marker = new GMapMarker(point);
            gMapControl.Markers.Add(marker);
        }
    }
}

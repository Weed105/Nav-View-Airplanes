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
using RestSharp;
using System.Net.Http;
using System.Windows.Threading;
using System.Net.Http.Headers;
using System.Net;
using System.Threading;
using static GMap.NET.Entity.OpenStreetMapRouteEntity;
using Nav_View_Airplanes.Models;

namespace Nav_View_Airplanes.ViewModels
{
    public class mMainPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly GetService _getService;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public GMapControl gMapControl { get; set; }
        public GMapMarker Marker { get; set; }
        public List<Airport> Airports { get; set; }

        double lat = 0;
        double lng = 0;
        double stepLat = 0;
        double stepLng = 0;

        int i = 0;
        double finalLat = 55.2522;
        double finalLng = 37.6156;

        public mMainPageViewModel(PageService pageService, GetService getService)
        {
            _pageService = pageService;
            _getService = getService;

            gMapControl = new GMapControl();
            GMap.NET.GMaps.Instance.Mode = AccessMode.ServerAndCache; 
            gMapControl.MapProvider = OpenStreetMapProvider.Instance;
            gMapControl.MinZoom = 2;
            gMapControl.MaxZoom = 17;
            PointLatLng russiaCenter = new PointLatLng(60.0, 100.0);
            gMapControl.Position = russiaCenter;
            gMapControl.Zoom = 4;
            gMapControl.ShowCenter = false;
            gMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gMapControl.CanDragMap = true;
            gMapControl.ClipToBounds = false;
            gMapControl.DragButton = MouseButton.Left;
            gMapControl.SetPositionByKeywords("Russia");
            gMapControl.EmptyMapBackground = new SolidColorBrush(Color.FromRgb(170, 211, 223));
            gMapControl.MouseDoubleClick += Click;

            AddPlane(54.0529, 55.8860); // +0.2 : -0.4

            //var response = GetCall();
            //if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    string result = response.Result.Content.ReadAsStringAsync().Result;
            //    MessageBox.Show(result);
            //}

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            lat = Marker.Position.Lat;
            lng = Marker.Position.Lng;
            stepLat = (finalLat - lat) / 100;
            stepLng = (finalLng - lng) / 100;

            LoadAirports();
        }
        public async void LoadAirports()
        {
            if (gMapControl.Markers.Count == 1)
            {
                var airportsDb = await _getService.GetAirports();
                Airports = airportsDb;
                for (int i = 0; i < Airports.Count; i++)
                {
                    GMapMarker marker = new GMapMarker(new PointLatLng(Airports[i].X, Airports[i].Y));
                    marker.Shape = new Rectangle()
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.OrangeRed,
                        RenderTransform = new RotateTransform(45),
                    };
                    gMapControl.Markers.Add(marker);
                }
            }
            
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {            
            //gMapControl.Markers[gMapControl.Markers.Count - 1].Position = new PointLatLng(55.7522, 37.6156);

            

            if (i < 100 )
            {
                lat += stepLat;
                lng += stepLng;
                Marker.Position = new PointLatLng(lat, lng);
            }
            i += 1;
        }
        void Click(object sender, MouseButtonEventArgs e)
        {

            //Point point = e.GetPosition(e.Source as FrameworkElement);
            //MessageBox.Show(gMapControl.FromLocalToLatLng((int)point.X, (int)point.Y).Lat.ToString() + " " + gMapControl.FromLocalToLatLng((int)point.X, (int)point.Y).Lng.ToString());

            //gMapControl.Markers[gMapControl.Markers.Count - 1].Position = new PointLatLng(54.5529, 55.8860);
            //double lat = Marker.Position.Lat;
            //double lng = Marker.Position.Lng;


            //double finalLat = 55.7522;
            //double finalLng = 37.6156;

            //double stepLat = (finalLat - lat) / 10;
            //double stepLng = (finalLng - lng) / 10;

            //for (int i = 0; i < 10; i++)
            //{
            //    lat += stepLat;
            //    lng += stepLng;
            //    Marker.Position = new PointLatLng(lat, lng);
            //    gMapControl.Markers[gMapControl.Markers.Count - 1].Position = new PointLatLng(lat, lng);
            //    Thread.Sleep(100);
            //}

        }

        private void AddPlane(double lat, double lng)
        {
            //AddMarker(55.7522, 37.6156);
            //AddMarker(54.5529, 55.8860);

            double rad = Math.Atan((55.8860 - 37.6156) / (54.5529 - 55.7522)) * (180 / Math.PI);

            Marker = new GMapMarker(new PointLatLng(lat, lng));
            Marker.Shape = new Image()
            {
                Source = new BitmapImage(new Uri("../../../Resources/plane.png", UriKind.Relative)),
                Width = 22,
                Height = 22,
                RenderTransform = new RotateTransform(rad),
            };
            gMapControl.Markers.Add(Marker);            
        }
        //public static Task<HttpResponseMessage> GetCall()
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        string apiUrl = "https://opensky-network.org/api/flights/all?begin=1686672129&end=1686758529";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.Timeout = TimeSpan.FromSeconds(900);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = client.GetAsync(apiUrl);
        //            response.Wait();
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
// Гениальные мысли
// 15.06 Было добвалено изменение положения самолета за 10 секунд от А до Б
// * Если будет задаваться время отбытия и прибития то можно будет брать их разницу и использовать как точки карте (гениально!)
// * Не забудь исправить бд для правильного использования 

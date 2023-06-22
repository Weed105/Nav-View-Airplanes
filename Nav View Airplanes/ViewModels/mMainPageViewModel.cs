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

        public string Login { get; set; }
        public string Password { get; set; }
        public string TextButton { get; set; } = "Войти";
        public string ErrorMessageButton { get; set; }

        private readonly PageService _pageService;
        private readonly UserService _userService;
        private readonly GetService _getService;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public GMapControl gMapControl { get; set; }
        public GMapMarker Marker { get; set; }
        public List<Airport> Airports { get; set; }
        public Visibility Visibility { get; set; } = Visibility.Collapsed;
        public Visibility ButtonVisibility { get; set; } = Visibility.Collapsed;
        public List<Flight> Flights { get; set; }
        public List<(GMapMarker, double, double, double, Flight)> Vehicles { get; set; } = new();
        public mMainPageViewModel(PageService pageService, GetService getService, UserService userService)
        {
            _pageService = pageService;
            _getService = getService;
            _userService = userService;

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
            gMapControl.MouseDown += HideAuth;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            LoadAirports();
            LoadFlights();
        }
        public void HideAuth(object sender, MouseEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
        public async void LoadAirports()
        {
            if (gMapControl.Markers.Count == 0)
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
        public async void LoadFlights()
        {
                var flightsDb = await _getService.GetFlights();
                Flights = flightsDb;
                for (int i = 0; i< Flights.Count; i++)
                {
                    if (Flights[i].Status == 1)
                    {
                        if (DateTime.Now > Flights[i].DepartureTime && DateTime.Now < Flights[i].ArrivalTime)
                        {
                            double steps = (Flights[i].ArrivalTime - Flights[i].DepartureTime).TotalSeconds;
                            double step = (DateTime.Now - Flights[i].DepartureTime).TotalSeconds;

                            double step1 = (Flights[i].ArrivalAirportNavigation.X - Flights[i].DepartureAirportNavigation.X) / steps;
                            double step2 = (Flights[i].ArrivalAirportNavigation.Y - Flights[i].DepartureAirportNavigation.Y) / steps;

                            double lat = Flights[i].DepartureAirportNavigation.X + (step1 * step);
                            double lng = Flights[i].DepartureAirportNavigation.Y + (step2 * step);
                            AddPlane2(lat, lng, step1, step2, steps, Flights[i]);
                        }
                    }
                }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Vehicles.Count; i++)
            {
                if (DateTime.Now <= Vehicles[i].Item5.ArrivalTime)
                {
                    Vehicles[i].Item1.Position = new PointLatLng(Vehicles[i].Item1.Position.Lat + Vehicles[i].Item2, Vehicles[i].Item1.Position.Lng + Vehicles[i].Item3);
                }
            }
        }

        private void AddPlane2(double lat, double lng, double step1, double step2, double steps, Flight flight)
        {
            GMapMarker marker = new GMapMarker(new PointLatLng(lat, lng));
            double rad = Math.Atan((flight.ArrivalAirportNavigation.Y - flight.DepartureAirportNavigation.Y) / (flight.ArrivalAirportNavigation.X - flight.DepartureAirportNavigation.X)) * (180 / Math.PI);
            marker.Shape = new Image()
            {
                Source = new BitmapImage(new Uri("../../../Resources/plane.png", UriKind.Relative)),
                Width = 22,
                Height = 22,
                RenderTransform = new RotateTransform(rad),
            };
            gMapControl.Markers.Add(marker);
            Vehicles.Add((marker, step1, step2, steps, flight));
        }
        public DelegateCommand ViewAuth => new(() =>
        {
            if (!TextButton.Equals("Выйти"))
                Visibility = Visibility.Visible;
            else
            {
                ButtonVisibility = Visibility.Collapsed;
                Global.CurrentUser = null;
                TextButton = "Войти";
            }
        });

        public AsyncCommand SignInCommand => new(async () =>
        { 
                await Task.Run(async () =>
                {
                    if (await _userService.AuthorizationAsync(Login, Password))
                    {
                        ErrorMessageButton = string.Empty;
                        Visibility = Visibility.Collapsed;
                        TextButton = "Выйти";
                        Login = "";
                        Password = "";
                        ButtonVisibility = Visibility.Visible;
                        MessageBox.Show("Вы авторизовались!");
                    }
                    else
                    {
                        ErrorMessageButton = "Неверный логин или пароль";
                    }
                });

        });
    }
}
// Гениальные мысли
// 15.06 Было добвалено изменение положения самолета за 10 секунд от А до Б
// * Если будет задаваться время отбытия и прибития то можно будет брать их разницу и использовать как точки карте (гениально!)
// * Не забудь исправить бд для правильного использования 

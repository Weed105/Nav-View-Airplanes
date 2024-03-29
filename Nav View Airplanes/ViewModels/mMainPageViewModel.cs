﻿using DevExpress.Mvvm;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Nav_View_Airplanes.Models;
using System.Windows.Media.Media3D;

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
        public Visibility AdminVisibility { get; set; } = Visibility.Collapsed;
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
            if (Global.CurrentUser != null && Global.CurrentUser.Indicator == 1)
            {
                ErrorMessageButton = string.Empty;
                Visibility = Visibility.Collapsed;
                TextButton = "Выйти";
                Login = "";
                Password = "";
                ButtonVisibility = Visibility.Visible;
            }
            if (Global.CurrentUser != null && Global.CurrentUser.Indicator == 0)
            {
                ErrorMessageButton = string.Empty;
                Visibility = Visibility.Collapsed;
                TextButton = "Выйти";
                Login = "";
                Password = "";
                AdminVisibility = Visibility.Visible;
            }
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
                        ToolTip = Airports[i].City,
                        RenderTransform = new RotateTransform(45),
                    };

                    gMapControl.OnSelectionChange += (sender, e) =>
                    {
                        MessageBox.Show(sender.Lng.ToString());
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
                if (DateTime.Now > Flights[i].DepartureTime && DateTime.Now < Flights[i].ArrivalTime)
                {
                    double steps = (Flights[i].ArrivalTime - Flights[i].DepartureTime).TotalSeconds;
                    double step = (DateTime.Now - Flights[i].DepartureTime).TotalSeconds;

                    double step1 = (Flights[i].ArrivalAirportNavigation.X - Flights[i].DepartureAirportNavigation.X) / steps;
                    double step2 = (Flights[i].ArrivalAirportNavigation.Y - Flights[i].DepartureAirportNavigation.Y) / steps;

                    double lat = Flights[i].DepartureAirportNavigation.X + (step1 * step);
                    double lng = Flights[i].DepartureAirportNavigation.Y + (step2 * step);
                    AddPlane2(lat, lng, step1, step2, steps, Flights[i]);
                    Flights[i].Status = 2;
                    _getService.ChangeState(Flights[i]);
                }
                if (DateTime.Now < Flights[i].DepartureTime)
                {
                    Flights[i].Status = 3;
                    _getService.ChangeState(Flights[i]);
                }
                if (DateTime.Now >  Flights[i].ArrivalTime)
                {
                    Flights[i].Status = 1;
                    _getService.ChangeState(Flights[i]);
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
                else
                {
                    Vehicles[i].Item5.Status = 1;
                    _getService.ChangeState(Vehicles[i].Item5);
                }
            }
        }
        private void AddPlane2(double lat, double lng, double step1, double step2, double steps, Flight flight)
        {
            GMapMarker marker = new GMapMarker(new PointLatLng(lat, lng));
            double rad = Math.Atan((flight.ArrivalAirportNavigation.Y - flight.DepartureAirportNavigation.Y) / (flight.ArrivalAirportNavigation.X - flight.DepartureAirportNavigation.X)) * (180 / Math.PI);
            if (flight.ArrivalAirportNavigation.Y - flight.DepartureAirportNavigation.Y  < 0 || flight.ArrivalAirportNavigation.X - flight.DepartureAirportNavigation.X < 0)
            {
                rad = Math.Abs(rad);
            }
            marker.Shape = new Image()
            {
                Source = new BitmapImage(new Uri("../../../Resources/plane.png", UriKind.Relative)),
                Width = 22,
                Height = 22,
                ToolTip = $"Самолет:\n    {flight.IdplaneNavigation.Model}\nОтбытие:\n    {flight.DepartureAirportNavigation.City}\n    {flight.DepartureTime.ToString("g")}\nПрибытие:\n    {flight.ArrivalAirportNavigation.City}\n    {flight.ArrivalTime.ToString("g")}",
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
                AdminVisibility = Visibility.Collapsed;
                Global.CurrentUser = null;
                TextButton = "Войти";
            }
        });
        public DelegateCommand ListlFlights => new(() => _pageService.ChangePage(new FlightsPage()));
        public DelegateCommand SignInFlight => new(() => _pageService.ChangePage(new DispatcherPage()));
        public DelegateCommand SignInDisp => new(() => _pageService.ChangePage(new PersonPage()));
        public DelegateCommand SignInPlane => new(() => _pageService.ChangePage(new PlanePage()));
        public AsyncCommand SignInCommand => new(async () =>
        { 
                await Task.Run(async () =>
                {
                    if (await _userService.AuthorizationAsync(Login, Password))
                    {
                        if (Global.CurrentUser.Indicator == 1)
                            ButtonVisibility = Visibility.Visible;
                        else
                            AdminVisibility = Visibility.Visible;
                        ErrorMessageButton = string.Empty;
                        Visibility = Visibility.Collapsed;
                        TextButton = "Выйти";
                        Login = "";
                        Password = "";
                    }
                    else
                    {
                        ErrorMessageButton = "Неверный логин или пароль";
                    }
                });
        });
    }
}
using DevExpress.Mvvm;
using Nav_View_Airplanes.Models;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nav_View_Airplanes.ViewModels
{
    public class mDispatcherViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly GetService _getService;
        public List<Flight> Flights { get; set; }
        public Flight SelectedFlight { get; set; }
        bool changing = false;
        public Visibility ButtonsVisible { get; set; } = Visibility.Visible;
        public Visibility ChangeVisible { get; set; } = Visibility.Collapsed;
        public DateTime Now { get; set; } = DateTime.Now.AddDays(1);
        public List<string> Departuries { get; set; } = new();
        public List<string> Arrivals { get; set; } = new();
        public List<string> Planes { get; set; } = new();
        public List<Airport> AirportsDb { get; set; } = new();
        public List<Plane> PlanesDb { get; set; } = new();



        public string TextButton { get; set; }
        public string SelectedDeparture { get; set; }
        public string SelectedArrival { get; set; }
        public string SelectedPlane { get; set; }
        public string SelectedDate { get; set; }
        public string SelectedTime { get; set; }



        public mDispatcherViewModel(PageService pageService, GetService getService)
        {
            _pageService = pageService;
            _getService = getService;
            Load();
        }
        public async void Load()
        {
            var context = await _getService.GetFlights();
            var air_context = await _getService.GetAirports();
            var plane_context = await _getService.GetPlanes();
            Flights = context;
            if (Departuries.Count == 0)
            {

                foreach (var air in air_context)
                {
                    Departuries.Add(air.City);
                    Arrivals.Add(air.City);
                    AirportsDb.Add(air);
                }
                foreach (var plane in plane_context)
                {
                    Planes.Add(plane.Model);
                    PlanesDb.Add(plane);
                }
            }
        }

        public DelegateCommand AddFlight => new(() =>
        {
            ButtonsVisible = Visibility.Collapsed;
            ChangeVisible = Visibility.Visible;
            TextButton = "Добавить";
        });
        public DelegateCommand ChangeFlight => new (() => 
        {
            if (SelectedFlight != null && SelectedFlight.Status == 3)
            {
                TextButton = "Изменить";
                ButtonsVisible = Visibility.Collapsed;
                ChangeVisible = Visibility.Visible;
                changing = true;
                SelectedDeparture = AirportsDb.Where(i => i.Idairport == SelectedFlight.DepartureAirport).SingleOrDefault().City;
                SelectedArrival = AirportsDb.Where(i => i.Idairport == SelectedFlight.ArrivalAirport).SingleOrDefault().City;
                SelectedPlane = PlanesDb.Where(i => i.Idplane == SelectedFlight.Idplane).SingleOrDefault().Model;
                SelectedDate = SelectedFlight.DepartureTime.ToString("yyyy-MM-dd");
                SelectedTime = SelectedFlight.DepartureTime.TimeOfDay.ToString();
            }
            else
            {
                changing = false;
                MessageBox.Show("Выберите забронированный рейс для изменения", "Внимание");
            }
        });
        public DelegateCommand DeleteFlight => new(() =>
        {
            if (SelectedFlight != null && SelectedFlight.Status == 3)
            {
                _getService.DeleteFlight(SelectedFlight);
                MessageBox.Show("Рейс удален", "Сообщение");
                Load();
            }
            else
                MessageBox.Show("Выберите забронированный рейс для изменения", "Внимание");

        });
        public DelegateCommand HideChange=> new(() =>
        {
            ButtonsVisible = Visibility.Visible;
            ChangeVisible = Visibility.Collapsed;
            SelectedDeparture = null;
            SelectedArrival = null;
            SelectedPlane = null;
            SelectedDate = string.Empty;
            SelectedTime = string.Empty;

        });

        public DelegateCommand AddOrChange => new(() =>
        {

                if (SelectedDate  != null && SelectedTime != null && SelectedArrival != null && SelectedDeparture != null && SelectedPlane !=null)
                {
                    DateTime dateTime = DateTime.Parse(SelectedDate, System.Globalization.CultureInfo.InvariantCulture);
                    DateTime time = DateTime.Parse(SelectedTime, System.Globalization.CultureInfo.InvariantCulture);
                    dateTime = dateTime.AddHours(time.Hour).AddMinutes(time.Minute);
                    Flight flight = new Flight
                    {
                        Idflight = changing ? SelectedFlight.Idflight : Flights.Max(i => i.Idflight) + 1,
                        DepartureAirport = AirportsDb.Where(i => i.City.Equals(SelectedDeparture)).SingleOrDefault().Idairport,
                        ArrivalAirport = AirportsDb.Where(i => i.City.Equals(SelectedArrival)).SingleOrDefault().Idairport,
                        Idplane = PlanesDb.Where(i => i.Model.Equals(SelectedPlane)).SingleOrDefault().Idplane,
                        DepartureTime = dateTime,
                        ArrivalTime = dateTime.AddHours(1),
                        Status = 3,
                    };
                if (!changing)
                {
                    _getService.AddFlight(flight);
                    MessageBox.Show("Рейс добавлен", "Сообщение");
                    Load();
                }
                else
                {
                    _getService.ChangeFlight(flight);
                    MessageBox.Show("Рейс изменен", "Сообщение");
                    Load();
                }
                }

        });
        public DelegateCommand Back => new(() => _pageService.ChangePage(new MainPage()));

    }
}

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
using System.Windows.Media.Media3D;

namespace Nav_View_Airplanes.ViewModels
{
    public class mPlaneViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly GetService _getService;
        public List<Plane> Planes { get; set; }
        public Plane SelectedPlane { get; set; }
        public List<string> Departuries { get; set; } = new();
        public List<Airport> Airports { get; set; } = new();
        public string SelectedDeparture { get; set; }
        public string NameModel { get; set; }


        public mPlaneViewModel (PageService pageService, GetService getService)
        {
            _pageService = pageService;
            _getService = getService;
            Load();
        }
        public async void Load()
        {
            var context = await _getService.GetPlanes();
            var departuries = await _getService.GetAirports();
            Airports = departuries;
            Planes = context;
            if (Departuries.Count == 0)
                foreach (var depart in departuries)
                    Departuries.Add(depart.City);
        }
        public DelegateCommand SelectPlane => new(() =>
        {
            if (SelectedPlane != null)
            {
                NameModel = SelectedPlane.Model;
                SelectedDeparture = Airports.Where(i => i.Idairport == SelectedPlane.Idairport).SingleOrDefault().City;
            }
        });
        public DelegateCommand AddPlane=> new(() =>
        {
            Plane plane = new Plane
            {
                Idplane = Planes.Max(i => i.Idplane) + 1,
                Model = NameModel,
                Idairport = Airports.Where(i => i.City.Equals(SelectedDeparture)).SingleOrDefault().Idairport,
            };

            _getService.AddPlane(plane);
            MessageBox.Show("Самолет добавлен", "Сообщение");
            Load();
        });
        public DelegateCommand ChangePlane=> new(() =>
        {
            if (SelectedPlane != null)
            {
                Plane plane = new Plane
                {
                    Idplane = SelectedPlane.Idplane,
                    Model = NameModel,
                    Idairport = Airports.Where(i => i.City.Equals(SelectedDeparture)).SingleOrDefault().Idairport,
                };

                _getService.ChangePlane(plane);
                MessageBox.Show("Самолет изменен", "Сообщение");
                Load();
                SelectedPlane = null;
            }
            else
            {
                MessageBox.Show("Выберите самолет для изменения", "Внимание");
            }
        });

        public DelegateCommand DeletePlane=> new(() =>
        {
            if (SelectedPlane != null)
            {
                _getService.DeletePlane(SelectedPlane);
                MessageBox.Show("Самолет удален", "Сообщение");
                Load();
            }
            else
                MessageBox.Show("Выберите самолет для удаления", "Внимание");

        });
        public DelegateCommand Back => new(() => _pageService.ChangePage(new MainPage()));

    }
}

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
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RestSharp;
using System.Net.Http;

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


            //Для учета выполнения рейсов
            //В бд добавить маршрут(замена рейса) и расписание                          !!!
            //Статус рейса



            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://airlabs.co/api/v9/ping?api_key=c63254e2-19ab-442f-b090-8b0b2e63d4a1");
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
            //MessageBox.Show(response.Content.ToString());

            //var client = new RestClient("http://airlabs.co/api/v9/ping?api_key=c63254e2-19ab-442f-b090-8b0b2e63d4a1");
        }
    }
}
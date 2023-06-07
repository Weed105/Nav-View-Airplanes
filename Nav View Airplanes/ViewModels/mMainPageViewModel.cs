using DevExpress.Mvvm;
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

            //gMapControl.EmptyMapBackground = new SolidColorBrush(Color.FromRgb(170, 211, 223));
            gMapControl.EmptyTileBrush = new SolidColorBrush(Colors.Gainsboro);
            gMapControl.EmptyTileBorders = new Pen(new SolidColorBrush(Colors.Gray), 2);
        }
    }
}

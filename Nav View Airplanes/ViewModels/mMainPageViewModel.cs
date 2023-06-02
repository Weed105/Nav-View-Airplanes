using DevExpress.Mvvm;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nav_View_Airplanes.ViewModels
{
    public class mMainPageViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public mMainPageViewModel(PageService pageService)
        {
            _pageService = pageService;
        }


    }
}

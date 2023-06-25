using DevExpress.Mvvm;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nav_View_Airplanes.ViewModels
{
    public class mWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public Page  PageSource { get; set; }

        public mWindowViewModel(PageService pageService)
        {
            _pageService = pageService;
            _pageService.onPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new PersonPage());
        }
    }
}

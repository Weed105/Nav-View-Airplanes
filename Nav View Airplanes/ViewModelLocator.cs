using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.ViewModels;

namespace Nav_View_Airplanes
{
    public class ViewModelLocator
    {
        public static ServiceProvider provider;

        public static void Init()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<mWindowViewModel>();
            services.AddTransient<mMainPageViewModel>();

            services.AddSingleton<PageService>();

            provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                provider.GetRequiredService(service.ServiceType);
            }
        }
        public mWindowViewModel mWindowViewModel => provider.GetRequiredService<mWindowViewModel>();
        public mMainPageViewModel mMainPageViewModel => provider.GetRequiredService<mMainPageViewModel>();

    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nav_View_Airplanes.Models;
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
            services.AddTransient<mDispatcherViewModel>();
            services.AddTransient<mPersonViewModel>();
            services.AddTransient<mPlaneViewModel>();

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<GetService>();

            services.AddDbContext<FlightContext>();
            provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                provider.GetRequiredService(service.ServiceType);
            }

        }
        public mWindowViewModel mWindowViewModel => provider.GetRequiredService<mWindowViewModel>();
        public mMainPageViewModel mMainPageViewModel => provider.GetRequiredService<mMainPageViewModel>();
        public mDispatcherViewModel mDispatcherViewModel => provider.GetRequiredService<mDispatcherViewModel>();
        public mPersonViewModel mPersonViewModel => provider.GetRequiredService<mPersonViewModel>();
        public mPlaneViewModel mPlaneViewModel => provider.GetRequiredService<mPlaneViewModel>();
    }
}

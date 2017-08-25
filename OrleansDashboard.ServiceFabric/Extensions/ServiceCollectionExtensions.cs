using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDashboard.ServiceFabric.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDashboard(this IServiceCollection services)
        {
            services.AddSingleton(TaskScheduler.Current)
                        .AddSingleton(providerRuntime)
                        .AddSingleton(dashboardTraceListener);
            services.AddMvcCore()
                    .AddApplicationPart(typeof(DashboardController).GetTypeInfo().Assembly)
                    .AddJsonFormatters();
        }
    }
}

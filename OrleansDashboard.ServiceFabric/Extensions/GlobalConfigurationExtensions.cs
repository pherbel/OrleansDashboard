using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDashboard.ServiceFabric.Extensions
{
    public static class GlobalConfigurationExtensions
    {
        public static void RegisterDashboard(this GlobalConfiguration config)
        {

            config.RegisterBootstrapProvider<DashboardBootstrapProviderForServiceFabric>("Dashboard");

            config.RegisterStatisticsProvider<StatsPublisher>("DashboardStats");
        }

    }
}

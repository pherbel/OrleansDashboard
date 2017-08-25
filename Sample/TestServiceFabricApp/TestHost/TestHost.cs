using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime;
using TestHost.Logger;
using Microsoft.Orleans.ServiceFabric;
using OrleansDashboard.ServiceFabric;

namespace TestHost
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class TestHost : StatelessService
    {
        public TestHost(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new WebListenerCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting WebListener on {url}");

                        return new WebHostBuilder()
                                    .UseWebListener()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseApplicationInsights()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    })),
                OrleansServiceListener.CreateStateless(this.GetClusterConfiguration()),
                OrleansDashboardServiceListener.CreateStateless<DashboardStartup>(null,Context.ServiceName)
            };
        }


        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)

        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            }

        }


        public ClusterConfiguration GetClusterConfiguration()

        {

            var config = new ClusterConfiguration();
            config.UseStartupType<SiloStartup>();
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.ReminderTableGrain;

            config.Globals.DataConnectionString = "UseDevelopmentStorage=true";

            //    config.Globals.RegisterBootstrapProvider<BootstrapProvider>("booter");


            LogManager.LogConsumers.Add(new EventSourceLogger());

            return config;

        }
    }
}

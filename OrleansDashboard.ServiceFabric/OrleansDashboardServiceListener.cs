using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Orleans.ServiceFabric;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDashboard.ServiceFabric
{
    /// <summary>
    /// Creates Service Fabric listeners for Orleans Dashboard.
    /// </summary>
    public static class OrleansDashboardServiceListener
    {
        /// <summary>
        /// The Service Fabric endpoint name used by Orleans silos.
        /// </summary>

        internal const string OrleansServiceFabricEndpointName = "OrleansDashboard";

        public static ServiceInstanceListener CreateStateless<TStartup>(ClusterConfiguration configuration, Uri serviceFabricOrleansHostServiceName) where TStartup : class
        {
            return new ServiceInstanceListener(serviceContext =>
                 new WebListenerCommunicationListener(serviceContext, OrleansServiceFabricEndpointName, (url, listener) =>
                 {
                     return new WebHostBuilder()
                                 .UseWebListener()
                                 .ConfigureServices(
                                     services => services
                                         .AddSingleton<StatelessServiceContext>(serviceContext))
                                         .ConfigureServices(services => services.AddSingleton<IClusterClient>(sp =>
                                         {
                                             IHostingEnvironment env = sp.GetRequiredService<IHostingEnvironment>();
                                             IClusterClient client = new ClientBuilder()
                                                     .UseConfiguration(GetClientConfiguration(env, sp))

                                                     .AddServiceFabric(serviceFabricOrleansHostServiceName)
                                                     .ConfigureServices(
                                                         orleansServices =>
                                                         {
                                                             // Some deployments require a custom FabricClient, eg so that cluster endpoints and certificates can be configured.
                                                             // A pre-configured FabricClient can be injected.
                                                             FabricClient fabricClient = new FabricClient();
                                                             orleansServices.AddSingleton(fabricClient);
                                                         })
                                                     .Build();
                                             client.Connect().GetAwaiter().GetResult();

                                             return client;
                                         })
                                         .AddSingleton<IGrainFactory>(sp => sp.GetRequiredService<IClusterClient>()))
                                 .UseContentRoot(Directory.GetCurrentDirectory())
                                 .UseStartup<TStartup>()
                                 .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                 .UseUrls(url)
                                 .Build();
                 }));

        }


        public static ServiceReplicaListener CreateStateful<TStartup>(ClusterConfiguration configuration, Uri serviceFabricOrleansHostServiceName) where TStartup : class
        {

            return new ServiceReplicaListener(serviceContext =>
                 new WebListenerCommunicationListener(serviceContext, OrleansServiceFabricEndpointName, (url, listener) =>
                 {
                     return new WebHostBuilder()
                                 .UseWebListener()
                                 .ConfigureServices(
                                     services => services
                                         .AddSingleton<StatefulServiceContext>(serviceContext))
                                         .ConfigureServices(services => services.AddSingleton<IClusterClient>(sp =>
                                         {
                                             IHostingEnvironment env = sp.GetRequiredService<IHostingEnvironment>();
                                             IClusterClient client = new ClientBuilder()
                                                     .UseConfiguration(GetClientConfiguration(env, sp))

                                                     .AddServiceFabric(serviceFabricOrleansHostServiceName)
                                                     .ConfigureServices(
                                                         orleansServices =>
                                                         {
                                                             // Some deployments require a custom FabricClient, eg so that cluster endpoints and certificates can be configured.
                                                             // A pre-configured FabricClient can be injected.
                                                             FabricClient fabricClient = new FabricClient();
                                                             orleansServices.AddSingleton(fabricClient);
                                                         })
                                                     .Build();
                                             client.Connect().GetAwaiter().GetResult();

                                             return client;
                                         })
                                         .AddSingleton<IGrainFactory>(sp => sp.GetRequiredService<IClusterClient>()))
                                 .UseContentRoot(Directory.GetCurrentDirectory())
                                 .UseStartup<TStartup>()
                                 .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                 .UseUrls(url)
                                 .Build();
                 }));

        }

        private static ClientConfiguration GetClientConfiguration(IHostingEnvironment env, IServiceProvider sp)
        {
            ClientConfiguration clientconfig = new ClientConfiguration();

            return clientconfig;
        }

    }
}

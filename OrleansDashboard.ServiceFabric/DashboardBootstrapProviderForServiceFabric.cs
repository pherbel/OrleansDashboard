using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDashboard.ServiceFabric
{
    public class DashboardBootstrapProviderForServiceFabric : IBootstrapProvider
    {
        public string Name { get; private set; }

        private Logger logger;
        private GrainProfiler profiler;

        private DashboardTraceListener dashboardTraceListener;
        public static TaskScheduler OrleansScheduler { get; private set; }

        public Task Close()
        {
            try
            {
                Trace.Listeners.Remove(dashboardTraceListener);
            }
            catch { }

            try
            {
                profiler?.Dispose();
            }
            catch { }

            OrleansScheduler = null;

            return Task.CompletedTask;
        }

        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            this.Name = name;

            this.logger = providerRuntime.GetLogger("Dashboard");

            this.dashboardTraceListener = new DashboardTraceListener();

            this.profiler = new GrainProfiler(TaskScheduler.Current, providerRuntime);

            IDashboardGrain dashboardGrain = providerRuntime.GrainFactory.GetGrain<IDashboardGrain>(0);
            await dashboardGrain.Init();

            ISiloGrain siloGrain = providerRuntime.GrainFactory.GetGrain<ISiloGrain>(providerRuntime.ToSiloAddress());
            await siloGrain.SetOrleansVersion(typeof(SiloAddress).GetTypeInfo().Assembly.GetName().Version.ToString());
            Trace.Listeners.Add(dashboardTraceListener);

            // horrible hack to grab the scheduler
            // to allow the stats publisher to push
            // counters to grains
            OrleansScheduler = TaskScheduler.Current;
        }
    }
}

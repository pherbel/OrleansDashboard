using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHost
{
    public class CallGeneratorBootstrapProvider : IBootstrapProvider
    {
        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Orleans.Runtime.Logger logger = providerRuntime.GetLogger(nameof(CallGeneratorBootstrapProvider));
            this.Name = name;

            //var grain = providerRuntime.GrainFactory.GetGrain<ICalculatorGrain>(Guid.Empty);
            //Task.Factory.StartNew(
            //    async () =>
            //    {
            //        while (true)
            //        {
            //            try
            //            {
            //                var value = await grain.Add(1);
            //                logger.Info($"{value - 1} + 1 = {value}");
            //                await Task.Delay(TimeSpan.FromSeconds(4));

            //            }
            //            catch (Exception exception)
            //            {
            //                logger.Warn(exception.HResult, "Exception in bootstrap provider. Ignoring.", exception);

            //            }

            //        }

            //    }).Ignore();

            return Task.FromResult(0);

        }



        public Task Close() => Task.FromResult(0);



        public string Name { get; set; }
    }
}

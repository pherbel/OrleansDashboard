using System;
using System.Collections.Generic;
using System.Fabric.Query;
using System.Linq;
using System.Threading.Tasks;

namespace TestHost
{
    public class SiloStartup
    {

        public static StatelessService Service { get; set; }



        public IServiceProvider ConfigureServices(IServiceCollection services)

        {

            services.AddServiceFabricSupport(Service);

            return services.BuildServiceProvider();

        }
    }
}

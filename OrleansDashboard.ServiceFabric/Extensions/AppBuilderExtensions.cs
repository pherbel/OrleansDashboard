using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDashboard.ServiceFabric.Extensions
{
    public static class AppBuilderExtensions
    {

        public static IApplicationBuilder UseDashboard(this IApplicationBuilder app)
        {
           return app.UseMvc();
        }

        public static IApplicationBuilder UseDashboardWithBasicAuth(this IApplicationBuilder app,string username,string password)
        {
            var credentials = new UserCredentials(username, password);
            if (credentials.HasValue())
            {
                // only when usename and password are configured
                // do we inject basicauth middleware in the pipeline
                app.UseMiddleware<BasicAuthMiddleware>(credentials);
            }
            return app.UseDashboard();
        }

    }
}

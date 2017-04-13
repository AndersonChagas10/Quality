using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Web;
using Hangfire.Dashboard;

[assembly: OwinStartup(typeof(SgqSystem.Startup))]

namespace SgqSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var dashboardoptions = new DashboardOptions
            {
                AppPath = VirtualPathUtility.ToAbsolute("~"),
                Authorization = new[] { new CustomAuthorizationHangFireFilter() }
            };
            var joboptions = new BackgroundJobServerOptions { WorkerCount = 5 };
            app.UseHangfireServer(joboptions);
            app.UseHangfireDashboard("/hangfire", dashboardoptions);
            
        }
    }

    public class CustomAuthorizationHangFireFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            //if (HttpContext.Current.User.IsInRole("Admin"))
            //{
                return true;
            //}

            //return false;
        }
    }
}

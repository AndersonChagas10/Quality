using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Web;

[assembly: OwinStartup(typeof(SgqSystem.Startup))]

namespace SgqSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Make `Back to site` link working for subfolder applications
            var options = new DashboardOptions { AppPath = VirtualPathUtility.ToAbsolute("~") };

            app.UseHangfireDashboard();
        }
    }
}

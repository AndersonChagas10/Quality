using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Web;
using Hangfire.Dashboard;
using SgqSystem.Mail;
using DTO;

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
            var joboptions = new BackgroundJobServerOptions { };
            app.UseHangfireServer(joboptions);
            app.UseHangfireDashboard("/hangfire", dashboardoptions);

            //"*/1 * * * *" = 1 minutos.
            if (GlobalConfig.Eua)
            {
                RecurringJob.RemoveIfExists("MailServer");
                RecurringJob.RemoveIfExists("ReProcessJson");
                RecurringJob.AddOrUpdate("MailServer",
                    () => SimpleAsynchronousUSA.Mail(),
                    "*/10 * * * *");
            }
            else if (GlobalConfig.Brasil)
            {

                //RecurringJob.RemoveIfExists("ReProcessJson");
                //RecurringJob.AddOrUpdate("ReProcessJson",
                //    () => SimpleAsynchronous.Reconsolidacao(),
                //    "*/15 * * * *");

                //RecurringJob.RemoveIfExists("MailServer");
                //RecurringJob.AddOrUpdate("MailServer",
                //    () => SimpleAsynchronous.Mail(),
                //    "*/10 * * * *");
            }

            //BackgroundJob.Enqueue(
            //() => Debug.WriteLine(" >>>>>>>>>>>>>>>>>>>>>> TESTE"));

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

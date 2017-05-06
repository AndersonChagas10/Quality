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


            // BackgroundJob.Enqueue(
            //() => SimpleAsynchronous.SendMailFromDeviationSgqApp());

            // BackgroundJob.Enqueue(
            //() => SimpleAsynchronous.ResendProcessJson());

            //"*/1 * * * *" = 1 minutos.
            //RecurringJob.AddOrUpdate(
            //    () => SimpleAsynchronous.ResendProcessJson(),
            //    Cron.Minutely);

            //"*/1 * * * *" = 1 minutos.
            //RecurringJob.AddOrUpdate(
            //    () => SimpleAsynchronous.SendMailFromDeviationSgqApp(),
            //    Cron.Minutely);


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

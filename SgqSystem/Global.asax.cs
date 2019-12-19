using Helper;
using SgqSystem.Mappers;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using DTO;
using System.Globalization;
using System.Threading;
using Jobs;
using SgqSystem.Jobs;
using SgqSystem.Helpers;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using Dominio;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

namespace SgqSystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        private string ScriptFull;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
            SgqServiceBusiness.Mappers.AutoMapperConfig.RegisterMappings();
            DisableApplicationInsightsOnDebug();
            GlobalConfig.VerifyConfig("DefaultConnection");

            #region LOG
            //System.Data.Entity.Database.SetInitializer<Dominio.SgqDbDevEntities>(null);
            //AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            //{
            //    Task.Run(() =>
            //    {
            //        using (var db = new Dominio.SgqDbDevEntities())
            //        {
            //            db.ErrorLog.Add(new Dominio.ErrorLog() { AddDate = DateTime.Now, StackTrace = eventArgs.Exception.ToClient() });
            //            db.SaveChanges();
            //        }
            //    });

            //};

            #endregion


            //DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.RunScriptsSystem;
            Dominio.Seed.Seed.SetDicionario();

            SetGlobalConfigAmbient();

            Dominio.Seed.Seed.SetSeedValues(isEN:GlobalConfig.LanguageEUA, runSetSeed: bool.Parse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.RunScriptsSystem ?? "false"));

            ThreadPool.QueueUserWorkItem(IntegrationJobFactory.ExecuteIntegrationJobFunction);
            ThreadPool.QueueUserWorkItem(CollectionDataJobFactory.ExecuteCollectionDataJobFunction);

            ThreadPool.QueueUserWorkItem(MailJob.SendMailJobFunction);
            ThreadPool.QueueUserWorkItem(MandalaJob.PreencherListaMandala);
            ThreadPool.QueueUserWorkItem(ReProcessJsonJob.ReProcessJsonJobFunction);

            //ThreadPool.QueueUserWorkItem(CollectionJob.ExecuteCollectionJob);        

            //if (GlobalConfig.Brasil)
            //    GlobalConfig.UrlEmailAlertas = System.Configuration.ConfigurationManager.AppSettings["EnderecoEmailAlertaBR" + GlobalConfig.Ambient];
            //else if (GlobalConfig.Eua)
            //    GlobalConfig.UrlEmailAlertas = System.Configuration.ConfigurationManager.AppSettings["EnderecoEmailAlertaEUA" + GlobalConfig.Ambient];
            //else if (GlobalConfig.Ytoara)
            //    GlobalConfig.UrlEmailAlertas = System.Configuration.ConfigurationManager.AppSettings["EnderecoEmailAlertaYTOARA" + GlobalConfig.Ambient];

            #if DEBUG
            //TelemetryConfiguration.Active.DisableTelemetry = true;

            #endif

            if (GlobalConfig.LanguageBrasil)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }
            else if (GlobalConfig.LanguageEUA)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);         

        }

        private static void SetGlobalConfigAmbient()
        {
            
            //GlobalConfig.Ambient = System.Configuration.ConfigurationManager.AppSettings["BuildEm"];
            GlobalConfig.Ambient = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.BuildEm;
            GlobalConfig.Producao = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.Producao == "SIM";

        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Disables the application insights locally.
        /// </summary>
        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            //TelemetryConfiguration.Active.DisableTelemetry = true;
        }


    }

}

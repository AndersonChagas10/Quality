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
using Microsoft.ApplicationInsights.Extensibility;

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

            bool runSetSeed = false;
#if !DEBUG
                runSetSeed = bool.Parse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.RunScriptsSystem ?? "false");
#endif

            Conformity.Domain.Core.Entities.Global.DicionarioEstatico.DicionarioEstaticoHelpers = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers;

            Dominio.Seed.Seed.SetSeedValues(isEN:GlobalConfig.LanguageEUA, runSetSeed: runSetSeed);

#if !DEBUG
            ThreadPool.QueueUserWorkItem(IntegrationJobFactory.ExecuteIntegrationJobFunction);
            ThreadPool.QueueUserWorkItem(CollectionDataJobFactory.ExecuteCollectionDataJobFunction);

            ThreadPool.QueueUserWorkItem(MailJob.SendMailJobFunction);
            ThreadPool.QueueUserWorkItem(MandalaJob.PreencherListaMandala);
            ThreadPool.QueueUserWorkItem(ReProcessJsonJob.ReProcessJsonJobFunction);

            ThreadPool.QueueUserWorkItem(CollectionJob.ExecuteCollectionJob);

            ThreadPool.QueueUserWorkItem(PlanoDeAcaoAlterarStatusJob.ExecutarAlteracoesDeAcoes);
#endif

#if DEBUG
            
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
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }


    }

}

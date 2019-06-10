﻿using Helper;
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

            SetGlobalConfigAmbient();
            ThreadPool.QueueUserWorkItem(IntegrationJobFactory.ExecuteIntegrationJobFunction);
            ThreadPool.QueueUserWorkItem(CollectionDataJobFactory.ExecuteCollectionDataJobFunction);

            ThreadPool.QueueUserWorkItem(MailJob.SendMailJobFunction);
            ThreadPool.QueueUserWorkItem(MandalaJob.PreencherListaMandala);
            ThreadPool.QueueUserWorkItem(ReProcessJsonJob.ReProcessJsonJobFunction);

            ThreadPool.QueueUserWorkItem(CollectionJob.ExecuteCollectionJob);

            var seed = new Seed.Seed();
            seed.SetSeedValues();

            using (var db = new Dominio.SgqDbDevEntities())
            {
                var x = new ExpandoObject() as IDictionary<string,object>;
                foreach (var item in db.ResourcePT.ToList())
                {
                    x.Add(item.Key, item.Value);
                   // Resources.Resource.Add( item.Key, item.Value);
                    //Resources.Resource.GetType().InvokeMember(item.Key,
                    //    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    //    Type.DefaultBinder, Resources.Resource, item.Value);
                }
                Resources.Resource = x;
            }

            

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
            GlobalConfig.Ambient = System.Configuration.ConfigurationManager.AppSettings["BuildEm"];
            GlobalConfig.Producao = System.Configuration.ConfigurationManager.AppSettings["Producao"] == "SIM";

        }

        /// <summary>
        /// Verifica se coluna existe se não ele cria, gera arquivo de scripts em LOCA: ScriptFull, para evitar conflito entre clientes.
        /// EXEMPLO:
        ///  DATA - Responsavel - Breve desc
        ///  VerifyColumnExistsNotExistisThenCreate("CollectionLevel2XParHeaderField", "Sample", "int", "default (null)", "Sample = null");
        /// </summary>
        /// <param name="table">Ex: "ParLevel1"</param>
        /// <param name="colmun">Ex: "IsRecravacao"</param>
        /// <param name="type">Ex: "bit"</param>
        /// <param name="defaultValue">Ex: "default (0)"</param>
        /// <param name="setValue">Ex: "IsRecravacao = 0"</param>
        private void VerifyColumnExistsNotExistisThenCreate(string table, string colmun, string type, string defaultValue, string setValue)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                var sql = string.Empty;
                try
                {

                    sql = string.Format(@"IF COL_LENGTH('{0}','{1}') IS NULL
                        BEGIN
                        /*Column does not exist or caller does not have permission to view the object*/
                        Alter table {0} add {1} {2} {3}
                        EXEC ('update {0} set {4}')
                        END", table, colmun, type, defaultValue, setValue);

                    ScriptFull += sql + "\n\n";
                    db.Database.ExecuteSqlCommand(sql);
                }
                catch (Exception e)
                {
                    new CreateLog(new Exception("Erro ao criar a coluna " + colmun + " para tabela " + table + " em global.asax", e), ControllerAction: sql);
                }
            }
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

using Helper;
using Microsoft.ApplicationInsights.Extensibility;
using SgqSystem.Handlres;
using SgqSystem.Mappers;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Hangfire;
using DTO;
using System.Globalization;
using System.Threading;
using Hangfire.SqlServer;

namespace SgqSystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private string ScriptFull;
        private BackgroundJobServer _backgroundJobServer;

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
            GlobalConfig.VerifyConfig("DbContextSgqEUA");
#if DEBUG
            TelemetryConfiguration.Active.DisableTelemetry = true;
#endif
            var options = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = false,
            };
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("DbContextSgqEUA", options);
            _backgroundJobServer = new BackgroundJobServer();

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

            ScriptFull = string.Empty;
            VerifyColumnExistsNotExistisThenCreate("ParLevel1", "IsRecravacao", "bit", "default (0)", "IsRecravacao = 0");
            VerifyColumnExistsNotExistisThenCreate("ParLevel1", "AllowAddLevel3", "bit", "default (0)", "AllowAddLevel3 = 0");
            VerifyColumnExistsNotExistisThenCreate("ParLevel1", "AllowEditPatternLevel3Task", "bit", "default (0)", "AllowEditPatternLevel3Task = 0");
            VerifyColumnExistsNotExistisThenCreate("ParLevel1", "AllowEditWeightOnLevel3", "bit", "default (0)", "AllowEditWeightOnLevel3 = 0");

            //09/09/2017 CG
            VerifyColumnExistsNotExistisThenCreate("ParRecravacao_Linhas", "ParLevel2_Id", "int", "default null", "ParLevel2_Id = null");
            VerifyColumnExistsNotExistisThenCreate("RecravacaoJson", "isValidated", "bit", "default (0)", "IsValidated = 0");
            VerifyColumnExistsNotExistisThenCreate("RecravacaoJson", "ValidateLockDate", "datetime2(7)", "default null", "ValidateLockDate = null");

            //18 10 2017 CG 
            VerifyColumnExistsNotExistisThenCreate("ParLevel3", "IsPointLess", "bit", "default (1)", "IsPointLess = 1");
            VerifyColumnExistsNotExistisThenCreate("ParLevel3", "AllowNA", "bit", "default (0)", "AllowNA = 0");
        }

        /// <summary>
        /// Verifica se coluna existe se não ele cria, para eveitar conflito entre clientes.
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
            _backgroundJobServer.Dispose();
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

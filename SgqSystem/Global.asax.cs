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

            ThreadPool.QueueUserWorkItem(MailJob.SendMailJobFunction);
            ThreadPool.QueueUserWorkItem(MandalaJob.PreencherListaMandala);
            ThreadPool.QueueUserWorkItem(ReProcessJsonJob.ReProcessJsonJobFunction);

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

            //DicionarioEstatico
            SetDicionarioEstatico();

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

        private static void SetDicionarioEstatico()
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {

                //System.Diagnostics.Debugger.Break();

                var dicionariosKeys = db.DicionarioEstatico.Select(r => r.Key).ToList();

                var DicionariosInserir = new List<DicionarioEstatico>();


                //Acompanhamento Embarque
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoVeiculo", Value = "198", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho tipo de veículo" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Transportador", Value = "199", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Transportador" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Placa", Value = "200", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Placa" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "NomeMotorista", Value = "201", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Nome do Motorista" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "LacreNumero", Value = "202", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Número do Lacre" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Termografo_Id", Value = "203", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Termografo" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "SifNumber", Value = "204", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do SIF" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Pedido", Value = "205", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do Pedido" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataCarregamento", Value = "206", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de Carregamento" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Instrucao", Value = "207", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero da Instrução" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "NumeroNotaFiscal", Value = "208", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero da Nota Fiscal" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoCarga", Value = "209", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do Tipo de Carga" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoProduto", Value = "211", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do tipo de Produto" });               
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TipoEmbalagem", Value = "210", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Numero do tipo de embalagem" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TemperaturaMin", Value = "212", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Temperatura Minima" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "TemperaturaMax", Value = "213", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Temperatura Maxima" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "PlanilhaRecebimentoCDs_Id", Value = "89", ControllerName = "AcompanhamentoEmbarque.cshtml", Descricao = "Id do Indicador Planilia de Recebimentos CDs" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "SIF", Value = "216", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho SIF" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataValidade", Value = "218", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de Validade" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataProducaoEmbarque", Value = "217", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de Produção" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "CB", Value = "215", ControllerName = "AcompanhamentoEmbarqueApi", Descricao = "Id do campo de cabeçalho Data de CB" });

                DicionariosInserir.Add(new DicionarioEstatico() { Key = "DataProducao", Value = "1197", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Data de Produção" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "Produto", Value = "1196", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdIndicadorPesoHB", Value = "71", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdTarefaPesoHB", Value = "1378", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdCabecalhoTaraPesoHB", Value = "cb1198", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });
                DicionariosInserir.Add(new DicionarioEstatico() { Key = "IdCabecalhoQuantidadeAmostraPesoHB", Value = "cb1199", ControllerName = "AvaliacaoSensorial.cshtml", Descricao = "Id do campo de cabeçalho Produto" });

                var add = DicionariosInserir.Select(r => r.Key).Except(dicionariosKeys);

                if (add != null)
                {
                    db.DicionarioEstatico.AddRange(DicionariosInserir.Where(r => add.Contains(r.Key)));
                    db.SaveChanges();
                }

            }
        }
    }

}

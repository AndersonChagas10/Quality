using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using DTO;
using SgqSystem.Mail;
using System.Configuration;
using Dominio;
using IntegrationModule;
using SgqSystem.Helpers;

namespace Jobs
{
    public class IntegrationJobFactory : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ExecuteIntegrationJobFunction(null);
        }

        public static void ExecuteIntegrationJobFunction(object stateInfo)
        {
            Thread.Sleep(new Random().Next(500, 2000));
            while (true)
            {
                if (DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.IntegrationJob == "on")
                {
                    IntegrationJobFactory.Execute();
                }

                Thread.Sleep(new Random().Next(2000, 10000));
            }
        }

        private static void Execute()
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    var integracaoSistemica = db.IntegracaoSistemica.Where(x => x.IsActive).ToList();

                    foreach (var item in integracaoSistemica)
                    {
                        var key = "IntegrationJobFactory" + item.Name;
                        var hasKey = GlobalConfig.UltimaExecucaoDoJob.Any(x => x.Key == key);
                        if (!hasKey || (hasKey &&
                            GlobalConfig.UltimaExecucaoDoJob[key].AddSeconds(item.Intervalo) <= DateTime.Now))
                        {

                            Task.Run(() => IntegrationJobFactory.RunIntegrationOneValue(item));
                            GlobalConfig.UltimaExecucaoDoJob[key] = DateTime.Now;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(new Exception("Erro no metodo [IntegrationJob]", ex));
            }
        }

        public static void RunIntegrationOneValue(IntegracaoSistemica item)
        {
            Exception ex = null;
            using (var db = new SgqDbDevEntities())
            {
                db.ErrorLog.Add(new Dominio.ErrorLog() { AddDate = DateTime.Now, StackTrace = "Entrou RunIntegrationOneValue ("+ item.Configuration + ")" });
                db.SaveChanges();
            }
            Integration.RunIntegrationOneValue(
                                    item.Configuration,
                                    item.Script,
                                    item.TableName,
                                    out ex);

            if (ex != null)
            {
                using (var db = new SgqDbDevEntities())
                {
                    db.ErrorLog.Add(new Dominio.ErrorLog() { AddDate = DateTime.Now, StackTrace = ex.ToClient() });
                    db.SaveChanges();
                }
            }

        }
    }
}
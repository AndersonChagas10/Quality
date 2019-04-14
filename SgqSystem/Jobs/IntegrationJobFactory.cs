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
                IntegrationJobFactory.Execute();

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

                            Task.Run(()=>
                                Integration.RunIntegrationOneValue(
                                    item.Configuration,
                                    item.Script,
                                    item.TableName)
                            );
                            GlobalConfig.UltimaExecucaoDoJob[key] = DateTime.Now;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo [IntegrationJob]", ex));
            }
        }
    }
}
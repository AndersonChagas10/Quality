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

namespace Jobs
{
    public class ReProcessJsonJob : IJob
    {
        public static bool Executing { get; set; } = false;
        public void Execute(IJobExecutionContext context)
        {
            ReProcessJsonJobFunction(null);
        }

        public static void ReProcessJsonJobFunction(object stateInfo)
        {
            Thread.Sleep(new Random().Next(30000, 50000));
            while (true)
            {
                if (ReProcessJsonJob.Executing)
                    return;

                ReProcessJsonJob.Executing = true;  //iniciou a execução
                ReProcessJsonJob.Execute();
                ReProcessJsonJob.Executing = false;  //finalizou a execução
                GlobalConfig.UltimaExecucaoDoJob["ResendProcessJsonJob"] = DateTime.Now;

                Thread.Sleep(new Random().Next(180000, 250000));
            }
        }

        private static void Execute()
        {
            if (DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.ResendProcessJsonJob == "on")
            {
                try
                {
                    SimpleAsynchronous.ResendProcessJson();
                }
                catch (Exception ex)
                {
                    LogSystem.LogErrorBusiness.Register(new Exception("Erro no metodo [ReProcessJsonJobFunction]", ex));
                }
            }
        }
    }
}
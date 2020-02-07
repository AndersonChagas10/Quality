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
    public class MailJob : IJob
    {
        public static bool Executing { get; set; } = false;
        public void Execute(IJobExecutionContext context)
        {
            SendMailJobFunction(null);
        }

        public static void SendMailJobFunction(object stateInfo)
        {
            Thread.Sleep(new Random().Next(1000, 5000));
            while (true)
            {
                if (MailJob.Executing)
                    return;

                MailJob.Executing = true;  //iniciou a execução
                MailJob.Execute();
                MailJob.Executing = false;  //finalizou a execução
                GlobalConfig.UltimaExecucaoDoJob["SendMailJob"] = DateTime.Now;

                Thread.Sleep(new Random().Next(60000, 120000));
            }
        }

        private static void Execute()
        {
            try
            {
                if (DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.SendMailJob == "on")
                {
                    if (GlobalConfig.Brasil || GlobalConfig.SESMT)
                    {
                        SimpleAsynchronous.CreateMailSgqAppDeviation();
                        Thread.Sleep(new Random().Next(100, 500));
                        SimpleAsynchronous.SendEmail();
                    }
                    else if (GlobalConfig.Eua)
                    {
                        SimpleAsynchronousUSA.SendMailUSA();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.LogErrorBusiness.Register(new Exception("Erro no metodo [SendMailJobFunction]", ex));
            }
        }
    }
}
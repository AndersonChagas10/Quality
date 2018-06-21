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
        public void Execute(IJobExecutionContext context)
        {
            SendMailJobFunction(null);
        }

        public static void SendMailJobFunction(object stateInfo)
        {
            Thread.Sleep(5000);
            while (true)
            {
                Task.Run(() =>
                {
                    try
                    {
                        if (ConfigurationManager.AppSettings["SendMailJob"] == "on")
                        {
                            if (GlobalConfig.Brasil)
                            {
                                SimpleAsynchronous.CreateMailSgqAppDeviation();
                                Task.Delay(3000);
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
                        new CreateLog(new Exception("Erro no metodo [SendMailJobFunction]", ex));
                    }
                });
                GlobalConfig.UltimaExecucaoDoJob["SendMailJob"] = DateTime.Now;
                Thread.Sleep(40000);
            }
        }
    }
}
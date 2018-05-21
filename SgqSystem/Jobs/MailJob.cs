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
            Thread.Sleep(2000);
            while (true)
            {
                try
                {
                    if (ConfigurationManager.AppSettings["SendMailJob"] == "exec")
                    {
                        if (GlobalConfig.Brasil)
                        {
                            SimpleAsynchronous.CreateMailSgqAppDeviation();
                            Thread.Sleep(5000);
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
                Thread.Sleep(2000);
            }
        }
    }
}
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
    public class DeviationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            DeviationJobFunction(null);
        }

        public static void DeviationJobFunction(object stateInfo)
        {
            Thread.Sleep(33777);
            while (true)
            {
                try
                {
                    if (ConfigurationManager.AppSettings["SendMailJob"] == "exec")
                    {
                        if (GlobalConfig.Brasil)
                        {
                            SimpleAsynchronous.CreateMailSgqAppDeviation();
                        }
                    }
                }
                catch (Exception ex)
                {
                    new CreateLog(new Exception("Erro no metodo [DeviationJobFunction]", ex));
                }
                Thread.Sleep(333333);
            }
        }
    }
}
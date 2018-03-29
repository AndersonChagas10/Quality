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
        public void Execute(IJobExecutionContext context)
        {
            ReProcessJsonJobFunction(null);
        }

        public static void ReProcessJsonJobFunction(object stateInfo)
        {
            Thread.Sleep(40000);
            while (true)
            {
                if (ConfigurationManager.AppSettings["ResendProcessJsonJob"] == "exec")
                {
                    try
                    {
                        SimpleAsynchronous.ResendProcessJson();
                    }
                    catch (Exception ex)
                    {
                        new CreateLog(new Exception("Erro no metodo [ReProcessJsonJobFunction]", ex));
                    }
                }
                Thread.Sleep(40000);
            }
        }
    }
}
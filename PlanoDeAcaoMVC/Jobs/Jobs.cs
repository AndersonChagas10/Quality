using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanoDeAcaoMVC
{
    public class Jobs
    {
        public static void UpdateStatus()
        {
            using (var dbPa = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 1 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (5) AND  CONVERT (date ,QuandoFim) < CONVERT (date ,GETDATE()))");
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 5 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (1) AND  CONVERT (date ,QuandoFim) >= CONVERT (date ,GETDATE()))");
            }
        }
    }
}
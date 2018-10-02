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
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 1 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] IN (5, 9) AND CONVERT(DATE, GETDATE()) > CONVERT(DATE, QuandoFim))");
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 9 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] IN (1, 5) AND  CONVERT(DATE, GETDATE()) < CONVERT(DATE, QuandoInicio))");
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 5 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] IN (1, 9) AND CONVERT(DATE, GETDATE()) >= CONVERT(DATE, QuandoInicio) AND CONVERT(DATE, GETDATE()) <= CONVERT(DATE, QuandoFim))");
            }
        }
    }
}
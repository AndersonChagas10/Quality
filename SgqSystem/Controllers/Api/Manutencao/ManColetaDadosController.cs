using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Manutencao
{
    [RoutePrefix("api/GetManColetaDados")]
    public class ManColetaDadosController : ApiController
    {
        //private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("Get")]
        public List<ManColetaDados> GetManColetaDados()
        {

            var sql = "Select * From [SERVERGRT\\MSSQLSERVER2014].SgqDbDev.dbo.ManColetaDados";


            using (var db = new SgqDbDevEntities())
            {

                var d = db.Database.SqlQuery<ManColetaDados>(sql).ToList();

                return d;
            }

        }
    }

}



using ADOFactory;
using Dominio;
using SgqSystem.Handlres;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
   
    [HandleApi()]
    [RoutePrefix("api/RetornaQueryRotinaApi")]
    public class RetornaQueryRotinaApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("RetornaQueryRotina")]
        public Object RetornaQueryRotina(string rotina_Id, string parametro)
        {
            var service = new SyncServices();
            var retornoRotinaNinja = new Object();

            var retorno = service.RetornaQueryRotina(rotina_Id, parametro);
            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoRotinaNinja = QueryNinja(db, retorno).FirstOrDefault();
            }
            return retornoRotinaNinja;
        }
    }
}

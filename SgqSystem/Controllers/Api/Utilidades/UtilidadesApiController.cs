using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SgqSystem.Controllers.Api;
using System.Data;
using ADOFactory;
using Newtonsoft.Json.Linq;
using SgqSystem.Helpers;

namespace SgqSystem.Controllers.Api.Utilidades
{
    [RoutePrefix("api/UtilidadesApi")]
    public class UtilidadesApiController : ApiController
    {
        [HttpPost]
        [Route("RunSqlStudio")]
        public object RunSqlStudio([FromBody]string query)
        {
            try
            {
                using (Factory factory = new Factory("DefaultConnection"))
                 {
                     return factory.QueryNinjaDataTable(query);
                 }
            }
            catch (Exception e)
            {
                return e.ToClient();
            }
        }
    }
}
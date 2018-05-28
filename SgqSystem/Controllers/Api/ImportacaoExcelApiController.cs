using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/ImportacaoExcel")]
    [HandleApi()]
    public class ImportacaoExcelApiController : ApiController
    {
        [HttpPost]
        [Route("SalvarExcel")]
        public HttpStatusCode SalvarExcel([FromBody] List<JObject> dados)
        {
            //ver com leo para subir a branche do gabriel( SyncServicee = insertJson nao esta funcionando)
            try
            {
                foreach (var item in dados)
                {
                    var teste = item.Last.ToString();
                    new SgqSystem.Services.SyncServices().InsertJson(item.ToString(), "5", "1", false);

                }
                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}

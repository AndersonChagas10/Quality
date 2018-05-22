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

            return HttpStatusCode.OK;
        }
    }
}

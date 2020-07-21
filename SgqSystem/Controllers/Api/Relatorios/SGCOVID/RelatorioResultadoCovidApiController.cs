using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Relatorios.SGCOVID
{
    [RoutePrefix("api/RelatorioResultadoCovid")]
    public class RelatorioResultadoCovidApiController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public string GetRelatorio()
        {
            return "";
        }
    }
}

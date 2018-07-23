using ADOFactory;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/SIFReports")]
    public class SIFReportsApiController : BaseApiController
    {

        [HttpPost]
        [Route("Get")]
        public dynamic Get([FromBody] FormularioParaRelatorioViewModel form)
        {
            var query = "select name from parlevel1";

            using (Factory factory = new Factory("DefaultConnection"))
            {

                return factory.SearchQuery<String>(query).ToList();

            }
        }

    }
}

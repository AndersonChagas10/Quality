using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosMD
{
    [RoutePrefix("api/AvaliacaoSensorial")]
    public class AvaliacaoSensorialApiController : ApiController
    {

        [HttpPost]
        [Route("Get")]
        public Retorno Get([FromBody] FormularioParaRelatorioViewModel form)
        {

            return null;
        }

        public class Retorno
        {

        }

    }
}

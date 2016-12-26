using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using SgqSystem.Controllers;
using SgqSystem.Handlres;

namespace SgqSystem.Controllers.Api
{

    [HandleApi()]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/NQA")]

    public class NQAController : ApiController
    {

        [HttpGet]
        [Route("getNQA/{nivel}/{tamanhoLote}")]
        public int getNQA(string nivel, string tamanhoLote)
        {
            var obj = new CepRecortesController();
            return obj.getNQA(nivel, tamanhoLote);
        }

    }
}

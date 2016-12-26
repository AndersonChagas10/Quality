using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using SgqSystem.Controllers;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/NQA")]

    public class NQAController : ApiController
    {

        [HttpPost]
        [Route("getNQA")]
        public int getNQA(int nivel, int tamanhoLote)
        {
            return CepRecortesController.getNQA(1, 1);
        }

    }
}

using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Apiteste")]
    public class TesteAPIController : ApiController
    {
        [HttpGet]
        [Route("testeApi")]
        public void tesssste()
        {
        }
    }
}

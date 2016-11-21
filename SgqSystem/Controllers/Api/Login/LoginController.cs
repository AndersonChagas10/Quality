using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Login
{
    [HandleApi()]
    [RoutePrefix("api/LoginApi")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("Logado")]
        public string Logado()
        {
            return "ok";
        }
    }
}

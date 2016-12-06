using SgqSystem.Handlres;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Login
{
    [HandleApi()]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

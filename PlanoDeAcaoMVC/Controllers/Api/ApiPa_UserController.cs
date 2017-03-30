using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_User")]
    public class ApiPa_UserController : ApiController
    {
        [HttpPost]
        [Route("CheckPass")]
        public string teste()
        {
            return "Senha validada com sucesso.";
        }
    }
}

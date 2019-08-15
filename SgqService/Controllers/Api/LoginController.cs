using SgqService.Handlres;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Login
{
    [HandleApi(saveLog: false)]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/LoginApi")]
    public class LoginController : ApiController
    {
        private SgqServiceBusiness.Api.Login.LoginController business;
        public LoginController()
        {
            business = new SgqServiceBusiness.Api.Login.LoginController();
        }

        [HttpGet]
        [Route("Logado/{dataApp?}")]
        public string Logado(DateTime? dataApp = null)
        {
            return business.Logado(dataApp);
        }
    }
}

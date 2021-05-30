using AppService;
using AppServiceSesmt.Api.Controllers;
using Dominio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppServiceSesmt.Controllers.Api.Sesmt
{
    [RoutePrefix("api/AppColeta")]
    public class SetActionController : BaseApiController
    {
        [HttpGet]
        [Route("SetAction")]
        public async Task<JObject> SetAction(Acao acao)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/AppColeta/SetAction";
            RestRequest restRequest = await RestRequest.Post(url, acao, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }

            return null;

        }
    }
}

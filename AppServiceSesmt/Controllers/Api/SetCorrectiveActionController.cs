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
    public class SetCorrectiveActionController : BaseApiController
    {
        [HttpGet]
        [Route("SetCorrectiveAction")]
        public async Task<JObject> SetCorrectiveAction([FromBody] List<CorrectiveAction> correctiveActions)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/AppColeta/SetCorrectiveAction";
            RestRequest restRequest = await RestRequest.Post(url, correctiveActions, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }

            return null;
        }
    }
}
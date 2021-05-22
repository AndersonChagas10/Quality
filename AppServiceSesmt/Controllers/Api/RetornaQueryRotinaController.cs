using AppService;
using AppServiceSesmt.Api.Controllers;
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
    [RoutePrefix("api/RetornaQueryRotinaApi")]
    public class RetornaQueryRotinaController : BaseApiController
    {
        [HttpPost]
        [Route("RetornaQueryRotina")]
        public async Task<JObject> RetornaQueryRontina(JToken body)
        {
            string url = $"/api/RetornaQueryRotinaApi/RetornaQueryRotina";
            RestRequest restRequest = await RestRequest.Post(url, body, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }

            return null;

        }
    }
}

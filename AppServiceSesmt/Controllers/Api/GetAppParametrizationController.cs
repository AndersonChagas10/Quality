using AppService;
using AppServiceSesmt.Api.Controllers;
using Newtonsoft.Json.Linq;
using ServiceModel;
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
    public class GetAppParametrizationController : BaseApiController
    {
        [HttpPost]
        [Route("GetAppParametrization")]
        public async Task<JObject> GetAppParametrization(PlanejamentoColeta appParametrization)
        {
            string url = $"/api/AppColeta/GetAppParametrization";
            RestRequest restRequest = await RestRequest.Post(url, appParametrization, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }

            return null;

        }
    }
}

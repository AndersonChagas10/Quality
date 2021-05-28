using AppService;
using AppServiceSesmt.Api.Controllers;
using Newtonsoft.Json;
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
    public partial class GetResultsController : BaseApiController
    {
        [HttpPost]
        [Route("GetResults")]
        public async Task<string> GetResults(GetResultsData obj)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/AppColeta/GetResults";
            RestRequest restRequest = await RestRequest.Post(url, obj, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }
    }
}

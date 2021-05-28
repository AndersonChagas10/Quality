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
    public partial class AppColetaController : BaseApiController
    {
        [HttpPost]
        [Route("GetColetaParcial")]
        public async Task<string> GetColetaParcial(GetResultsData resultsData)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/AppColeta/GetColetaParcial";
            RestRequest restRequest = await RestRequest.Post(url, resultsData, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }

            return null;
        }
    }
}

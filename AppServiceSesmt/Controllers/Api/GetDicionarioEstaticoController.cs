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
    [RoutePrefix("api/AppParams")]
    public partial class GetDicionarioEstaticoController : BaseApiController
    {
        [HttpGet]
        [Route("GetDicionarioEstatico")]
        public async Task<string> GetDicionarioEstatico()
        {
            string url = $"/api/AppParams/GetDicionarioEstatico";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }
    }
}

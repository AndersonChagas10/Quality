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
    public class SetCollectController : BaseApiController
    {
        [HttpGet]
        [Route("SetCollect")]
        public async Task<JObject> SetCollect(List<Collection> listSimpleCollect)
        {
            string url = $"/api/AppColeta/SetCollect";
            RestRequest restRequest = await RestRequest.Post(url, listSimpleCollect, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }

            return null;

        }
    }
}

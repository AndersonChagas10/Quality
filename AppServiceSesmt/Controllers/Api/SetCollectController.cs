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
        [HttpPost]
        [Route("SetCollect")]
        public async Task<object> SetCollect(List<Collection> listSimpleCollect)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/AppColeta/SetCollect";
            RestRequest restRequest = await RestRequest.Post(url, listSimpleCollect, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject(restRequest.Response);
            }

            return null;

        }
    }
}

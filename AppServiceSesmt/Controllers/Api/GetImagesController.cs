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

namespace AppServiceSesmt.Api.Controllers
{
    [RoutePrefix("api/AppParams")]
    public partial class GetImagesController : BaseApiController
    {
        [HttpPost]
        [Route("GetImages")]
        public async Task<JObject> GetImages()
        {

            string url = $"/api/AppParams/GetImages";
            RestRequest restRequest = await RestRequest.Post(url, null, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }

            return null;
        }
    }
}

using AppService;
using AppServiceSesmt.Api.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppServiceSesmt.Controllers.Api
{
    [RoutePrefix("api/AppParams")]
    public partial class GetAppVersionIsUpdatedController : BaseApiController
    {

        [HttpGet]
        [Route("GetAppVersionIsUpdated")]
        public async Task<JObject> GetAppVersionIsUpdated(string versionNumber)
        {
            string url = $"/Config/GetAppVersionIsUpdated?versionNumber=" + versionNumber;
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<JObject>(restRequest.Response);
            }

            return null;
        }

    }
}

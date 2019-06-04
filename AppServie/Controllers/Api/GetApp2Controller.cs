using AppService;
using ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppServie.Api.Controllers
{
    [RoutePrefix("api/SyncServiceApi")]
    public class GetApp2Controller : BaseApiController
    {
        [HttpPost]
        [Route("getAPP2")]
        public async Task<string> getAPP2(string version)
        {
            string url = $"/api/SyncServiceApi/getAPP2?version={version}";
            RestRequest restRequest = await RestRequest.Post(url, null);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

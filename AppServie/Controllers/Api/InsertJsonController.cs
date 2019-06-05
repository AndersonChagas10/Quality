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
    public partial class SyncServiceApiController : BaseApiController
    {
        [HttpPost]
        [Route("InsertJson")]
        public async Task<string> InsertJson([FromBody] InsertJsonClass insertJsonClass)
        {
            string url = "/api/SyncServiceApi/InsertJson";
            RestRequest restRequest = await RestRequest.Post(url, insertJsonClass, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

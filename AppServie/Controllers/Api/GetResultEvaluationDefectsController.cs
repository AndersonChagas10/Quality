using AppService;
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

namespace AppServie.Api.Controllers
{
    public partial class SyncServiceApiController : BaseApiController
    {
        [HttpPost]
        [Route("GetResultEvaluationDefects")]
        public async Task<string> GetResultEvaluationDefects(GetResultEvaluationDefects getResultEvaluationDefects)
        {
            string url = "/api/SyncServiceApi/GetResultEvaluationDefects";
            RestRequest restRequest = await RestRequest.Post(url, getResultEvaluationDefects, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

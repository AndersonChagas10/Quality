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
    [RoutePrefix("api/ParamsApi")]
    public partial class ParamsApiController : BaseApiController
    {
        [HttpPost]
        [Route("GetResource")]
        public async Task<string> GetResource([FromBody] InsertDeviationClass insertDeviationClass)
        {
            string url = "/api/SyncServiceApi/insertDeviation";
            RestRequest restRequest = await RestRequest.Post(url, insertDeviationClass, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

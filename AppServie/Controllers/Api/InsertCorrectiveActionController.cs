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
        [Route("InsertCorrectiveAction")]
        public async Task<string> InsertCorrectiveAction([FromBody] InsertCorrectiveActionClass insertCorrectiveActionClass)
        {
            string url = "/api/SyncServiceApi/insertDeviation";
            RestRequest restRequest = await RestRequest.Post(url, insertCorrectiveActionClass, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

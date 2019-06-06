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
        [Route("GetLastSampleByCollectionLevel2")]
        public async Task<int> GetLastSampleByCollectionLevel2(GetLastSampleByCollectionLevel2Class getLastSampleByCollectionLevel2Class)
        {
            string url = "/api/SyncServiceApi/GetLastSampleByCollectionLevel2";
            RestRequest restRequest = await RestRequest.Post(url, getLastSampleByCollectionLevel2Class, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<int>(restRequest.Response);
            }
            return 0;
        }

    }
}

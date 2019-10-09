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
        [Route("getPhaseLevel2")]
        public async Task<string> GetPhaseLevel2(int ParCompany_Id, string date)
        {
            string url = $"/api/SyncServiceApi/getPhaseLevel2?ParCompany_Id={ParCompany_Id}&date={date}";
            RestRequest restRequest = await RestRequest.Post(url, null, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

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
        [Route("reciveDataByLevel1")]
        public async Task<string> ReciveDataByLevel1(string ParCompany_Id, string data, string ParLevel1_Id)
        {
            string url = $"/api/SyncServiceApi/reciveDataByLevel1?ParCompany_Id={ParCompany_Id}&data={data}&ParLevel1_Id={ParLevel1_Id}";
            RestRequest restRequest = await RestRequest.Post(url, null, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

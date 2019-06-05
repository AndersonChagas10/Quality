using AppService;
using DTO.DTO;
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
    [RoutePrefix("api/SyncServiceApi")]
    public partial class SyncServiceApiController : BaseApiController
    {
        [HttpPost]
        [Route("getCollectionLevel2Keys")]
        public async Task<string> GetCollectionLevel2Keys([FromBody] GetCollectionLevel2KeysClass getCollectionLevel2KeysClass)
        {
            string url = $"/api/SyncServiceApi/getCollectionLevel2Keys";
            RestRequest restRequest = await RestRequest.Post(url, getCollectionLevel2KeysClass, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

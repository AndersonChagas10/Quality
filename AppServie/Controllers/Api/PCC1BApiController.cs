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
    [RoutePrefix("api/PCC1B")]
    public partial class PCC1BApiController : BaseApiController
    {
        [HttpPost]
        [Route("Next")]
        public async Task<_PCC1B> Next([FromBody] _Receive receive)
        {
            string url = "/api/PCC1B/Next";
            RestRequest restRequest = await RestRequest.Post(url, receive, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<_PCC1B>(restRequest.Response);
            }
            return null;
        }

        [HttpPost]
        [Route("TotalNC/{parLevel2IdDianteiro}/{parLevel2Id2Traseiro}/{shift}")]
        public async Task<IEnumerable<CollectionLevel2PCC1B>> TotalNC(int parLevel2IdDianteiro, int parLevel2Id2Traseiro, int shift, 
            [FromBody] _Receive receive)
        {
            string url = $"/api/PCC1B/TotalNC/{parLevel2IdDianteiro}/{parLevel2Id2Traseiro}/{shift}";
            RestRequest restRequest = await RestRequest.Post(url, receive, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<CollectionLevel2PCC1B>>(restRequest.Response);
            }
            return null;
        }

    }
}

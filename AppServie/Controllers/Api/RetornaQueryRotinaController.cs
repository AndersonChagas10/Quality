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
    [RoutePrefix("api/RetornaQueryRotinaApi")]
    public partial class RetornaQueryRotinaApiController : BaseApiController
    {
        [HttpPost]
        [Route("RetornaQueryRotina")]
        public async Task<object> RetornaQueryRotina([FromBody] JToken body)
        {
            string url = "/api/RetornaQueryRotinaApi/RetornaQueryRotina";
            RestRequest restRequest = await RestRequest.Post(url, body, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<object>(restRequest.Response);
            }
            return null;
        }

    }
}

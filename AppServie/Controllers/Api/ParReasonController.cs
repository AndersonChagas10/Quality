using AppService;
using Dominio;
using Newtonsoft.Json;
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
    [RoutePrefix("api/ParReason")]
    public partial class ParReasonController : BaseApiController
    {
        [HttpGet]
        [Route("Get")]
        public async Task<List<ParReason>> Get()
        {
            string url = "/api/ParReason/Get";
            RestRequest restRequest = await RestRequest.Get(url);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<ParReason>>(restRequest.Response);
            }
            return null;
        }

    }
}

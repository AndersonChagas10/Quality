using AppService;
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
   [RoutePrefix("api/AppParams")]
    public partial class AppParamsController : BaseApiController
    {

        [HttpGet]
        [Route("GetTela/{UnitId}/{ShiftId?}")]
        public async Task<RetornoParaTablet> GetTela(int UnitId, int ShiftId = 0)
        {
            string url = $"/api/AppParams/GetTela/{UnitId}/{ShiftId}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<RetornoParaTablet>(restRequest.Response);
            }
            return null;
        }

    }
}

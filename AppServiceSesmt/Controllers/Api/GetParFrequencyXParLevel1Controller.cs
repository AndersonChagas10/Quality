using AppService;
using AppServiceSesmt.Api.Controllers;
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

namespace AppServiceSesmt.Controllers.Api.Sesmt
{
    [RoutePrefix("api")]
    public partial class GetParFrequencyXParLevel1Controller : BaseApiController
    {
        [HttpPost]
        [Route("GetParFrequencyXParLevel1")]
        public async Task<List<JObject>> GetParFrequencyXParLevel1(PlanejamentoColetaViewModel obj)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/GetParFrequencyXParLevel1";
            RestRequest restRequest = await RestRequest.Post(url, obj, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<JObject>>(restRequest.Response);
            }

            return null;
        }
    }
}

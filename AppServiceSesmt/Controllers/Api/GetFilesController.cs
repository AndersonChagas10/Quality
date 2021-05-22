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

namespace AppServiceSesmt.Api.Controllers
{
    [RoutePrefix("api/AppParams")]
    public partial class GetFilesController : BaseApiController
    {
        [HttpGet]
        [Route("GetFiles")]
        public async Task<string> GetFiles()
        {
            string url = $"/api/AppParams/GetFiles";
            RestRequest restRequest = await RestRequest.Get(url);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

using AppService;
using DTO.DTO;
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
    [RoutePrefix("api/SyncServiceApi")]
    public class GetAPPLevelsVolumeController : BaseApiController
    {
        [HttpPost]
        [Route("getAPPLevelsVolume")]
        public async Task<string> GetAPPLevelsVolume([FromBody] GetAPPLevelsVolumeClass getAPPLevelsVolumeClass)
        {
            string url = $"/api/SyncServiceApi/getAPPLevelsVolume";
            RestRequest restRequest = await RestRequest.Post(url, getAPPLevelsVolumeClass, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

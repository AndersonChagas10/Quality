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
    [RoutePrefix("api/ResultLevel3PhotosApi")]
    public partial class ResultLevel3PhotosApiController : BaseApiController
    {
        [HttpPost]
        public async Task<dynamic> Post([FromBody] List<Result_Level3_PhotosDTO> Fotos)
        {
            string url = "/api/ResultLevel3PhotosApi";
            RestRequest restRequest = await RestRequest.Post(url, Fotos, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<dynamic>(restRequest.Response);
            }
            return null;
        }

    }
}

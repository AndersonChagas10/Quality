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
    [RoutePrefix("api/User")]
    public class GetAllUserByUnitController : BaseApiController
    {

        [HttpPost]
        [Route("GetAllUserByUnit/{unidadeId}")]
        public async Task<List<UserDTO>> GetAllUserByUnit(int unidadeId)
        {
            string url = $"/api/User/GetAllUserByUnit/{unidadeId}";
            RestRequest restRequest = await RestRequest.Post(url, null, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<UserDTO>>(restRequest.Response);
            }
            return null;
        }

    }
}

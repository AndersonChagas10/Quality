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
    [RoutePrefix("api/Reprocesso")]
    public class ReprocessoController : BaseApiController
    {
        [HttpGet]
        [Route("Get/{ParCompany_Id}")]
        public async Task<RetrocessoReturn> Get(int ParCompany_Id)
        {
            string url = $"/api/Reprocesso/Get/{ParCompany_Id}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<RetrocessoReturn>(restRequest.Response);
            }
            return null;
        }

    }
}

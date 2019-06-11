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
    public partial class ParHeaderController : BaseApiController
    {

        [HttpGet]
        [Route("GetListParMultipleValuesXParCompany/{unitId}/{level1_id}")]
        public async Task<IEnumerable<ParMultipleValuesXParCompany>> GetListParMultipleValuesXParCompany(int UnitId, string level1_id)
        {
            string url = $"/api/ParHeader/GetListParMultipleValuesXParCompany/{UnitId}/{level1_id}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ParMultipleValuesXParCompany>>(restRequest.Response);
            }
            return null;
        }

    }
}

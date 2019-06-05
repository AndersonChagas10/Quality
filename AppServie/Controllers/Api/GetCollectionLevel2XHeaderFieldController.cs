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
    [RoutePrefix("api/ParHeader")]
    public partial class ParHeaderController : BaseApiController
    {
        [HttpGet]
        [Route("GetCollectionLevel2XHeaderField/{unitId}/{date}")]
        public async Task<IEnumerable<CollectionHeaderField>> GetListCollectionHeaderField(int UnitId, String Date)
        {
            string url = $"/api/ParHeader/GetCollectionLevel2XHeaderField/{UnitId}/{Date}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<CollectionHeaderField>>(restRequest.Response);
            }
            return null;
        }

    }
}

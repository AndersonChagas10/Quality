using AppService;
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
    public partial class SyncServiceApiController : BaseApiController
    {
        [HttpPost]
        [Route("UserCompanyUpdate")]
        public async Task<string> UserCompanyUpdate(string UserSgq_Id, int ParCompany_Id)
        {
            string url = $"/api/SyncServiceApi/UserCompanyUpdate?UserSgq_Id={UserSgq_Id}&ParCompany_Id={ParCompany_Id}";
            RestRequest restRequest = await RestRequest.Post(url, null, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

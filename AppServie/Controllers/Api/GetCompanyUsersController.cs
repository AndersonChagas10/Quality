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
    public partial class SyncServiceApiController : BaseApiController
    {

        [HttpPost]
        [Route("getCompanyUsers")]
        public async Task<string> getCompanyUsers(string ParCompany_Id)
        {
            string url = $"/api/SyncServiceApi/getCompanyUsers?ParCompany_Id={ParCompany_Id}";
            RestRequest restRequest = await RestRequest.Post(url, null, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

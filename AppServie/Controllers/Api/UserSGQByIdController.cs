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
    [RoutePrefix("api/SyncServiceApi")]
    public class UserSGQByIdController : BaseApiController
    {
        [HttpPost]
        [Route("UserSGQById")]
        public async Task<string> UserSGQById(int Id)
        {
            string url = $"/api/SyncServiceApi/UserSGQById?id={Id}";
            RestRequest restRequest = await RestRequest.Post(url, null);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

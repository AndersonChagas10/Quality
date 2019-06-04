using AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppServie.Controllers
{
    public class GetTelaController : ApiController
    {
        // GET api/GetTela
        public async Task<string> Get()
        {
            RestRequest restRequest = await RestRequest.Post("", null, "");

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

        [HttpPost]
        public async Task<string> Get2()
        {
            RestRequest restRequest = await RestRequest.Get("", "");
            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

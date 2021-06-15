using AppService;
using AppServiceSesmt.Api.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppServiceSesmt.Controllers.Api.Sesmt
{
    [RoutePrefix("api")]
    public partial class ParCompanyController : BaseApiController
    {
        [HttpGet]
        [Route("parCompany")]
        public async Task<List<JObject>> ParCompany(int userSgq_Id)
        {
            VerifyIfIsAuthorized();
            string url = $"/api/parCompany?userSgq_Id=" + userSgq_Id + "";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<JObject>>(restRequest.Response);
            }

            return null;
        }
    }
}

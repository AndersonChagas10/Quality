using AppService;
using DTO.DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceModel;
using SgqService.ViewModels;
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
    public partial class UserController : BaseApiController
    {


        [HttpPost]
        [Route("AuthenticationLogin")]
        public async Task<JObject> AuthenticationLogin([FromBody] UserViewModel userVm)
        {
            string url = $"/api/User/AuthenticationLogin";
            RestRequest restRequest = await RestRequest.Post(url, userVm, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(restRequest.Response);
            }
            return null;
        }

    }
}

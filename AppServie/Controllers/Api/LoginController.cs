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
    [RoutePrefix("api/LoginApi")]
    public class LoginController : BaseApiController
    {
        [HttpGet]
        [Route("Logado/{dataApp?}")]
        public string Logado(DateTime? dataApp = null)
        {
            if (dataApp != null)
            {
                var dataServer = DateTime.Now;

                if (dataApp < dataServer.AddHours(-30))
                {
                    return "dataInvalida";
                }
            }

            return "onLine";
        }
    }
}

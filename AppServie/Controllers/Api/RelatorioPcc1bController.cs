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
    [RoutePrefix("api/RelatorioGenerico")]
    public class RelatorioGenericoController : BaseApiController
    {
        [HttpGet]
        [Route("reciveDataPCC1b2/{unidadeId}/{data}")]
        public async Task<dynamic> reciveDataPCC1b2(string unidadeId, string data)
        {
            string url = $"/api/RelatorioGenerico/reciveDataPCC1b2/{unidadeId}/{data}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<dynamic>(restRequest.Response);
            }
            return null;
        }

    }
}

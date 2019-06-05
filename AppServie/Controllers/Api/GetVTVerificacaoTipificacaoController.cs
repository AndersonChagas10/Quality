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
    [RoutePrefix("api/VTVerificacaoTipificacao")]
    public class VTVerificacaoTipificacaoController : BaseApiController
    {
        [HttpGet]
        [Route("GetAll/{Date}/{UnidadeId}")]
        public async Task<string> GetVTVerificacaoTipificacao(String Date, int UnidadeId)
        {
            string url = $"/api/VTVerificacaoTipificacao/GetAll/{Date}/{UnidadeId}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }
            return null;
        }

    }
}

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
using SgqService.ViewModels;

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


        [HttpPost]
        [Route("Save")]
        public async Task<string> SaveVTVerificacaoTipificacao(TipificacaoViewModel model)
        {
            string url = $"/api/VTVerificacaoTipificacao/Save";
            RestRequest restRequest = await RestRequest.Post(url, model, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<string>(restRequest.Response);
            }

            return null;
        }

    }
}

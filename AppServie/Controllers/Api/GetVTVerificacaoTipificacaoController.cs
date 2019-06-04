﻿using AppService;
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
    public class GetVTVerificacaoTipificacaoController : BaseApiController
    {
        [HttpGet]
        [Route("GetAll/{Date}/{UnidadeId}")]
        public async Task<string> GetVTVerificacaoTipificacao(String Date, int UnidadeId)
        {
            string url = $"/api/VTVerificacaoTipificacao/GetAll/{Date}/{UnidadeId}";
            RestRequest restRequest = await RestRequest.Get(url, this.token);

            if (restRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return restRequest.Response;
            }
            return null;
        }

    }
}

using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqService.ViewModels;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/RelatorioGenerico")]
    public class RelatorioGenericoApiController : BaseApiController
    {
        SgqServiceBusiness.Api.RelatorioGenericoApiController business;
        public RelatorioGenericoApiController()
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            business = new SgqServiceBusiness.Api.RelatorioGenericoApiController(conexao);
        }

        //Tabela Angular
        [HttpPost]
        [Route("getTabela")]
        public dynamic getTabela(FormularioParaRelatorioViewModel model)
        {
            return business.getTabela(model);
        }

        //Tabela Javascript
        [HttpPost]
        [Route("getTabela2")]
        public dynamic getTabela2(FormularioParaRelatorioViewModel model)
        {
            return business.getTabela2(model);
        }

        [HttpPost]
        [Route("getTabela3")]
        public dynamic getTabela3(JObject model)
        {
            return business.getTabela3(model);
        }

        [HttpGet]
        [Route("reciveDataPCC1b2/{unidadeId}/{data}/{shift}")]
        public dynamic reciveDataPCC1b2(string unidadeId, string data, string shift)
        {
            VerifyIfIsAuthorized();
            return business.reciveDataPCC1b2(unidadeId, data, shift);
        }

        //QueryGenerica para implementar

        [HttpPost]
        [Route("GetParametrizacaoGeral")]
        public dynamic GetParametrizacaoGeral([FromBody] FormularioParaRelatorioViewModel form)
        {
            return business.GetParametrizacaoGeral(form);
        }
    }
}
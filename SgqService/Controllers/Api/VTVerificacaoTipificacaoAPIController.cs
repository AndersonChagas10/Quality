using ADOFactory;
using Dapper;
using Dominio;
using DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using SGQDBContext;
using SgqService.Handlres;
using SgqService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SgqService.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/VTVerificacaoTipificacao")]
    public class VTVerificacaoTipificacaoApiController : BaseApiController
    {

        private SgqServiceBusiness.Api.VTVerificacaoTipificacaoApiController business;
        public VTVerificacaoTipificacaoApiController()
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            business = new SgqServiceBusiness.Api.VTVerificacaoTipificacaoApiController(conexao);
            dbSgq = new SgqDbDevEntities();
            dbSgq.Configuration.LazyLoadingEnabled = false;
            dbSgq.Configuration.ValidateOnSaveEnabled = false;

        }
        public string mensagemErro { get; set; }

        SgqDbDevEntities dbSgq;

        [Route("Save")]
        [HttpPost]
        public void SaveVTVerificacaoTipificacao(TipificacaoViewModel model)
        {
            business.SaveVTVerificacaoTipificacao(model);
        }

        [HttpGet]
        [Route("GetAll/{Date}/{UnidadeId}")]
        public string GetVTVerificacaoTipificacao(String Date, int UnidadeId)
        {
            VerifyIfIsAuthorized();
            return business.GetVTVerificacaoTipificacao(Date, UnidadeId);
        }
    }

}
using ADOFactory;
using Dominio;
using DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using ServiceModel;
using SgqService.Controllers.Api;
using SgqService.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqService.Controllers.Api.App
{
    [RoutePrefix("api/AppParams")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppParamsApiController : BaseApiController
    {
        SgqServiceBusiness.Api.App.AppParamsApiController Business;

        public AppParamsApiController()
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Business = new SgqServiceBusiness.Api.App.AppParamsApiController(conexao);
        }

        /// <summary>
        /// Sobrescreve a tela do tablet para todas as unidades.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateTelaDoTablet")]
        public RetornoParaTablet UpdateTelaDoTablet()
        {
            return Business.UpdateTelaDoTablet();
        }

        /// <summary>
        /// Atualiza, se existir, a tela do tablet para determinada unidade.
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateTelaDoTablet/{UnitId}")]
        public RetornoParaTablet UpdateTelaDoTablet(int UnitId)
        {
            return Business.UpdateTelaDoTablet(UnitId);
        }

        /// <summary>
        /// Faz download de todas as telas prontas / atualizadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ParamsDisponiveis")]
        public Dictionary<int, HtmlDoTablet> ParamsDisponiveis()
        {
            return Business.ParamsDisponiveis();
        }

        /// <summary>
        /// Responde a tela de uma unidade para o tablet
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTela/{UnitId}/{ShiftId?}")]
        public RetornoParaTablet GetTela(int UnitId, int ShiftId = 0)
        {
            VerifyIfIsAuthorized();

            return Business.GetTela(UnitId, ShiftId);
        }

        [HttpGet]
        [Route("UpdateDbRemoto/{UnitId}")]
        public void UpdateDbRemoto(int UnitId)
        {
            Business.UpdateDbRemoto(UnitId);
        }

        [HttpGet]
        [Route("GetUnits")]
        public List<int> GetUnits()
        {
            return Business.GetUnits();
        }

        [HttpGet]
        [Route("GetFiles")]
        public string GetFiles()
        {
            return Business.GetFiles();
        }

        [HttpGet]
        [Route("GetAPP")]
        public string GetAPP()
        {
            return Business.GetAPP();
        }

        #region Nova Proposta Get Tela

        [HttpGet]
        [Route("GetStackTrace/{id}")]
        public object GetStackTrace(int id)
        {
            return Business.GetStackTrace(id);
        }

        [HttpPost]
        [Route("GetGeneratedUnits")]
        public object GetGeneratedUnits([FromBody]GeneratedUnit generatedUnit)
        {
            return Business.GetGeneratedUnits(generatedUnit);
        }

        [HttpPost]
        [Route("UpdateGetTelaThread")]
        public void UpdateGetTelaThread([FromBody]GeneratedUnit generatedUnit)
        {
            Business.UpdateGetTelaThread(generatedUnit);
        }

        #endregion

    }
}

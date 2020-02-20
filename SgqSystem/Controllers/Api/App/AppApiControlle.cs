using ADOFactory;
using Dominio;
using DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using ServiceModel;
using SgqSystem.Controllers.Api;
using SgqSystem.Helpers;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.App
{
    /// <summary>
    /// 
    /// Gerencia em memória a tela da parametrização dos tablets para unidades.
    /// 
    /// Serviços disponíveis:
    /// 
    /// var root = @Html.Raw(Json.Encode(GlobalConfig.urlPreffixAppColleta));
    /// $.get(root +'/api/AppParams/UpdateTelaDoTablet', { }, function(r) { console.log(r)});
    /// $.get(root +'/api/AppParams/UpdateTelaDoTablet/21', {UnitId: 21 }, function(r) { console.log(r)});
    /// $.get(root +'/api/AppParams/ParamsDisponiveis', { }, function(r) { console.log(r)});
    /// $.get(root +'/api/AppParams/GetTela/21', { }, function(r) { console.log(r)});
    /// 
    /// </summary>
    [RoutePrefix("api/AppParams")]
    public class AppParamsApiController : BaseApiController, IDisposable
    {
        SgqServiceBusiness.Api.App.AppParamsApiController Business;

        public AppParamsApiController()
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Business = new SgqServiceBusiness.Api.App.AppParamsApiController(conexao);
        }

        [HttpPost]
        [Route("GetImages")]
        public Dictionary<string, string> GetImages()
        {
            return Business.GetImages();
        }

        [HttpGet]
        [Route("GetDicionarioEstatico")]
        public string GetDicionarioEstatico()
        {
            return Business.GetDicionarioEstatico();
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
            bool liberarFilaSemaforoThread = false;
            return Business.UpdateTelaDoTablet(UnitId, out liberarFilaSemaforoThread);
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

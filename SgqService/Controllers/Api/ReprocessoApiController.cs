using ADOFactory;
using Dominio;
using ServiceModel;
using SgqService.Handlres;
using SgqService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqService.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Reprocesso")]
    public class ReprocessoApiController : BaseApiController
    {
        private SgqServiceBusiness.Api.ReprocessoApiController business;

        public ReprocessoApiController()
        {
            string PROC_ParReprocessoEntradaOP = AppSettingsWebConfig.GetValue("PROC_ParReprocessoEntradaOP");
            string PROC_ParReprocessoCertificadosSaidaOP = AppSettingsWebConfig.GetValue("PROC_ParReprocessoCertificadosSaidaOP");
            string PROC_ParReprocessoSaidaOP = AppSettingsWebConfig.GetValue("PROC_ParReprocessoSaidaOP");
            string PROC_ParReprocessoHeaderOP = AppSettingsWebConfig.GetValue("PROC_ParReprocessoHeaderOP");
            string BuildEm = AppSettingsWebConfig.GetValue("BuildEm");
            business = new SgqServiceBusiness.Api.ReprocessoApiController(
                PROC_ParReprocessoEntradaOP,
                BuildEm,
                PROC_ParReprocessoCertificadosSaidaOP,
                PROC_ParReprocessoSaidaOP,
                PROC_ParReprocessoHeaderOP
                );
        }

        [Route("Get/{ParCompany_Id}")]
        [HttpGet]
        public RetrocessoReturn Get(int ParCompany_Id)
        {
            VerifyIfIsAuthorized();
            return business.Get(ParCompany_Id);
        }

        [Route("GetCollectionLevel2Reprocesso/{ParCompany_Id}/{dtIni}/{dtFim}/{headerEntrada}/{headerSaida}")]
        [HttpGet]
        public IEnumerable<dynamic> GetCollectionLevel2Reprocesso(int ParCompany_Id, string dtIni
            , string dtFim, int headerEntrada, int headerSaida)
        {
            return business.GetCollectionLevel2Reprocesso(ParCompany_Id, dtIni, dtFim, headerEntrada, headerSaida);
        }

        [Route("GetReportReprocesso/{cl2_Id}/{ParCompany_Id}/{dtIni}/{dtFim}/{cabecalho_idEntrada}/{level2_idEntrada}/{cabecalho_idSaida}/{level2_idSaida}")]
        [HttpGet]
        public IEnumerable<dynamic> GetReportReprocesso(int cl2_Id, int ParCompany_Id
            , string dtIni, string dtFim, int cabecalho_idEntrada, int level2_idEntrada
            , int cabecalho_idSaida, int level2_idSaida)
        {
            return business.GetReportReprocesso(cl2_Id, ParCompany_Id, dtIni, dtFim, cabecalho_idEntrada,
                level2_idEntrada, cabecalho_idSaida, level2_idSaida);
        }

        [Route("GetReportReprocessoHeader/{cl2_Id}/{unitId}/{dtIni}/{dtFim}/{cabecalho_idEntrada}/{level2_idEntrada}/{cabecalho_idSaida}/{level2_idSaida}")]
        [HttpGet]
        public IEnumerable<dynamic> GetReportReprocessoHeader(int cl2_Id, int unitId,
            string dtIni, string dtFim, int cabecalho_idEntrada, int level2_idEntrada,
            int cabecalho_idSaida, int level2_idSaida)
        {
            return business.GetReportReprocessoHeader(cl2_Id, unitId, dtIni, dtFim,
                cabecalho_idEntrada, level2_idEntrada, cabecalho_idSaida, level2_idSaida);
        }

        [Route("GetMonitor/{user}")]
        [HttpGet]
        public IEnumerable<dynamic> GetMonitor(int user)
        {
            return business.GetMonitor(user);
        }
    }
}

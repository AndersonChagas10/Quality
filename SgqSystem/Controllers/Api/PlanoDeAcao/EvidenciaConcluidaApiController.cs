using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Conformity.Infra.CrossCutting;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/EvidenciaConcluidaApi")]

    public class EvidenciaConcluidaApiController : BaseAuthenticatedApiController
    {
        private readonly EvidenciaConcluidaService _evidenciaConcluidaService;

        public EvidenciaConcluidaApiController(EvidenciaConcluidaService evidenciaConcluidaService
            , ApplicationConfig applicationConfig) : base(applicationConfig)
        {
            _evidenciaConcluidaService = evidenciaConcluidaService;
        }

        [Route("GetFotosEvidenciaConcluida/{id}")]
        [HttpGet]
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidenciaConcluida(int id)
        {
            return _evidenciaConcluidaService.ObterFotosEvidenciaConcluida(id);
        }
    }
}
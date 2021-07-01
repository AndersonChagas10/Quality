using DTO.PlanoDeAcao;
using Services.PlanoDeAcao;
using Services.PlanoDeAcao.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/EvidenciaConcluidaApi")]

    public class EvidenciaConcluidaApiController : BaseApiController
    {
        private readonly IEvidenciaConcluidaService _evidenciaConcluidaService;

        public EvidenciaConcluidaApiController(IEvidenciaConcluidaService evidenciaConcluidaService)
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
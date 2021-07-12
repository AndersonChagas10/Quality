using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Conformity.Infra.CrossCutting;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/EvidenciaNaoConformeApi")]

    public class EvidenciaNaoConformeApiController : BaseAuthenticatedApiController
    {
        private readonly EvidenciaNaoConformeService _evidenciaNaoConformeService;

        public EvidenciaNaoConformeApiController(EvidenciaNaoConformeService evidenciaNaoConformeService
            , ApplicationConfig applicationConfig) : base(applicationConfig)
        {
            _evidenciaNaoConformeService = evidenciaNaoConformeService;
        }

        [Route("GetFotosEvidencia/{id}")]
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidencia(int id)
        {
            return _evidenciaNaoConformeService.ObterFotosEvidencia(id);
        }
    }
}
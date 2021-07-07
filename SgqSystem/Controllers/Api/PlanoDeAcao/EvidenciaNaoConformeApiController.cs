using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/EvidenciaNaoConformeApi")]

    public class EvidenciaNaoConformeApiController : BaseApiController
    {
        private readonly EvidenciaNaoConformeService _evidenciaNaoConformeService;

        public EvidenciaNaoConformeApiController(EvidenciaNaoConformeService evidenciaNaoConformeService)
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
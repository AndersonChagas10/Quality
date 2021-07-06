using DTO.PlanoDeAcao;
using Services.PlanoDeAcao;
using Services.PlanoDeAcao.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/EvidenciaNaoConformeApi")]

    public class EvidenciaNaoConformeApiController : BaseApiController
    {
        private readonly IEvidenciaNaoConformeService _evidenciaNaoConformeService;

        public EvidenciaNaoConformeApiController(IEvidenciaNaoConformeService evidenciaNaoConformeService)
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
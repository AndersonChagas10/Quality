using DTO.PlanoDeAcao;
using Services.PlanoDeAcao;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/EvidenciaNaoConformeApi")]

    public class EvidenciaNaoConformeApiController : BaseApiController
    {
        private readonly IEvidenciaNaoConformeService _evidenciaNaoConformeRepository;

        public EvidenciaNaoConformeApiController(IEvidenciaNaoConformeService evidenciaNaoConformeRepository)
        {
            _evidenciaNaoConformeRepository = evidenciaNaoConformeRepository;
        }

        [Route("GetFotosEvidencia/{id}")]
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidencia(int id)
        {
            return _evidenciaNaoConformeRepository.ObterFotosEvidencia(id);
        }
    }
}
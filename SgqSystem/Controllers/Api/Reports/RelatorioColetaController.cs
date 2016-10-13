using Application.Interface;
using Dominio.Interfaces.Services;
using DTO;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/RelatorioColeta")]
    public class RelatorioColetaController : ApiController
    {
        private readonly IRelatorioColetaDomain _relColetaDomain;

        public RelatorioColetaController(IRelatorioColetaDomain relColetaDomain)
        {
            _relColetaDomain = relColetaDomain;
        }

        [HttpPost]
        [Route("GetConsolidationLevel01")]
        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel01([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaDomain.GetConsolidationLevel01(form);
        }

        [HttpPost]
        [Route("GetConsolidationLevel02")]
        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel02([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaDomain.GetConsolidationLevel02(form);
        }

        [HttpPost]
        [Route("GetCollectionLevel02")]
        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel02([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaDomain.GetCollectionLevel02(form);
        }

        [HttpPost]
        [Route("GetCollectionLevel03")]
        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel03([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaDomain.GetCollectionLevel03(form);
        }

        [HttpPost]
        [Route("GetEntryByDate")]
        public GenericReturn<GetSyncDTO> GetEntryByDate([FromBody] FormularioParaRelatorioViewModel form)
        {
            var teste = _relColetaDomain.GetEntryByDate(form);
            return teste;
        }

    }
}

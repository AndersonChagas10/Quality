using Application.Interface;
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
        private readonly IRelatorioColetaApp _relColetaApp;

        public RelatorioColetaController(IRelatorioColetaApp relColetaApp)
        {
            _relColetaApp = relColetaApp;
        }

        [HttpPost]
        [Route("GetConsolidationLevel01")]
        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel01([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaApp.GetConsolidationLevel01(form);
        }

        [HttpPost]
        [Route("GetConsolidationLevel02")]
        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel02([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaApp.GetConsolidationLevel02(form);
        }

        [HttpPost]
        [Route("GetCollectionLevel02")]
        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel02([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaApp.GetCollectionLevel02(form);
        }

        [HttpPost]
        [Route("GetCollectionLevel03")]
        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel03([FromBody] FormularioParaRelatorioViewModel form)
        {
            return _relColetaApp.GetCollectionLevel03(form);
        }

        [HttpPost]
        [Route("GetEntryByDate")]
        public GenericReturn<GetSyncDTO> GetEntryByDate([FromBody] FormularioParaRelatorioViewModel form)
        {
            var teste = _relColetaApp.GetEntryByDate(form);
            return teste;
        }

    }
}

using Application.Interface;
using AutoMapper;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SyncController : ApiController
    {

        #region Construtor e atributos

        private readonly ISyncApp _syncApp;
        private readonly IColetaApp _coletaApp;

        public SyncController(ISyncApp syncApp, IColetaApp coletaApp)
        {
            _syncApp = syncApp;
            _coletaApp = coletaApp;
        }

        #endregion

        #region Envia Dados Para Syncronizar a plataforma remota

        [HttpPost]
        [Route("api/Sync/GetData")]
        public GenericReturnViewModel<SyncViewModel> GetDataToSincyAudit()
        {
            var queryDataToSync = _syncApp.GetDataToSincyAudit();
            var mappedToReturn = Mapper.Map<GenericReturn<SyncDTO>, GenericReturnViewModel<SyncViewModel>>(queryDataToSync);
            return mappedToReturn;
        }

        #endregion

        #region Recebe Dados Para Syncronizar o Db Interno e Web

        [HttpPost]
        [Route("api/Sync/SetDataAudit")]
        public GenericReturnViewModel<ColetaViewModel> SetDataToSincyAudit([FromBody] SyncViewModel objToSync)
        {
            var queryDataToSync = _coletaApp.SalvarLista(objToSync.Coleta);
            var mappedToReturn = Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryDataToSync);
            return mappedToReturn;
        }

        //[HttpPost]
        //[Route("api/Sync/SetDataCorrectiveAction")]
        //public GenericReturnViewModel<ColetaViewModel> SetDataToSincyCorrectiveAction([FromBody] SyncViewModel objToSync)
        //{
        //    var queryDataToSync = _coletaApp.SalvarLista(objToSync.Coleta);
        //    var mappedToReturn = Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryDataToSync);
        //    return mappedToReturn;
        //}
        #endregion

    }
}

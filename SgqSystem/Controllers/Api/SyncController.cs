using Application.Interface;
using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    public class SyncController : ApiController
    {
        private readonly ISyncApp _syncApp;

        public SyncController(ISyncApp syncApp)
        {
            _syncApp = syncApp;
        }

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
        [Route("api/Sync/SetData")]
        public GenericReturnViewModel<SyncViewModel> SetDataToSincyAudit([FromBody] SyncViewModel objToSync)
        {
            var queryDataToSync = _syncApp.SetDataToSincyAudit(objToSync);

            var mappedToReturn = Mapper.Map<GenericReturn<SyncDTO>, GenericReturnViewModel<SyncViewModel>>(queryDataToSync);
            return mappedToReturn;
        }

        #endregion
    }
}

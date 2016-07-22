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
        private readonly ISyncApp _coletaAppService;

        public SyncController(ISyncApp coletaAppService)
        {
            _coletaAppService = coletaAppService;
        }

        #region Envia Dados Para Syncronizar a plataforma remota

        [HttpPost]
        [Route("api/Sync/GetData")]
        public GenericReturnViewModel<SyncDTO> GetDataToSincyAudit()
        {
            var queryDataToSync = _coletaAppService.GetDataToSincyAudit();
            var mappedToReturn = Mapper.Map<GenericReturn<SyncDTO>, GenericReturnViewModel<SyncDTO>>(queryDataToSync);
            return mappedToReturn;
        }

        #endregion
    }
}

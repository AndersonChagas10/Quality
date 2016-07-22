using Application.Interface;
using AutoMapper;
using Dominio;
using DTO.DTO;
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
        public void GetDataToSincyAudit()
        {
            //var objReturn = new GenericReturnViewModel<SyncViewModel>(GetSync);
            _coletaAppService.GetDataToSincyAudit();
            //return objReturn;
        }

        #endregion
    }
}

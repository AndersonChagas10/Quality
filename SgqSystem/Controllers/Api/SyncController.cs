using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SyncController : ApiController
    {

        #region Construtor e atributos

        private readonly ISyncApp _syncApp;
        //private readonly IColetaApp _coletaApp;

        public SyncController(ISyncApp syncApp/*, IColetaApp coletaApp*/)
        {
            _syncApp = syncApp;
            //_coletaApp = coletaApp;
        }

        #endregion

        #region Envia Dados Para Syncronizar a plataforma remota

        //[HttpPost]
        //[Route("api/Sync/GetData")]
        //public GenericReturn<SyncDTO> GetDataToSincyAudit()
        //{
        //    return _syncApp.GetDataToSincyAudit();
        //}

        #endregion

        #region Recebe Dados Para Syncronizar o Db Interno e Web

        [HttpPost]
        [Route("api/Sync/SetDataAuditConsolidated")]
        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated([FromBody] SyncViewModel objToSync)
        {
            return _syncApp.SetDataToSincyAuditConsolidated(objToSync);
        }


        [HttpPost]
        [Route("api/Sync/GetLastEntry")]
        public GenericReturn<ColetaDTO> GetLastEntry()
        {
            return _syncApp.GetLastEntry();
        }

        #endregion

    }
}

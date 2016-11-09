﻿using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    /// <summary>
    /// Api para Coleta de Dados SgqGlobal.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SyncController : ApiController
    {

        #region Construtor e atributos

        private readonly ISaveConsolidateDataCollectionDomain _saveConsolidateDataCollectionDomain;
        private readonly IGetConsolidateDataCollectionDomain _getConsolidateDataCollectionDomain;
        
        public string UserSgqDTO { get; private set; }

        public SyncController(ISaveConsolidateDataCollectionDomain saveConsolidateDataCollectionDomain,
            IGetConsolidateDataCollectionDomain getConsolidateDataCollectionDomain)
        {
            _saveConsolidateDataCollectionDomain = saveConsolidateDataCollectionDomain;
            _getConsolidateDataCollectionDomain = getConsolidateDataCollectionDomain;
        }

        #endregion

        #region Data Collection

        /// <summary>
        /// Metodo do Api para salvar Consolidação lelve01, level2, collection level02 e level03, corrective action. 
        /// O Objeto SyncViewModel objToSync deve conter o parametro Root com a estrutura correta segundo definido no
        /// Banco de Dados para que o restanten do processo ocorra sem imprevistos.
        /// </summary>
        /// <param name="objToSync"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Sync/SetDataAuditConsolidated")]
        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated([FromBody] SyncViewModel objToSync)
        {
            return _saveConsolidateDataCollectionDomain.SetDataToSincyAuditConsolidated(objToSync);
        }

        #endregion

        #region Html

        /// <summary>
        /// Api para Salvar o html da div Results do SgqGlobal. O Objeto objToSync deve conter os seguintes parametros:
        /// objToSync.CollectionHtml.Period.
        /// objToSync.CollectionHtml.Shift.
        /// objToSync.CollectionHtml.CollectionDate.
        /// objToSync.CollectionHtml.UnitId.
        /// objToSync.html.
        /// </summary>
        /// <param name="objToSync"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Sync/SaveHtml")]
        public GenericReturn<SyncDTO> SaveHtml([FromBody] SyncViewModel objToSync)
        {
            var results = _saveConsolidateDataCollectionDomain.SaveHtml(objToSync);
            //unLock();
            return results;
        }

        /// <summary>
        /// Retorna a ultima div salva com parametros definidos para: Unidade e Shift.
        /// </summary>
        /// <param name="objToSync"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Sync/GetHtmlLastEntry")]
        public GenericReturn<GetSyncDTO> GetHtmlLastEntry([FromBody] SyncViewModel objToSync)
        {
            return _getConsolidateDataCollectionDomain.GetHtmlLastEntry(objToSync);
        }

        #endregion

        #region Bkp

        [HttpPost]
        [Route("api/Sync/SaveBkp")]
        public int SaveBkp([FromBody] BkpCollection bkpCollection)
        {
            return _saveConsolidateDataCollectionDomain.SaveBkp(bkpCollection);
        }

        #endregion

            #region Lock BETA TRAVA CLIENTE QUANDO ALGUM TABLET ESTIVER SINCRONIZANDO.

        [HttpPost]
        [Route("api/Sync/Lock")]
        public string Lock([FromBody] string lockable)
        {
            return lockable;
        }

        [HttpPost]
        [Route("api/Sync/unLock")]
        public string unLock()
        {
            ReqController.avaliable = null;
            return ReqController.avaliable;
        }

        [HttpPost]
        [Route("api/Sync/verifyUnlock")]
        public string verifyUnlock()
        {
            return ReqController.avaliable;
        }

        #endregion

    }
}

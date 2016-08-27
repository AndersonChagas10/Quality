using Application.Interface;
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

        public SyncController(ISyncApp syncApp)
        {
            _syncApp = syncApp;
        }

        #endregion

        #region Envia Dados Para Syncronizar a plataforma remota

        [HttpPost]
        [Route("api/Sync/SetDataAuditConsolidated")]
        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated([FromBody] SyncViewModel objToSync)
        {
            return _syncApp.SetDataToSincyAuditConsolidated(objToSync);
        }

        [HttpPost]
        [Route("api/Sync/SaveHtml")]
        public GenericReturn<SyncDTO> SaveHtml([FromBody]SyncViewModel objToSync)
        {
            return _syncApp.SaveHtml(objToSync);
        }

        #endregion

        #region Recebe Dados Para Syncronizar o Db Interno e Web

        [HttpPost]
        [Route("api/Sync/GetLastEntry")]
        public GenericReturn<GetSyncDTO> GetLastEntry()
        {
            var teste =  _syncApp.GetLastEntry();
            teste.Retorno.MakeHtml();
            
            return teste;
        }

        [HttpPost]
        [Route("api/Sync/GetHtmlLastEntry")]
        public GenericReturn<GetSyncDTO> GetHtmlLastEntry()
        {
            return _syncApp.GetHtmlLastEntry();
        }
        
        #endregion

    }
}

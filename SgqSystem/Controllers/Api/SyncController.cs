using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.Secirity;
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

        public string UserSgqDTO { get; private set; }

        public SyncController(ISyncApp syncApp)
        {
            _syncApp = syncApp;
        }

        #endregion

        #region Sync Data

        [HttpPost]
        [Route("api/Sync/SetDataAuditConsolidated")]
        public  GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated([FromBody] SyncViewModel objToSync)
        {
            //if (ReqController.avaliable.Equals(objToSync.lockPattern))
            //{
                var results = _syncApp.SetDataToSincyAuditConsolidated(objToSync);
                return results;
            //}
            //else
            //{
            //    return new GenericReturn<SyncDTO>("Wait");
            //}
        }

        #endregion

        #region Lock

        [HttpPost]
        [Route("api/Sync/Lock")]
        public string Lock([FromBody] string lockable)
        {
            //if (!(ReqController.avaliable == null))
            //    if (ReqController.avaliable.Equals(lockable))
            //        return ReqController.avaliable;

            //if (ReqController.avaliable == null)
            //{
            //    ReqController.avaliable = lockable;
            //    return ReqController.avaliable;
            //}
            //else
            //{
            //    return "wait";
            //}
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

        #region Html

        [HttpPost]
        [Route("api/Sync/SaveHtml")]
        public GenericReturn<SyncDTO> SaveHtml([FromBody] SyncViewModel objToSync)
        {
            //if (ReqController.avaliable.Equals(objToSync.lockPattern))
            //{
                var results = _syncApp.SaveHtml(objToSync);
                unLock();
                return results;
            //}
            //else
            //{
            //    return new GenericReturn<SyncDTO>("Wait");
            //}
        }

        [HttpPost]
        [Route("api/Sync/GetHtmlLastEntry")]
        public GenericReturn<GetSyncDTO> GetHtmlLastEntry([FromBody] SyncViewModel objToSync)
        {
            //if (ReqController.avaliable.Equals(objToSync.lockPattern))
            //{
                //objToSync.username = (User as CustomPrincipal).UserName;
            var results = _syncApp.GetHtmlLastEntry(objToSync);
            return results;
            //return "<div class=\"Results hide\">" + results + "</div>";
            //}
            //else
            //{
            //    return new GenericReturn<GetSyncDTO>("Wait");
            //}
        }

        //[HttpPost]
        //[Route("api/Sync/GetHtmlLastEntryNoLock")]
        //public GenericReturn<GetSyncDTO> GetHtmlLastEntryNoLock([FromBody]SyncViewModel objToSync)
        //{
        //    if (ReqController.avaliable.Equals(objToSync.lockPattern))
        //    {
        //        var results = _syncApp.GetHtmlLastEntry(objToSync.idUnidade);
        //        return results;
        //    }
        //    else
        //    {
        //        return new GenericReturn<GetSyncDTO>("Wait");
        //    }
        //}

        #endregion

        #region deprecated

        [HttpPost]
        [Route("api/Sync/GetLastEntry")]
        public GenericReturn<GetSyncDTO> GetLastEntry()
        {
            var teste = _syncApp.GetLastEntry();
            teste.Retorno.MakeHtml();
            return teste;
        }

        [HttpPost]
        [Route("api/Sync/SetFuncionaAgora")]
        public GenericReturn<SyncDTO> SetFuncionaAgora([FromBody] SyncViewModel objToSync)
        {
            return _syncApp.SetDataToSincyAuditConsolidated(objToSync);
        }

        #endregion

    }
}

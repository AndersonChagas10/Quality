using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using DTO.DTO;
using System;

namespace Application.AppServiceClass
{
    public class SyncApp : ISyncApp
    {

        //private readonly ISyncDomain _syncDomain;
        private readonly ISaveConsolidateDataCollectionDomain _saveConsolidateDataCollectionDomain;

        public SyncApp(/*ISyncDomain syncDomain,*/
            ISaveConsolidateDataCollectionDomain saveConsolidateDataCollectionDomain)
        {
            //_syncDomain = syncDomain;
            _saveConsolidateDataCollectionDomain = saveConsolidateDataCollectionDomain;
        }

        //public GenericReturn<SyncDTO> GetDataToSincyAudit()
        //{
        //    return _syncDomain.GetDataToSincyAudit();
        //}

        public GenericReturn<ObjectConsildationDTO> SetDataToSincyAuditConsolidated(ObjectConsildationDTO syncConsolidado)
        {
            return _saveConsolidateDataCollectionDomain.SetDataToSincyAuditConsolidated(syncConsolidado);
        }
    }
}

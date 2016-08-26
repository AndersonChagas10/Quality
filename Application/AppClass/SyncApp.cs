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
        private readonly IGetConsolidateDataCollectionDomain _getConsolidateDataCollectionDomain;

        public SyncApp(/*ISyncDomain syncDomain,*/
            ISaveConsolidateDataCollectionDomain saveConsolidateDataCollectionDomain,
            IGetConsolidateDataCollectionDomain getConsolidateDataCollectionDomain)
        {
            _getConsolidateDataCollectionDomain = getConsolidateDataCollectionDomain;
            _saveConsolidateDataCollectionDomain = saveConsolidateDataCollectionDomain;
        }

        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO syncConsolidado)
        {
            return _saveConsolidateDataCollectionDomain.SetDataToSincyAuditConsolidated(syncConsolidado);
        }

        public GenericReturn<GetSyncDTO> GetLastEntry()
        {
            return _getConsolidateDataCollectionDomain.GetLastEntry();
        }
    }
}

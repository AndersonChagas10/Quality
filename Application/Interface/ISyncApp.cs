﻿using DTO.DTO;
using DTO.Helpers;

namespace Application.Interface
{
    public interface ISyncApp
    {
        //GenericReturn<SyncDTO> GetDataToSincyAudit();
        GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO syncConsolidado);
        GenericReturn<GetSyncDTO> GetLastEntry();
        GenericReturn<SyncDTO> SaveHtml(SyncDTO objToSync);
        GenericReturn<GetSyncDTO> GetHtmlLastEntry(SyncDTO idUnidade);
    }
}

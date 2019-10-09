using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface ISaveConsolidateDataCollectionDomain
    {
        //ObjectConsildationDTO SendData();
        GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO syncConsolidado);
        GenericReturn<SyncDTO> SaveHtml(SyncDTO objToSync);
        int SaveBkp(BkpCollection bkpCollection);
        GenericReturn<CollectionJson> SaveFastJson(SyncDTO obj);
        void SetFullSave(List<int> listId);
    }
}

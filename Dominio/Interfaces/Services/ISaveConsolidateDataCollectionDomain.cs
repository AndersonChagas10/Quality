using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface ISaveConsolidateDataCollectionDomain
    {
        //ObjectConsildationDTO SendData();
        GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO syncConsolidado);
        GenericReturn<SyncDTO> SaveHtml(SyncDTO objToSync);
        int SaveBkp(BkpCollection bkpCollection);
    }
}

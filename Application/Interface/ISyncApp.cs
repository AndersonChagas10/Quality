using DTO.DTO;
using DTO.Helpers;

namespace Application.Interface
{
    public interface ISyncApp
    {
        //GenericReturn<SyncDTO> GetDataToSincyAudit();
        GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO syncConsolidado);

    }
}

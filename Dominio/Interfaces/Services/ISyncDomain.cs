using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface ISyncDomain
    {
        GenericReturn<SyncDTO> GetDataToSincyAudit();
        GenericReturn<SyncDTO> SetDataToSincyAudit(SyncDTO objToSync);
    }
}

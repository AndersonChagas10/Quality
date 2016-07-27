using DTO.DTO;
using System.Collections.Generic;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface ISaveConsolidateDataCollectionDomain
    {
        ObjectConsildationDTO SendData();
        GenericReturn<ObjectConsildationDTO> SetDataToSincyAuditConsolidated(ObjectConsildationDTO syncConsolidado);
    }
}

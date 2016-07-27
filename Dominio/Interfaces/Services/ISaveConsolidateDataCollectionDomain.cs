using DTO.DTO;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface ISaveConsolidateDataCollectionDomain
    {
        void RecieveData(ObjectConsildationDTO obj);
        ObjectConsildationDTO SendData();
    }
}

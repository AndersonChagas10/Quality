using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface IGetConsolidateDataCollectionDomain
    {
        GenericReturn<ColetaDTO> GetLastEntry();
    }
}

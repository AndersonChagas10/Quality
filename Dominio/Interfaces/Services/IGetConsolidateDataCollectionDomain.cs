using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IGetConsolidateDataCollectionDomain
    {
        List<ConsolidationLevel01> GetLastEntryToMerge();
        //GenericReturn<GetSyncDTO> GetLastEntry();
        GenericReturn<GetSyncDTO> GetHtmlLastEntry(SyncDTO idUnidade);
        //GenericReturn<GetSyncDTO> GetLastEntryByDate(DataCarrierFormulario form);
    }
}

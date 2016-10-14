using DTO;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IRelatorioColetaApp
    {
        //GenericReturn<List<ColetaDTO>> GetColetas();
        GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel01(DataCarrierFormulario form);
        GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel02(DataCarrierFormulario form);
        GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel02(DataCarrierFormulario form);
        GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel03(DataCarrierFormulario form);
        GenericReturn<GetSyncDTO> GetEntryByDate(DataCarrierFormulario form);
    }
}

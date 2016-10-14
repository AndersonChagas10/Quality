using DTO.Helpers;
using DTO.DTO;
using DTO;

namespace Dominio.Interfaces.Services
{
    public interface IRelatorioColetaDomain
    {
        GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel02(DataCarrierFormulario form);
        GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel03(DataCarrierFormulario form);
        GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel01(DataCarrierFormulario form);
        GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel02(DataCarrierFormulario form);

        GenericReturn<GetSyncDTO> GetEntryByDate(DataCarrierFormulario form);
    }
}

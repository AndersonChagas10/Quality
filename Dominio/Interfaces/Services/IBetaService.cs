using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IBetaService
    {
        GenericReturn<List<ResultOldDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ResultOldDTO>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ResultOldDTO>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd);
        GenericReturn<List<ResultOldDTO>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<ResultOldDTO> Salvar(ResultOldDTO r);
        GenericReturn<ResultOldDTO> SalvarLista(List<ResultOldDTO> list);
    }
}

using Dominio.Entities;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IRelatorioBetaAppService
    {
        GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId);
        GenericReturn<List<ResultOld>> GetNcPorMonitoramento(int indicadorId);
        GenericReturn<List<ResultOld>> GetNcPorTarefa(int indicadorId, int monitoramentoId);
    }
}

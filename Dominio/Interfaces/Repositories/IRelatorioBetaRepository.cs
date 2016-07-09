using Dominio.Entities;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IRelatorioBetaRepository
    {
        List<ResultOld> GetNcPorIndicador(int indicadorId);
        List<ResultOld> GetNcPorMonitoramento(int indicadorId);
        List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId);
    }
}

using Dominio.Entities;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IRelatorioBetaRepository
    {
        List<ResultOld> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        List<ResultOld> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd);
        List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd);
        List<ResultOld> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd);
    }
}

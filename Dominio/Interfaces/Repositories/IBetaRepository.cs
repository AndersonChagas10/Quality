using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Repositories
{
    public interface IBetaRepository
    {
        void Salvar(ResultOld r);
        void SalvarLista(List<ResultOld> list);
        List<ResultOld> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        List<ResultOld> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd);
        List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd);
        List<ResultOld> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd);
    }
}

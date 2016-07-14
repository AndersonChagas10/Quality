using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IBetaAppService
    {
        GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ResultOld>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ResultOld>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd);
        GenericReturn<List<ResultOld>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<ResultOld> Salvar(ResultOld r);
        GenericReturn<ResultOld> SalvarLista(List<ResultOld> list);
    }
}

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
        void Salvar(Coleta r);
        void SalvarLista(List<Coleta> list);
        List<Coleta> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        List<Coleta> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd);
        List<Coleta> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd);
        List<Coleta> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd);
        void ValidaFkResultado(Coleta r);
    }
}

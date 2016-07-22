using System.Collections.Generic;

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

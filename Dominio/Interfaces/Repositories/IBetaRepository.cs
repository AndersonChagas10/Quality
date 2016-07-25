using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IBetaRepository
    {
       
        List<Coleta> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        List<Coleta> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd);
        List<Coleta> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd);
        List<Coleta> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd);
    }
}

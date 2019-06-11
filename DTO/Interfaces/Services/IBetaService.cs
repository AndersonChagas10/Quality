using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IBetaService
    {
        GenericReturn<List<ColetaDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ColetaDTO>> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ColetaDTO>> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd);
        GenericReturn<List<ColetaDTO>> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd);
       
    }
}

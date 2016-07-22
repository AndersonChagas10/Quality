using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IBetaAppService
    {
        GenericReturn<List<ColetaDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ColetaDTO>> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<List<ColetaDTO>> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd);
        GenericReturn<List<ColetaDTO>> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd);
        GenericReturn<ColetaDTO> Salvar(ColetaDTO r);
        GenericReturn<ColetaDTO> SalvarLista(List<ColetaDTO> list);
    }
}

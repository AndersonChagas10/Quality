using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IColetaApp
    {
        GenericReturn<ColetaDTO> Salvar(ColetaDTO r);
        GenericReturn<ColetaDTO> SalvarLista(List<ColetaDTO> list);
    }
}

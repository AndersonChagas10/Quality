using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IColetaApp
    {
        GenericReturn<ColetaDTO> SalvarColeta(ColetaDTO r);
        GenericReturn<ColetaDTO> SalvarListaColeta(List<ColetaDTO> list);
    }
}

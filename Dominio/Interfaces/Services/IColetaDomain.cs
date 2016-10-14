using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IColetaDomain
    {
        GenericReturn<ColetaDTO> SalvarColeta(ColetaDTO r);
        GenericReturn<ColetaDTO> SalvarListaColeta(List<ColetaDTO> list);
    }
}

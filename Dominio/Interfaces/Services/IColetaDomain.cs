using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IColetaDomain
    {
        GenericReturn<ColetaDTO> Salvar(ColetaDTO r);
        GenericReturn<ColetaDTO> SalvarLista(List<ColetaDTO> list);
    }
}

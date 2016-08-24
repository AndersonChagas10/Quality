using System.Collections.Generic;
using DTO.Helpers;
using DTO.DTO;

namespace Dominio.Interfaces.Services
{
    public interface IRelatorioColetaDomain
    {
        GenericReturn<List<ColetaDTO>> GetColetas();
    }
}

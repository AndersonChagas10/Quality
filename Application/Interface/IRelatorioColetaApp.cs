using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IRelatorioColetaApp
    {
        GenericReturn<List<ColetaDTO>> GetColetas();
    }
}

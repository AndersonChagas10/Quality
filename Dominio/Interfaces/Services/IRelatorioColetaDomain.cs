using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface IRelatorioColetaDomain
    {
        GenericReturn<List<ColetaDTO>> GetColetas();
    }
}

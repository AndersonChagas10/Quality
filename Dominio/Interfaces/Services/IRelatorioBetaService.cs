using Dominio.Entities;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IRelatorioBetaService
    {
        GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId);
    }
}

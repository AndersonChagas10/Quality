using Dominio.Entities;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IRelatorioBetaAppService
    {
        GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId);
    }
}

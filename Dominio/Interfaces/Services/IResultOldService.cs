using System.Collections.Generic;
using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    /// <summary>
    /// Interface a ser implementada na camada de Aplicação.
    /// </summary>
    public interface IResultOldService
    {
        GenericReturn<ResultOld> Salvar(ResultOld r);
        GenericReturn<ResultOld> SalvarLista(List<ResultOld> list);
    }
}

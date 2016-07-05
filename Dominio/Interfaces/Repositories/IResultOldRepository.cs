using Dominio.Entities;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IResultOldRepository
    {
        void Salvar(ResultOld r);
        void SalvarLista(List<ResultOld> list);
    }
}

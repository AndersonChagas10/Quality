using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IResultOldRepository
    {

        GenericReturn<ResultOld> Salvar(ResultOld r);

    }
}

using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IResultOldService
    {
        GenericReturn<ResultOld> Salvar(ResultOld r);
    }
}

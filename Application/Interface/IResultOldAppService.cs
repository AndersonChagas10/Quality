using Dominio.Entities;

namespace Application.Interface
{
    public interface IResultOldAppService
    {
        GenericReturn<ResultOld> Salvar(ResultOld r);
    }
}

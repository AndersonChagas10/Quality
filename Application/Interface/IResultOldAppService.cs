using Dominio.Entities;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IResultOldAppService
    {
        GenericReturn<ResultOld> Salvar(ResultOld r);
        GenericReturn<ResultOld> SalvarLista(List<ResultOld> list);
    }
}

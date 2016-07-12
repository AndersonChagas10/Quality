using Dominio.Interfaces.Services;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System.Collections.Generic;

namespace Dominio.Services
{
    public class ResultOldService : IResultOldService
    {

        private IResultOldRepository _repoResultOld;

        public ResultOldService(IResultOldRepository repoResultOld)
        {
            _repoResultOld = repoResultOld;
        }

        public object DbFunction { get; private set; }

        public GenericReturn<ResultOld> Salvar(ResultOld r)
        {
            _repoResultOld.Salvar(r);
            return new GenericReturn<ResultOld>("Your data has been successfully saved.");//Resultado Inserido com sucesso
        }

        public GenericReturn<ResultOld> SalvarLista(List<ResultOld> list)
        {
            _repoResultOld.SalvarLista(list);
            return new GenericReturn<ResultOld>("Your data has been successfully saved.");//Resultado Inserido com sucesso
        }
    }
}

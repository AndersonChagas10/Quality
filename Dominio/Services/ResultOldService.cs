using Dominio.Interfaces.Services;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;

namespace Dominio.Services
{
    public class ResultOldService : IResultOldService
    {

        private IResultOldRepository _repoResultOld;

        public ResultOldService(IResultOldRepository repoResultOld)
        {
            _repoResultOld = repoResultOld;
        }

        public GenericReturn<ResultOld> Salvar(ResultOld r)
        {
            return _repoResultOld.Salvar(r);
        }
    }
}

using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Services;

namespace Application
{
    public class ResultOldAppService : IResultOldAppService
    {

        private readonly IResultOldService _resultOldService;

        public ResultOldAppService(IResultOldService resultOldService)
        {
            _resultOldService = resultOldService;
        }

        public GenericReturn<ResultOld> Salvar(ResultOld r)
        {
            return _resultOldService.Salvar(r);
        }

    }
}

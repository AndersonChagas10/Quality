using Application;
using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using System.Collections.Generic;

namespace Application
{
    public class ResultOldAppService :  IResultOldAppService
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

        public GenericReturn<ResultOld> SalvarLista(List<ResultOld> list)
        {
            return _resultOldService.SalvarLista(list);
        }

    }
}

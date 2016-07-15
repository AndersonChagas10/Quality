using Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using DTO.Helpers;

namespace Application.AppServiceClass
{
    public class BetaAppService : IBetaAppService
    {

        private readonly IBetaService _betaService;

        public BetaAppService(IBetaService betaService)
        {
            _betaService = betaService;
        }

        #region Coleta De Dados

        public GenericReturn<ResultOld> Salvar(ResultOld r)
        {
            return _betaService.Salvar(r);
        }

        public GenericReturn<ResultOld> SalvarLista(List<ResultOld> list)
        {
            return _betaService.SalvarLista(list);
        }

        #endregion

        #region Busca De Dados

        public GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorMonitoramento(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorMonitoramentoJelsafa(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ResultOld>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorTarefa(indicadorId, monitoramentoId, dateInit, dateEnd);
        }

        #endregion

    }
}

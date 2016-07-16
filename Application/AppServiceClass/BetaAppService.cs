using Application.Interface;
using System.Collections.Generic;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using DTO.DTO;

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

        public GenericReturn<ResultOldDTO> Salvar(ResultOldDTO r)
        {
            return _betaService.Salvar(r);
        }

        public GenericReturn<ResultOldDTO> SalvarLista(List<ResultOldDTO> list)
        {
            return _betaService.SalvarLista(list);
        }

        #endregion

        #region Busca De Dados

        public GenericReturn<List<ResultOldDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ResultOldDTO>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorMonitoramento(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ResultOldDTO>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorMonitoramentoJelsafa(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ResultOldDTO>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorTarefa(indicadorId, monitoramentoId, dateInit, dateEnd);
        }

        #endregion

    }
}

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

        public GenericReturn<ColetaDTO> Salvar(ColetaDTO r)
        {
            return _betaService.Salvar(r);
        }

        public GenericReturn<ColetaDTO> SalvarLista(List<ColetaDTO> list)
        {
            return _betaService.SalvarLista(list);
        }

        #endregion

        #region Busca De Dados

        public GenericReturn<List<ColetaDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ColetaDTO>> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorLevel2(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ColetaDTO>> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorLevel2Jelsafa(indicadorId, dateInit, dateEnd);
        }

        public GenericReturn<List<ColetaDTO>> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd)
        {
            return _betaService.GetNcPorLevel3(indicadorId, Level2Id, dateInit, dateEnd);
        }

        #endregion

    }
}

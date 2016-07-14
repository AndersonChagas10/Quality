using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Services
{
    public class BetaService : IBetaService
    {
        private IBetaRepository _betaService;

        public BetaService(IBetaRepository relatorioBetaService)
        {
            _betaService = relatorioBetaService;
        }

        #region Coleta de Dados

        public GenericReturn<ResultOld> Salvar(ResultOld r)
        {
            _betaService.Salvar(r);
            return new GenericReturn<ResultOld>("Your data has been successfully saved.");
        }

        public GenericReturn<ResultOld> SalvarLista(List<ResultOld> list)
        {
            _betaService.SalvarLista(list);
            return new GenericReturn<ResultOld>("Your data has been successfully saved.");
        }

        #endregion

        #region Busca De Dados

        public GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {

            var retornoRepositorio = _betaService.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
            if (retornoRepositorio.Count == 0)
                throw new ExceptionHelper("No data.");

            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {
            var retornoRepositorio = _betaService.GetNcPorMonitoramento(indicadorId, dateInit, dateEnd);
            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }

        public GenericReturn<List<ResultOld>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {
            var retornoRepositorio = _betaService.GetNcPorTarefa(indicadorId, monitoramentoId, dateInit, dateEnd);
            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            var retornoRepositorio = _betaService.GetNcPorMonitoramentoJelsafa(indicadorId, dateInit, dateEnd);
            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        } 

        #endregion

    }
}

using System;
using System.Collections.Generic;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using Dominio.Interfaces.Repositories;

namespace Dominio.Services
{
    public class RelatorioBetaService : IRelatorioBetaService
    {

        private IRelatorioBetaRepository _relatorioBetaService;

        public RelatorioBetaService(IRelatorioBetaRepository relatorioBetaService)
        {
            _relatorioBetaService = relatorioBetaService;
        }

        public GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {

            var retornoRepositorio = _relatorioBetaService.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
            if (retornoRepositorio.Count == 0)
                throw new ExceptionHelper("No data.");

            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {
            var retornoRepositorio = _relatorioBetaService.GetNcPorMonitoramento(indicadorId, dateInit, dateEnd);
            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }

        public GenericReturn<List<ResultOld>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {
            var retornoRepositorio = _relatorioBetaService.GetNcPorTarefa(indicadorId, monitoramentoId, dateInit, dateEnd);
            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            var retornoRepositorio = _relatorioBetaService.GetNcPorMonitoramentoJelsafa(indicadorId, dateInit, dateEnd);
            var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
            return retorno;
        }
    }
}

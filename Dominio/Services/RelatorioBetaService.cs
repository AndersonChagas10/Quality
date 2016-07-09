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

        public GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId)
        {
            try
            {
                var retornoRepositorio = _relatorioBetaService.GetNcPorIndicador(indicadorId);
                var retorno = new GenericReturn<List<ResultOld>>(retornoRepositorio);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new ExceptionHelper("Não foi possível gerar o relatório de Indicadores.", ex);
            }

        }
    }
}

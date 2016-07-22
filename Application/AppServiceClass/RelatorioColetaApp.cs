using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Services;
using System.Collections.Generic;

namespace Application.AppServiceClass
{
    public class RelatorioColetaApp : IRelatorioColetaApp
    {
        private readonly IRelatorioColetaDomain _relatorioColetaDomain;

        public RelatorioColetaApp(IRelatorioColetaDomain relatorioColetaDomain)
        {
            _relatorioColetaDomain = relatorioColetaDomain;
        }

        public GenericReturn<List<ColetaDTO>> GetColetas()
        {
            return _relatorioColetaDomain.GetColetas();
        }
    }
}

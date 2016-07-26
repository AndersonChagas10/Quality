using Application.Interface;
using System.Collections.Generic;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Services;

namespace Application.AppServiceClass
{
    public class ColetaApp : IColetaApp
    {
        private readonly IColetaDomain _coletaService;

        public ColetaApp(IColetaDomain coletaService)
        {
            _coletaService = coletaService;
        }

        public GenericReturn<ColetaDTO> SalvarColeta(ColetaDTO r)
        {
            return _coletaService.SalvarColeta(r);
        }

        public GenericReturn<ColetaDTO> SalvarListaColeta(List<ColetaDTO> list)
        {
            return _coletaService.SalvarListaColeta(list);
        }
    }
}

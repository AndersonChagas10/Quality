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

        public GenericReturn<ColetaDTO> Salvar(ColetaDTO r)
        {
            return _coletaService.Salvar(r);
        }

        public GenericReturn<ColetaDTO> SalvarLista(List<ColetaDTO> list)
        {
            return _coletaService.SalvarLista(list);
        }
    }
}

using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Services;
using System.Collections.Generic;
using DTO;
using System;

namespace Application.AppServiceClass
{
    public class RelatorioColetaApp : IRelatorioColetaApp
    {
        private readonly IRelatorioColetaDomain _relatorioColetaDomain;

        public RelatorioColetaApp(IRelatorioColetaDomain relatorioColetaDomain)
        {
            _relatorioColetaDomain = relatorioColetaDomain;
        }

        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel02(DataCarrierFormulario form)
        {
            return _relatorioColetaDomain.GetCollectionLevel02(form);
        }

        public GenericReturn<ResultSetRelatorioColeta> GetCollectionLevel03(DataCarrierFormulario form)
        {
            return _relatorioColetaDomain.GetCollectionLevel03(form);
        }

        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel01(DataCarrierFormulario form)
        {
            return _relatorioColetaDomain.GetConsolidationLevel01(form);
        }

        public GenericReturn<ResultSetRelatorioColeta> GetConsolidationLevel02(DataCarrierFormulario form)
        {
            return _relatorioColetaDomain.GetConsolidationLevel02(form);
        }

        public GenericReturn<GetSyncDTO> GetEntryByDate(DataCarrierFormulario form)
        {
            return _relatorioColetaDomain.GetEntryByDate(form);
        }
    }
}

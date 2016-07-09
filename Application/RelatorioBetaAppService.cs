﻿using System;
using System.Collections.Generic;
using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Services;

namespace Application
{
    public class RelatorioBetaAppService : IRelatorioBetaAppService
    {
        private readonly IRelatorioBetaService _relatorioBetaService;

        public RelatorioBetaAppService(IRelatorioBetaService relatorioBetaService)
        {
            _relatorioBetaService = relatorioBetaService;
        }


        public GenericReturn<List<ResultOld>> GetNcPorIndicador(int indicadorId)
        {
            return _relatorioBetaService.GetNcPorIndicador(indicadorId);
        }

        public GenericReturn<List<ResultOld>> GetNcPorMonitoramento(int indicadorId)
        {
            return _relatorioBetaService.GetNcPorMonitoramento(indicadorId);
        }

        public GenericReturn<List<ResultOld>> GetNcPorTarefa(int indicadorId, int monitoramentoId)
        {
            return _relatorioBetaService.GetNcPorTarefa(indicadorId, monitoramentoId);
        }
    }
}

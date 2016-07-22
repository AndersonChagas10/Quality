using System;
using Application.Interface;
using Dominio.Interfaces.Services;

namespace Application.AppServiceClass
{
    public class SyncApp : ISyncApp
    {
        private readonly ISyncDomain _coletaService;

        public SyncApp(ISyncDomain coletaService)
        {
            _coletaService = coletaService;
        }

        public void GetDataToSincyAudit()
        {
            _coletaService.GetDataToSincyAudit();
        }
    }
}

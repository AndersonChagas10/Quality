using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using DTO.DTO;
using System;

namespace Application.AppServiceClass
{
    public class SyncApp : ISyncApp
    {

        private readonly ISyncDomain _coletaService;

        public SyncApp(ISyncDomain coletaService)
        {
            _coletaService = coletaService;
        }

        public GenericReturn<SyncDTO> GetDataToSincyAudit()
        {
            return _coletaService.GetDataToSincyAudit();
        }

        public GenericReturn<SyncDTO> SetDataToSincyAudit(SyncDTO objToSync)
        {
            return _coletaService.SetDataToSincyAudit(objToSync);
        }
    }
}

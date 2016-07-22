using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using DTO.DTO;

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

    }
}

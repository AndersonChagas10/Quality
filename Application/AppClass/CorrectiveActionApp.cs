using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;

namespace Application.AppServiceClass
{
    public class CorrectiveActionApp : ICorrectiveActionApp
    {
        #region Contrutor

        private readonly ICorrectiveActionDomain _correctiveActionService;

        public CorrectiveActionApp(ICorrectiveActionDomain correctiveActionService)
        {
            _correctiveActionService = correctiveActionService;
        }

        #endregion

        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto)
        {
            return _correctiveActionService.SalvarAcaoCorretiva(dto);
        }

        public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto)
        {
            return _correctiveActionService.VerificarAcaoCorretivaIncompleta(dto);
        }

    }
}

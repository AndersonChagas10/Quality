using Application.Interface;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;

namespace Application.AppServiceClass
{
    public class CorrectiveActionAppService : ICorrectiveActionAppService
    {
        #region Contrutor
        private readonly ICorrectiveActionService _correctiveActionService;

        public CorrectiveActionAppService(ICorrectiveActionService correctiveActionService)
        {
            _correctiveActionService = correctiveActionService;
        }
        #endregion

        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto)
        {
            return _correctiveActionService.SalvarAcaoCorretiva(dto);
        }

    }
}

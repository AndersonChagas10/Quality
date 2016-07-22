using DTO.DTO;
using DTO.Helpers;

namespace Application.Interface
{
    public interface ICorrectiveActionAppService
    {
        GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto);

    }
}

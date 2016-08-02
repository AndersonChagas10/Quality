using DTO.DTO;
using DTO.Helpers;

namespace Application.Interface
{
    public interface ICorrectiveActionApp
    {
        GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto);

        GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto);

    }
}

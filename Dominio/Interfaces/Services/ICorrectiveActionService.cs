using DTO.DTO;
using DTO.Helpers;

namespace Dominio.Interfaces.Services
{
    public interface ICorrectiveActionService
    {

        GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto);

        GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto);

    }

}

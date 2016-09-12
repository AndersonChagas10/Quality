using DTO;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface ICorrectiveActionApp
    {
        GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto);

        GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto);
        GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction(DataCarrierFormulario data);
        GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById(int id);
    }
}

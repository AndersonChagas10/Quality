using DTO;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface ICorrectiveActionDomain
    {

        GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto);

        GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto);

        GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction(DataCarrierFormulario data);
        GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById(int id);
    }

}

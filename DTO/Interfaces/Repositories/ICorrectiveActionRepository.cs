using DTO;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface ICorrectiveActionRepository
    {
        CorrectiveAction SalvarAcaoCorretiva(CorrectiveAction entitie);

        IEnumerable<CorrectiveAction> GetCorrectiveAction(DataCarrierFormulario data);

        //CorrectiveActionLevels SalvarAcaoCorretivaLevels(CorrectiveActionLevels entitie);

        CorrectiveAction VerificarAcaoCorretivaIncompleta(CorrectiveAction entitie);

    }
}

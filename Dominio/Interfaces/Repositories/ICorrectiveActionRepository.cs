namespace Dominio.Interfaces.Repositories
{
    public interface ICorrectiveActionRepository
    {
        CorrectiveAction SalvarAcaoCorretiva(CorrectiveAction entitie);

        //CorrectiveActionLevels SalvarAcaoCorretivaLevels(CorrectiveActionLevels entitie);

        CorrectiveAction VerificarAcaoCorretivaIncompleta(CorrectiveAction entitie);

    }
}

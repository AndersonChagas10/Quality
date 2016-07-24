namespace Dominio.Interfaces.Repositories
{
    public interface ICorrectiveActionRepository
    {
        void SalvarAcaoCorretiva(CorrectiveAction entitie);

        CorrectiveAction VerificarAcaoCorretivaIncompleta(CorrectiveAction entitie);

    }
}

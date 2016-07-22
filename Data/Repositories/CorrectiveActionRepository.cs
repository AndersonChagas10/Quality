using Dominio;
using Dominio.Interfaces.Repositories;

namespace Data.Repositories
{
    public class CorrectiveActionRepository : RepositoryBase<CorrectiveAction>, ICorrectiveActionRepository
    {

        public CorrectiveActionRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }


        public void SalvarAcaoCorretiva(CorrectiveAction entitie)
        {
            AddOrUpdate(entitie);
        }
    }
}

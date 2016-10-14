using Dominio;
using Dominio.Interfaces.Repositories.DataCollectionDomainRepositorie;

namespace Data.Repositories.DataCollectionRepo
{
    public class Level01Consolidation : RepositoryBase<DataCollection>, ILevel01ConsolidationRepository
    {
        public Level01Consolidation(SgqDbDevEntities Db) : base(Db)
        {
        }

    }
}

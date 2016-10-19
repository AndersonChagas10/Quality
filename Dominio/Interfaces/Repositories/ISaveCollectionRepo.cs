using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface ISaveCollectionRepo
    {
        void SaveAllLevel(List<ConsolidationLevel01> listConsolidationLelve1, 
            List<ConsolidationLevel02> listConsolidationLelve2,
            List<CollectionLevel02> listCollectionLelve2,
            List<CollectionLevel03> listCollectionLelve3,
            List<CorrectiveAction> listCorrectiveAction
            );
    }
}

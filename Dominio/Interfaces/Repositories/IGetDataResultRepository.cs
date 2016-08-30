using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IGetDataResultRepository<T> where T : class
    {
        IEnumerable<ConsolidationLevel01> GetLastEntryConsildatedLevel01();
        IEnumerable<ConsolidationLevel02> GetLastEntryConsildatedLevel02(IEnumerable<ConsolidationLevel01> cl1);
        IEnumerable<CollectionLevel02> GetLastEntryCollectionLevel02(IEnumerable<ConsolidationLevel02> cl2);
        IEnumerable<CollectionLevel03> GetLastEntryCollectionLevel03(IEnumerable<CollectionLevel02> cll2);
        CollectionHtml GetHtmlLastEntry();

        void SetDuplicated(CollectionLevel02 i);
        void SetDuplicated(List<CollectionLevel03> cll3, CollectionLevel02 collectionLevel02);
        int GetExistentLevel01Consollidation(ConsolidationLevel01 level01Consolidation);
        int GetExistentLevel02Consollidation(ConsolidationLevel02 level02Consolidation);
    }
}

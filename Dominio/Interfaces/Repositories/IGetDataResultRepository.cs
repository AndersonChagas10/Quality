using DTO;
using DTO.DTO;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IGetDataResultRepository<T> where T : class
    {
        IEnumerable<ConsolidationLevel01> GetLastEntryConsildatedLevel01();
        IEnumerable<ConsolidationLevel02> GetLastEntryConsildatedLevel02(IEnumerable<ConsolidationLevel01> cl1);
        IEnumerable<CollectionLevel02> GetLastEntryCollectionLevel02(IEnumerable<ConsolidationLevel02> cl2);
        IEnumerable<CollectionLevel03> GetLastEntryCollectionLevel03(IEnumerable<CollectionLevel02> cll2);
        CollectionHtml GetHtmlLastEntry(SyncDTO idUnidade);

        //IEnumerable<ConsolidationLevel01> GetEntryConsildatedLevel01ByDateAndUnit(DataCarrierFormulario form);
        //IEnumerable<ConsolidationLevel02>  GetEntryConsildatedLevel02ByDateAndUnit(IEnumerable<ConsolidationLevel01> consildatedLelve01);
        //IEnumerable<CollectionLevel02> GetEntryCollectionLevel02ByDateAndUnit(IEnumerable<ConsolidationLevel02> cl2);
        //IEnumerable<CollectionLevel03> GetEntryCollectionLevel03ByDateAndUnit(IEnumerable<CollectionLevel02> cll2);

        void SetDuplicated(CollectionLevel02 i);
        void SetDuplicated(List<CollectionLevel03> cll3, CollectionLevel02 collectionLevel02);
        int GetExistentLevel01Consollidation(ConsolidationLevel01 level01Consolidation);
        int GetExistentLevel02Consollidation(ConsolidationLevel02 level02Consolidation);

        void Remove(int id);
    }
}

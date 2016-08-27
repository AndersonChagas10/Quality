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
    }
}

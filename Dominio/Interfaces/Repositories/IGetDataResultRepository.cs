using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IGetDataResultRepository<T> where T : class
    {
        List<ConsolidationLevel01> GetLastEntryConsildatedLevel01();
        List<ConsolidationLevel02> GetLastEntryConsildatedLevel02();
        List<CollectionLevel02> GetLastEntryCollectionLevel02();
        List<CollectionLevel03> GetLastEntryCollectionLevel03();
    }
}

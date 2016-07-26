using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories.DataCollectionDomainRepositorie
{
    public interface IDataCollectionHeadRepository<T>
    {
        void SaveDataCollectionHead(T obj);
        void SaveDataCollectionHead(List<T> ListObj);
        void SaveOrUpdateDataCollectionHead(T obj);
        void SaveOrUpdateDataCollectionHead(List<T> ListObj);
        void UpdateDataCollectionHead(List<T> ListObj);
        void UpdateDataCollectionHead(T obj);
    }
}

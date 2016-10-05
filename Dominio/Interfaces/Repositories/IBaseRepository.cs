using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        
        void Add(T obj);

        void AddNotCommit(T obj);

        void AddAll(IEnumerable<T> obj);

        void AddOrUpdateAll(IEnumerable<T> obj);

        void AddAllNotCommit(IEnumerable<T> obj);

        void AddOrUpdateAllNotCommit(IEnumerable<T> obj);

        T GetById(int id);

        IEnumerable<T> GetAll();

        void Update(T obj);

        void UpdateAll(IEnumerable<T> listObj);

        void UpdateNotCommit(T obj);
        
        void Remove(T obj);

        void Delete(int id);

        void RemoveAll(IEnumerable<T> obj);

        T First();

        void AddOrUpdate(T obj);

        void AddOrUpdateNotCommit(T obj);
        
        void Commit();

        void Dispose();

        void Dettach(T obj);
    }
}

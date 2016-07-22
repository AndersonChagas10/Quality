using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        
        void Add(T obj);

        void AddAll(IEnumerable<T> obj);

        T GetById(int id);

        IEnumerable<T> GetAll();

        void Update(T obj);

        void Remove(T obj);

        void Delete(int id);

        void RemoveAll(IEnumerable<T> obj);

        T First();

        void AddOrUpdate(T obj);

        void Commit();

        void Dispose();

    }
}

using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface IServiceWithLog<T>
    {
        void Add(T obj);
        void AddAll(IEnumerable<T> obj);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Update(T obj);
        void UpdateWithLog(T obj);
        void UpdateAll(IEnumerable<T> listObj);
        void Remove(T obj);
        void RemoveAll(IEnumerable<T> obj);
    }
}

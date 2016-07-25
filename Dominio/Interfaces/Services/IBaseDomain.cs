using Dominio.Entities.BaseEntity;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    public interface IBaseDomain<T> where T : class
    {

        GenericReturn<T> Add(T obj);

        GenericReturn<T> AddAll(IEnumerable<T> obj);

        T GetById(int id);

        IEnumerable<T> GetAll();

        GenericReturn<T> Update(T obj);

        void Remove(T obj);

        void Dispose();

        void Delete(int id);

        void RemoveAll(IEnumerable<T> obj);

        T First();

        GenericReturn<T> AddOrUpdate(T obj);


    }
}

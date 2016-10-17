using Dominio.Entities.BaseEntity;
using DTO.Helpers;
using System.Collections.Generic;

namespace Dominio.Interfaces.Services
{
    /// <summary>
    /// Passa base repo dinamica.
    /// </summary>
    /// <typeparam name="T">Domain</typeparam>
    /// <typeparam name="Y">DTO</typeparam>
    public interface IBaseDomain<T, Y> where T : class where Y : class
    {

        //GenericReturn<T> Add(T obj);

        //GenericReturn<T> AddAll(IEnumerable<T> obj);

        Y GetById(int id);

        IEnumerable<Y> GetAll();

        //GenericReturn<T> Update(T obj);

        //void Remove(T obj);

        //void Dispose();

        //void Delete(int id);

        //void RemoveAll(IEnumerable<T> obj);

        Y First();

        //GenericReturn<T> AddOrUpdate(T obj);


    }
}

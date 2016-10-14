using Application.Interface;
using Dominio.Entities.BaseEntity;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace Application.AppServiceClass
{
    public class AppServiceBase<T, Y> : IDisposable, IBaseApp<T, Y> where T : class where Y : class
    {

        private readonly IBaseDomain<T, Y> _serviceDomain;

        public AppServiceBase(IBaseDomain<T, Y> serviceDomain)
        {
            _serviceDomain = serviceDomain;
        }

        #region Salvar

        public GenericReturn<T> Add(T obj)
        {
            return _serviceDomain.Add(obj);
        }

        public GenericReturn<T> AddAll(IEnumerable<T> obj)
        {
            return _serviceDomain.AddAll(obj);
        }

        public GenericReturn<T> AddOrUpdate(T obj)
        {
            return _serviceDomain.AddOrUpdate(obj);
        }

        public GenericReturn<T> Update(T obj)
        {
            return _serviceDomain.Update(obj);
        }

        #endregion

        public T GetById(int id)
        {
            return _serviceDomain.GetById(id);
        }

        public IEnumerable<Y> GetAll()
        {
            return _serviceDomain.GetAll();
        }

        public void Remove(T obj)
        {
            _serviceDomain.Remove(obj);
        }

        public void Dispose()
        {
            _serviceDomain.Dispose();
        }

        public void Delete(int id)
        {
            _serviceDomain.Delete(id);
        }

        public void RemoveAll(IEnumerable<T> obj)
        {
            _serviceDomain.RemoveAll(obj);
        }

        public T First()
        {
            return _serviceDomain.First();
        }

    }
}

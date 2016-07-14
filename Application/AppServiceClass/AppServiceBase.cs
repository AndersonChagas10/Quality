using Application.Interface;
using Dominio.Entities;
using Dominio.Entities.BaseEntity;
using Dominio.Helpers;
using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Application.AppServiceClass
{
    public class AppServiceBase<T> : IDisposable, IAppServiceBase<T> where T : EntityBase
    {

        private readonly IServiceBase<T> _serviceBase;

        public AppServiceBase(IServiceBase<T> serviceBase)
        {
            _serviceBase = serviceBase;
        }

        #region Salvar

        public GenericReturn<T> Add(T obj)
        {
            return _serviceBase.Add(obj);
        }

        public GenericReturn<T> AddAll(IEnumerable<T> obj)
        {
            return _serviceBase.AddAll(obj);
        }

        public GenericReturn<T> AddOrUpdate(T obj)
        {
            return _serviceBase.AddOrUpdate(obj);
        }

        public GenericReturn<T> Update(T obj)
        {
            return _serviceBase.Update(obj);
        }

        #endregion

        public T GetById(int id)
        {
            return _serviceBase.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _serviceBase.GetAll();
        }

        public void Remove(T obj)
        {
            _serviceBase.Remove(obj);
        }

        public void Dispose()
        {
            _serviceBase.Dispose();
        }

        public void Delete(int id)
        {
            _serviceBase.Delete(id);
        }

        public void RemoveAll(IEnumerable<T> obj)
        {
            _serviceBase.RemoveAll(obj);
        }

        public T First()
        {
            return _serviceBase.First();
        }

    }
}

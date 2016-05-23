using Application.Interface;
using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class AppServiceBase<T> : IDisposable, IAppServiceBase<T> where T : class
    {

        private readonly IServiceBase<T> _serviceBase;

        public AppServiceBase(IServiceBase<T> serviceBase) 
        {
            _serviceBase = serviceBase;
        }
        public void Add(T obj)
        {
            _serviceBase.Add(obj);
        }

        public T GetById(int id)
        {
            return _serviceBase.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _serviceBase.GetAll();
        }

        public void Update(T obj)
        {
            _serviceBase.Update(obj);
        }

        public void Remove(T obj)
        {
            _serviceBase.Remove(obj);
        }

        public void Dispose()
        {
            _serviceBase.Dispose();
        }

    }
}

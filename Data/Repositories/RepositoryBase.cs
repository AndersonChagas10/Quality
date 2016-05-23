using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {

        public void Add(T obj)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T obj)
        {
            throw new NotImplementedException();
        }

        public void Remove(T obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

    }
}

using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Application.Parametrizacao
{
    public abstract class BaseService<T> : IService<T> where T : IEntity //IEntity
    {
        protected IRepositoryNoLazyLoad<T> _repository;
        public BaseService(IRepositoryNoLazyLoad<T> repository)
        {
            _repository = repository;
        }

        public virtual void Add(T entity)
        {
            _repository.Add(entity);
        }

        public virtual void AddAll(IEnumerable<T> entity)
        {
            _repository.AddAll(entity);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual void Remove(T entity)
        {
            _repository.Remove(entity);
        }

        public virtual void RemoveAll(IEnumerable<T> entityList)
        {
            _repository.RemoveAll(entityList);
        }

        public virtual void Update(T entity)
        {
            _repository.Update(entity);
        }

        public virtual void UpdateAll(IEnumerable<T> entityList)
        {
            _repository.UpdateAll(entityList);
        }
    }
}

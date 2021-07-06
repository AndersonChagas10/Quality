using Conformity.Application.Core.Log;
using Conformity.Domain.Core.Interfaces;
using System.Collections.Generic;

namespace Conformity.Application.Core
{
    public abstract class BaseServiceWithLog<T> : IServiceWithLog<T> where T : IEntity //IEntity
    {
        protected IRepositoryNoLazyLoad<T> _repository;
        protected EntityTrackService _entityTrackService;
        public BaseServiceWithLog(IRepositoryNoLazyLoad<T> repository
            , EntityTrackService entityTrackService)
        {
            _repository = repository;
            _entityTrackService = entityTrackService;
        }

        public virtual void Add(T entity)
        {
            _repository.Add(entity);
            _entityTrackService.RegisterCreate(entity);
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

        public virtual void UpdateWithLog(T entity)
        {
            T dbEntity = GetById(((IEntity)entity).Id);
            _repository.Update(entity);
            _entityTrackService.RegisterUpdate(dbEntity, entity);
        }

        public virtual void UpdateAll(IEnumerable<T> entityList)
        {
            _repository.UpdateAll(entityList);
        }
    }
}

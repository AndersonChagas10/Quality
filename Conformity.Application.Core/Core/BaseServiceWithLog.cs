using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Application.Core.Core
{
    public class BaseServiceWithLog<T> : IServiceWithLog<T> where T : class //IEntity
    {
        protected IRepositoryNoLazyLoad<T> _repository;
        protected HistoricoAlteracaoService _historicoAlteracaoService;
        public BaseServiceWithLog(IRepositoryNoLazyLoad<T> repository
            , HistoricoAlteracaoService historicoAlteracaoService)
        {
            _repository = repository;
            _historicoAlteracaoService = historicoAlteracaoService;
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

        public virtual void UpdateWithLog(T entity)
        {
            T dbEntity = GetById(((IEntity)entity).Id);
            _historicoAlteracaoService.RegistrarAlteracoes(dbEntity, entity);
            _repository.Update(entity);
        }

        public virtual void UpdateAll(IEnumerable<T> entityList)
        {
            _repository.UpdateAll(entityList);
        }
    }
}

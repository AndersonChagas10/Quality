using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Interfaces.Repositories;
using Dominio.Entities;

namespace Dominio.Services
{
    public class ServiceBase<T> : IDisposable, IServiceBase<T> where T : class
    {

        private readonly IRepositoryBase<T> _repositoryBase;

        public ServiceBase(IRepositoryBase<T> repo)
        {

            _repositoryBase = repo;
        }

        public void Add(T obj)
        {
            _repositoryBase.Add(obj);
        }

        public T GetById(int id)
        {
            return _repositoryBase.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repositoryBase.GetAll();
        }

        public void Update(T obj)
        {
            _repositoryBase.Update(obj);
        }

        public void Remove(T obj)
        {
            _repositoryBase.Remove(obj);
        }

        public void Dispose()
        {
            _repositoryBase.Dispose();
        }

        public GenericReturn<T> RetornaExcecaoBase(Exception ex, string mensagemErro = "", string mensagemAlerta = "", T obj = null)
        {
           
            var inner = ex.InnerException.IsNotNull() ? ex.InnerException.Message : "Não consta.";
           
            return new GenericReturn<T>() {
               MensagemErro = mensagemErro,
               MensagemExcecao = ex.Message + inner,
               Retorno = obj,
               MensagemAlerta = mensagemAlerta
           };

        }
    }

}

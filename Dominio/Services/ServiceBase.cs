﻿using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using Dominio.Interfaces.Repositories;
using Dominio.Entities.BaseEntity;
using DTO.Helpers;

namespace Dominio.Services
{
    public class ServiceBase<T> : IDisposable, IServiceBase<T> where T : EntityBase
    {

        private readonly IRepositoryBase<T> _repositoryBase;
        private string inseridoOk { get { return "Data successfully saved."; } }
        private string AlteradoOk { get { return "Data successfully changed."; } }
        private string NaoInserido { get { return "It was not possible to insert data."; } }
        private string emptyObj { get { return "It was not possible to insert data, object is empity."; } }

        public ServiceBase(IRepositoryBase<T> repo)
        {

            _repositoryBase = repo;
        }

        #region Salvar

        public GenericReturn<T> Add(T obj)
        {
            try
            {

                _repositoryBase.Add(obj);
                if (obj.Id > 0)
                    return new GenericReturn<T>(AlteradoOk);
                else
                    return new GenericReturn<T>(inseridoOk);

            }
            catch (ExceptionHelper ex)
            {
                throw new ExceptionHelper(NaoInserido, ex);
            }
        }

        public GenericReturn<T> AddAll(IEnumerable<T> obj)
        {
            try
            {
                if (!obj.GetEnumerator().MoveNext())
                    throw new ExceptionHelper(emptyObj);

                _repositoryBase.AddAll(obj);
                return new GenericReturn<T>(inseridoOk);
            }
            catch (ExceptionHelper ex)
            {
                throw new ExceptionHelper("Erro ao inserir o registro.", ex);
            }
        }

        public GenericReturn<T> AddOrUpdate(T obj)
        {
            try
            {
                _repositoryBase.AddOrUpdate(obj);
                if (obj.Id > 0)
                    return new GenericReturn<T>(AlteradoOk);
                else
                    return new GenericReturn<T>(inseridoOk);
            }
            catch (ExceptionHelper ex)
            {
                throw new ExceptionHelper("Erro ao inserir o registro.", ex);
            }
        }

        public GenericReturn<T> Update(T obj)
        {
            try
            {

                _repositoryBase.Update(obj);
                return new GenericReturn<T>(AlteradoOk);

            }
            catch (ExceptionHelper ex)
            {
                throw new ExceptionHelper("Erro ao alterar o registro.", ex);
            }
        }

        #endregion

        public T GetById(int id)
        {
            return _repositoryBase.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repositoryBase.GetAll();
        }

        public void Remove(T obj)
        {
            _repositoryBase.Remove(obj);
        }

        public void Dispose()
        {
            _repositoryBase.Dispose();
        }

        public void Delete(int id)
        {
            _repositoryBase.Delete(id);
        }

        public void RemoveAll(IEnumerable<T> obj)
        {
            _repositoryBase.RemoveAll(obj);
        }

        public T First()
        {
            return _repositoryBase.First();
        }
    }

}

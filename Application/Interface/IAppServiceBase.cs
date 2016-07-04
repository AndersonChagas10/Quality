﻿using Dominio.Entities;
using System.Collections.Generic;

namespace Application.Interface
{
    public interface IAppServiceBase<T> where T : class
    {

        GenericReturn<T> Add(T obj);

        T GetById(int id);

        IEnumerable<T> GetAll();

        GenericReturn<T> Update(T obj);

        void Remove(T obj);

        void Dispose();

        GenericReturn<T> AddAll(IEnumerable<T> obj);

        void Delete(int id);

        void RemoveAll(IEnumerable<T> obj);

        T First();

        GenericReturn<T> AddOrUpdate(T obj);
    }
}

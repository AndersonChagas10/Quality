using System;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Helpers;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class ResultOldRepository :  IResultOldRepository
    {

        
        private readonly IRepositoryBase<ResultOld> _repoBase;
        public ResultOldRepository(IRepositoryBase<ResultOld> repoBase)
        {
            _repoBase = repoBase;
        }

        //public ResultOldRepository(DbContextSgq _db)
        //    :base (_db)
        //{

        //}

        public void Salvar(ResultOld r)
        {
            _repoBase.Add(r);
        }

        public void SalvarLista(List<ResultOld> list)
        {
            _repoBase.AddAll(list);
        }


    }
}

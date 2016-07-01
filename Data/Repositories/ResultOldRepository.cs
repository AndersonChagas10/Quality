using System;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Helpers;

namespace Data.Repositories
{
    public class ResultOldRepository : IResultOldRepository
    {
        private readonly IRepositoryBase<ResultOld> _repoBase;

        public ResultOldRepository(IRepositoryBase<ResultOld> repoBase)
        {
            _repoBase = repoBase;
        }

        public GenericReturn<ResultOld> Salvar(ResultOld r)
        {
            try
            {
                _repoBase.AddOrUpdate(r);
                _repoBase.Commit();
            }
            catch (Exception e)
            {
                return ExceptionHelper<ResultOld>.RetornaExcecaoBase(e);
            }
            return new GenericReturn<ResultOld>() { ReturnisBool = true };
        }
    }
}

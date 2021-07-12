using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;

namespace Conformity.Infra.Data.Core
{
    /// <summary>
    /// Repositório Base, classe de gerencia do Banco de Dados.
    /// </summary>
    /// <typeparam name="T">Object reconhecido pelo DataBase: EntityBase</typeparam>
    public class ParametrizacaoRepositoryBase<T> : RepositoryBase<T>, IParametrizacaoRepository<T>  where T : BaseModel
    {
        public ParametrizacaoRepositoryBase(ParametrizacaoEntityContext dbContext) : base(dbContext)
        {
        }
    }
}

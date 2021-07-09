using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;

namespace Conformity.Infra.Data.Core.Repository.PlanoDeAcao
{
    /// <summary>
    /// Repositório Base, classe de gerencia do Banco de Dados.
    /// </summary>
    /// <typeparam name="T">Object reconhecido pelo DataBase: EntityBase</typeparam>
    public class LogRepositoryBaseNoLazyLoad<T> : RepositoryBaseNoLazyLoad<T>, ILogRepositoryNoLazyLoad<T> where T : BaseModel
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="Db"></param>
        public LogRepositoryBaseNoLazyLoad(ParametrizacaoEntityContext dbContext) : base(dbContext)
        {
        }
    }
}

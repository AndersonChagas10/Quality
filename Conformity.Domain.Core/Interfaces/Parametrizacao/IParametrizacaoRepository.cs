using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface IParametrizacaoRepository<T> : IRepository<T> where T : IEntity
    {
    }
}

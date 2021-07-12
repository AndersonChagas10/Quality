using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface IParametrizacaoRepositoryNoLazyLoad<T> : IRepositoryNoLazyLoad<T> where T : IEntity
    {
    }
}

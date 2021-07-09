using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface ILogRepositoryNoLazyLoad<T> : IRepositoryNoLazyLoad<T> where T : IEntity
    {
    }
}

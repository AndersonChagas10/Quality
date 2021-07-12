using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface IPlanoDeAcaoRepositoryNoLazyLoad<T> : IRepositoryNoLazyLoad<T> where T : IEntity
    {
    }
}

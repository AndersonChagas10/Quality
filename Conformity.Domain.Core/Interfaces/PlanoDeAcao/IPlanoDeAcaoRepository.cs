using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface IPlanoDeAcaoRepository<T> : IRepository<T> where T : IEntity
    {
    }
}

using System.Collections.Generic;

namespace Conformity.Domain.Core.Interfaces
{
    public interface ILogRepository<T> : IRepository<T> where T : IEntity
    {
    }
}

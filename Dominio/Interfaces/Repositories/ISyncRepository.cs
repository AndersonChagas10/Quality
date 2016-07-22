using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Repositories
{
    public interface ISyncRepository<T> where T : class
    {
        List<T> GetDataToSincyAudit();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO;

namespace Dominio.Interfaces.Repositories
{
    public interface ISyncRepository<T> where T : class
    {
        List<T> GetDataToSincyAudit();
       
    }
}

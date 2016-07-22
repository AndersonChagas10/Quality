using Dominio;
using Dominio.Interfaces.Repositories;
using System.Linq;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class SyncRepository<T> :  ISyncRepository<T> where T : class
    {
        private SgqDbDevEntities connection = new SgqDbDevEntities();
       
        public List<T> GetDataToSincyAudit()
        {
            return new RepositoryBase<T>(connection).GetAll().ToList();
            //connection.Dispose();
        }
    }
}

using Dominio;
using Dominio.Interfaces.Repositories;
using System.Linq;
using System;

namespace Data.Repositories
{
    public class SyncRepository :  ISyncRepository
    {
       
        public void GetDataToSincyAudit()
        {
            SgqDbDevEntities connection = new SgqDbDevEntities();
            var Coleta = new RepositoryBase<Coleta>(connection).GetAll().ToList();
            var Level1 = new RepositoryBase<Level1>(connection).GetAll().ToList();
            var Level2 = new RepositoryBase<Level2>(connection).GetAll().ToList();
            var Level3 = new RepositoryBase<Level3>(connection).GetAll().ToList();
            var UserSgq = new RepositoryBase<UserSgq>(connection).GetAll().ToList();
            connection.Dispose();
        }
    }
}

using Dominio;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data.Entity;

namespace Data.Repositories
{
    public class CollectionLevel02Repo : RepositoryBase<CollectionLevel02>, ICollectionLevel02Repo
    {
        public CollectionLevel02Repo(SgqDbDevEntities Db) : base(Db)
        {
        }

        public void UpdateCollectionLevel02(CollectionLevel02 collectionLevel02)
        {
            var entry = db.Set<CollectionLevel02>();
            var old = entry.FirstOrDefault(r => r.Id == collectionLevel02.Id);
            //collectionLevel02.ConsolidationLevel02 = old.ConsolidationLevel02;
            //collectionLevel02.CollectionLevel03 = old.CollectionLevel03;
            var vtnc = collectionLevel02.AuditorId;
            old.AuditorId = vtnc;
            old.AlterDate = new DateTime();
            entry.Attach(old);
            //db.Entry(collectionLevel02).Property(x => x.CollectionLevel03).IsModified = false;
            //db.Entry(collectionLevel02).Property(x => x.ConsolidationLevel02).IsModified = false;
            db.Entry(old).State = EntityState.Modified;
            Commit();
        }
    }
}

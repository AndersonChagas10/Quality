using Dominio.Interfaces.Repositories;
using System.Linq;
using Dominio;
using System.Collections.Generic;
using DTO.Helpers;
using DTO.DTO;
using System;

namespace Data.Repositories
{
    public class GetDataResultRepository<T> : RepositoryBase<T>, IGetDataResultRepository<T> where T : class
    {

        public GetDataResultRepository(SgqDbDevEntities _db)
            : base(_db)
        {

        }

        public List<CollectionLevel02> GetLastEntryCollectionLevel02()
        {
            throw new NotImplementedException();
        }

        public List<CollectionLevel03> GetLastEntryCollectionLevel03()
        {
            throw new NotImplementedException();
        }

        public List<ConsolidationLevel01> GetLastEntryConsildatedLevel01()
        {
            var ids = db.Database.SqlQuery<int>("SELECT max(id) as id FROM [dbo].ConsolidationLevel01 group by Level01_Id").ToList();
            var lastResults = db.ConsolidationLevel01.Where(r => ids.Any(x => x == r.Id)).ToList();
            return lastResults;
        }

        public List<ConsolidationLevel02> GetLastEntryConsildatedLevel02()
        {
            throw new NotImplementedException();
        }
    }
}

﻿using Dominio.Interfaces.Repositories;
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

        public IEnumerable<CollectionLevel02> GetLastEntryCollectionLevel02(IEnumerable<ConsolidationLevel02> cl2)
        {
            var lastResults = db.CollectionLevel02.Where(r => cl2.Any(x => x.Id == r.ConsolidationLevel02_Id));
            return lastResults;

        }

        public IEnumerable<CollectionLevel03> GetLastEntryCollectionLevel03(IEnumerable<CollectionLevel02> cll2)
        {
            var lastResults = db.CollectionLevel03.Where(r => cll2.Any(x => x.Id == r.CollectionLevel02_ID));
            return lastResults;
        }

        public IEnumerable<ConsolidationLevel01> GetLastEntryConsildatedLevel01()
        {
            var ids = db.Database.SqlQuery<int>("SELECT max(id) as id FROM [dbo].ConsolidationLevel01 group by Level01_Id").ToList();
            var lastResults = db.ConsolidationLevel01.Where(r => ids.Any(x => x == r.Id));
            return lastResults;
        }

        public IEnumerable<ConsolidationLevel02> GetLastEntryConsildatedLevel02(IEnumerable<ConsolidationLevel01> cl1)
        {
            var listResults = db.ConsolidationLevel02.Where(r => cl1.Any(x => x.Id == r.Level01Consolidation_Id));
            return listResults;
        }
    }
}

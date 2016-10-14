using System;
using System.Collections.Generic;
using Data.Repositories;
using Dominio;
using Dominio.Interfaces.Repositories.DataCollectionDomainRepositorie;

namespace Data.DataCollectionRepo.Repositories
{
    public class DataCollectionResultRepository : RepositoryBase<DataCollection>, IDataCollectionResultRepository
    {
        public DataCollectionResultRepository(SgqDbDevEntities Db) 
            : base(Db)
        {
        }

        public void SaveDataCollectionResult(List<DataCollection> ListObj)
        {
            AddAll(ListObj);
        }

        public void SaveDataCollectionResult(DataCollection obj)
        {
            Add(obj);
        }

        public void SaveOrUpdateDataCollectionResult(List<DataCollection> ListObj)
        {
            foreach (var i in ListObj)
                AddOrUpdate(i);
        }

        public void SaveOrUpdateDataCollectionResult(DataCollection obj)
        {
            AddOrUpdate(obj);
        }

        public void UpdateDataCollectionResult(DataCollection obj)
        {
            Update(obj);
        }

        public void UpdateDataCollectionResult(List<DataCollection> ListObj)
        {
            foreach (var i in ListObj)
                Update(obj);
        }
    }
}

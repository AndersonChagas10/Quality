using Data.Repositories;
using System.Collections.Generic;
using Dominio;
using Dominio.Interfaces.Repositories.DataCollectionDomainRepositorie;
using System;

namespace Data.DataCollectionRepo.Repositories
{
    public class DataCollectionHeadRepository : RepositoryBase<DataCollection>, IDataCollectionHeadRepository
    {

        public DataCollectionHeadRepository(SgqDbDevEntities Db) 
            : base(Db)
        {

        }

        public void SaveDataCollectionHead(List<DataCollection> ListObj)
        {
            AddAll(ListObj);
        }

        public void SaveDataCollectionHead(DataCollection obj)
        {
            Add(obj);
        }

        public void SaveOrUpdateDataCollectionHead(List<DataCollection> ListObj)
        {
            foreach (var i in ListObj)
                AddOrUpdate(i);
        }

        public void SaveOrUpdateDataCollectionHead(DataCollection obj)
        {
            AddOrUpdate(obj);
        }

        public void UpdateDataCollectionHead(DataCollection obj)
        {
            Update(obj);
        }

        public void UpdateDataCollectionHead(List<DataCollection> ListObj)
        {
            foreach (var i in ListObj)
                Update(i);
        }
    }
}

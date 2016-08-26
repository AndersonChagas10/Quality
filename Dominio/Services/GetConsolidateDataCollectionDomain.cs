using Dominio.Interfaces.Services;
using System;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Repositories;

namespace Dominio.Services
{
    public class GetConsolidateDataCollectionDomain : IGetConsolidateDataCollectionDomain
    {
        private IGetDataResultRepository<ConsolidationLevel01> _consolidationLevel01Repository;
        private IGetDataResultRepository<ConsolidationLevel02> _consolidationLevel02Repository;
        private IGetDataResultRepository<CollectionLevel02> _collectionLevel02Repository;
        private IGetDataResultRepository<CollectionLevel03> _collectionLevel03Repository;

        public GetConsolidateDataCollectionDomain(IGetDataResultRepository<ConsolidationLevel01> consolidationLevel01Repository,
            IGetDataResultRepository<ConsolidationLevel02> consolidationLevel02Repository,
            IGetDataResultRepository<CollectionLevel02> collectionLevel02Repository,
            IGetDataResultRepository<CollectionLevel03> collectionLevel03Repository)
        {
            _consolidationLevel01Repository = consolidationLevel01Repository;
            _consolidationLevel02Repository = consolidationLevel02Repository;
            _collectionLevel02Repository = collectionLevel02Repository;
            _collectionLevel03Repository = collectionLevel03Repository;
        }

        public GenericReturn<ColetaDTO> GetLastEntry()
        {
            try
            {

                var consildatedLelve01 = _consolidationLevel01Repository.GetLastEntryConsildatedLevel01();
                var consildatedLelve02 = _consolidationLevel02Repository.GetLastEntryConsildatedLevel02();
                var collectionLelve02 = _consolidationLevel02Repository.GetLastEntryCollectionLevel02();
                var collectionLelve03 = _consolidationLevel02Repository.GetLastEntryCollectionLevel03();

                return new GenericReturn<ColetaDTO>(); 
}
            catch (Exception e)
            {
                return new GenericReturn<ColetaDTO>(e, "Cannot get data.");
            }
        }
    }
}

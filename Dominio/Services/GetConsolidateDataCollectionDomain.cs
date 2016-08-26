using Dominio.Interfaces.Services;
using System;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Repositories;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using System.Collections;

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

        public GenericReturn<GetSyncDTO> GetLastEntry()
        {
            try
            {

                var consildatedLelve01 = _consolidationLevel01Repository.GetLastEntryConsildatedLevel01();
                var consildatedLelve01List = consildatedLelve01.ToList();

                var consildatedLelve02 = _consolidationLevel02Repository.GetLastEntryConsildatedLevel02(consildatedLelve01);
                var consildatedLelve02List = consildatedLelve02.ToList();

                var collectionLelve02 = _consolidationLevel02Repository.GetLastEntryCollectionLevel02(consildatedLelve02);
                var collectionLelve02List = collectionLelve02.ToList();

                var collectionLelve03 = _consolidationLevel02Repository.GetLastEntryCollectionLevel03(collectionLelve02);
                var collectionLelve03List = collectionLelve03.ToList();

                var consildatedLelve01ListDTO1 = new List<ConsolidationLevel01DTO>();
                foreach(var i in consildatedLelve01List)
                    consildatedLelve01ListDTO1.Add(Mapper.Map<ConsolidationLevel01DTO>(i));

                var consildatedLelve01ListDTO2 = new List<ConsolidationLevel02DTO>();
                foreach (var i in consildatedLelve02List)
                    consildatedLelve01ListDTO2.Add(Mapper.Map<ConsolidationLevel02DTO>(i));

                var collectionLelve02ListDTO = new List<CollectionLevel02DTO>();
                foreach (var i in collectionLelve02List)
                    collectionLelve02ListDTO.Add(Mapper.Map<CollectionLevel02DTO>(i));

                var collectionLelve03ListDTO = new List<CollectionLevel03DTO>();
                foreach (var i in collectionLelve03List)
                    collectionLelve03ListDTO.Add(Mapper.Map<CollectionLevel03DTO>(i));


                //var putaQuePariuFdp = new List<ConsolidationLevel01DTO>();
                foreach (var i in consildatedLelve01ListDTO1)
                {
                    var temp = consildatedLelve01ListDTO2.Where(r => r.Level01ConsolidationId == i.Id);
                    
                    i.consolidationLevel02DTO = temp.ToList();
                    var temp2 = collectionLelve02ListDTO.Where(r => temp.Any(z => z.Id == r.ConsolidationLevel02_Id));
                    i.collectionLevel02DTO = temp2.ToList();
                    foreach (var y in i.collectionLevel02DTO)
                    {
                        y.collectionLevel03DTO = collectionLelve03ListDTO.Where(r=> temp2.Any(z=>y.Id == r.CollectionLevel02_ID)).ToList();
                    }
                    //putaQuePariuFdp.Add(i);
                }

               
                var listResult = new GetSyncDTO()
                {
                    ConsolidationLevel01 = consildatedLelve01ListDTO1,
                    //ConsolidationLevel02 = consildatedLelve01ListDTO2,
                    //CollectionLevel02 = collectionLelve02ListDTO,
                    //CollectionLevel03 = collectionLelve03ListDTO,
                };

                return new GenericReturn<GetSyncDTO>(listResult); 
}
            catch (Exception e)
            {
                return new GenericReturn<GetSyncDTO>(e, "Cannot get data.");
            }
        }
    }
}

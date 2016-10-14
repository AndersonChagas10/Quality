using Dominio.Interfaces.Services;
using System;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Repositories;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using DTO;

namespace Dominio.Services
{

    public class GetConsolidateDataCollectionDomain : IGetConsolidateDataCollectionDomain
    {
        private IGetDataResultRepository<ConsolidationLevel01> _consolidationLevel01Repository;
        private IGetDataResultRepository<ConsolidationLevel02> _consolidationLevel02Repository;
        private IGetDataResultRepository<CollectionLevel02> _collectionLevel02Repository;
        private IGetDataResultRepository<CollectionLevel03> _collectionLevel03Repository;
        private IGetDataResultRepository<CollectionHtml> _baseRepoCollectionHtml;

        public GetConsolidateDataCollectionDomain(IGetDataResultRepository<ConsolidationLevel01> consolidationLevel01Repository,
            IGetDataResultRepository<ConsolidationLevel02> consolidationLevel02Repository,
            IGetDataResultRepository<CollectionLevel02> collectionLevel02Repository,
            IGetDataResultRepository<CollectionLevel03> collectionLevel03Repository,
            IGetDataResultRepository<CollectionHtml> baseRepoCollectionHtml)
        {
            _baseRepoCollectionHtml = baseRepoCollectionHtml;
            _consolidationLevel01Repository = consolidationLevel01Repository;
            _consolidationLevel02Repository = consolidationLevel02Repository;
            _collectionLevel02Repository = collectionLevel02Repository;
            _collectionLevel03Repository = collectionLevel03Repository;
        }

        public GenericReturn<GetSyncDTO> GetLastEntry()
        {
            throw new NotImplementedException();
        }

        public GenericReturn<GetSyncDTO> GetHtmlLastEntry(SyncDTO idUnidade)
        {
            try
            {
                CollectionHtml result = _baseRepoCollectionHtml.GetHtmlLastEntry(idUnidade);
                var returnObj = new GetSyncDTO() { html = "<div class=\"Results \">" + result.Html + "</div>" };
                var retorno = new GenericReturn<GetSyncDTO>(returnObj);
                retorno.IdSaved = result.Id;
                return retorno;
            }
            catch (Exception e)
            {
                return new GenericReturn<GetSyncDTO>(e, "Cannot get html data.");
            }
        }

    }

}

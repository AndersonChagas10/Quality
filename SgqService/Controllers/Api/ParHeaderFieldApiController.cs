using ADOFactory;
using Dominio;
using ServiceModel;
using SgqService.Handlres;
using SgqService.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqService.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/ParHeader")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParHeaderFieldApiController : BaseApiController
    {
        SgqServiceBusiness.Api.ParHeaderFieldApiController business;
        public ParHeaderFieldApiController()
        {
            business = new SgqServiceBusiness.Api.ParHeaderFieldApiController();
        }

        [HttpGet]
        [Route("GetCollectionLevel2XHeaderField/{unitId}/{date}")]
        public IEnumerable<CollectionHeaderField> GetListCollectionHeaderField(int UnitId, String Date)
        {
            VerifyIfIsAuthorized();
            return business.GetListCollectionHeaderField(UnitId, Date);
        }

        [HttpGet]
        [Route("GetListParMultipleValuesXParCompany/{unitId}/{level1_id}")]
        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId, string level1_id)
        {
            VerifyIfIsAuthorized();
            return business.GetListParMultipleValuesXParCompany(UnitId);
        }

        [HttpGet]
        [Route("GetListParMultipleValuesXParCompany/{unitId}")]
        public IEnumerable<ParMultipleValuesXParCompany> GetListParMultipleValuesXParCompany(int UnitId)
        {
            VerifyIfIsAuthorized();
            return business.GetListParMultipleValuesXParCompany(UnitId);
        }
    }
}

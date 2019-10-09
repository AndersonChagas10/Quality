using ADOFactory;
using Dominio;
using DTO;
using ServiceModel;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/PCC1B")]
    public class PCC1BController : BaseApiController
    {
        SgqServiceBusiness.Api.PCC1BController business;
        public PCC1BController()
        {
            business = new SgqServiceBusiness.Api.PCC1BController();
        }

        [HttpPost]
        [Route("Next")]
        public _PCC1B Next(_Receive receive)
        {
            VerifyIfIsAuthorized();
            return business.Next(receive);
        }

        [HttpPost]
        [Route("TotalNC/{parLevel2IdDianteiro}/{parLevel2Id2Traseiro}/{shift}")]
        public IEnumerable<CollectionLevel2PCC1B> TotalNC(_Receive receive, int parLevel2IdDianteiro,
            int parLevel2Id2Traseiro, int shift)
        {
            VerifyIfIsAuthorized();
            return business.TotalNC(receive, parLevel2IdDianteiro, parLevel2Id2Traseiro, shift);
        }
    }
}

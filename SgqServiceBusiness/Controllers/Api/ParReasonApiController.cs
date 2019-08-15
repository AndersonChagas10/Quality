using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SgqServiceBusiness.Api
{
    public class ParReasonApiController
    {

        public List<ParReason> Get()
        {
            using (var db = new SgqDbDevEntities())
            {
                return db.ParReason.Where(r => r.IsActive).ToList();
            }
        }

    }
}

using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.App
{
    [RoutePrefix("api/AppParams")]
    public class AppParamsApiController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public AppParamsApiController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        [HttpPost]
        [HandleApi()]
        [Route("GetContadoresX")]
        public List<CollectionLevel2> GetContadoresX()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return  db.CollectionLevel2.Where(r => r.CollectionDate == DateTime.Now ).ToList();

        }


    }
}
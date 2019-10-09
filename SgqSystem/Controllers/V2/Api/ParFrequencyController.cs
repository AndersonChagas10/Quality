using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dominio;
using SgqSystem.Controllers.Api;

namespace SgqSystem.Controllers.V2.Api
{
    public class ParFrequencyController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParFrequency
        public IHttpActionResult GetParFrequency()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.ParFrequency.Where(x=>x.IsActive).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
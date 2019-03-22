using Dominio;
using SgqSystem.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/ParLevel3Api")]
    public class ParLevel3ApiController : BaseApiController
    {
        //private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParLevel1Api
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetParLevel3()
        {
            ParLevel3Selects parLevel3Selects = new ParLevel3Selects();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel3Selects.ParLevels3 = db.ParLevel3.ToList();
                parLevel3Selects.ParFieldTypes = db.ParFieldType.Where(x => x.IsActive).ToList();
            }

            return Ok(parLevel3Selects);
        }

        // GET: api/ParLevel1Api/5
        //[ResponseType(typeof(ParLevel1Result))]
        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParLevel1(int id)
        {
            ParLevel3Result parlevel3Result = new ParLevel3Result();
            ParLevel3 parLevel3 = new ParLevel3();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel3 = db.ParLevel3.Find(id);

                if (parLevel3 == null)
                {
                    return NotFound();
                }

                parlevel3Result.Parlevel3 = parLevel3;
            }

            return Ok(parlevel3Result);
        }

        private bool ParLevel1Exists(int id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                return db.ParLevel1.Count(e => e.Id == id) > 0;
            }
        }

        //Criar um model para os retornos das classes
        public class ParLevel3Result
        {
            public ParLevel3 Parlevel3 { get; set; }
        }

        public class ParLevel3Selects
        {
            public List<ParLevel3> ParLevels3 { get; set; }
            public List<ParFieldType> ParFieldTypes { get; set; }
        }
    }
}

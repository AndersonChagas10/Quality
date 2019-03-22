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
    [RoutePrefix("api/ParLevel1Api")]
    public class ParLevel1ApiController : BaseApiController
    {
        //private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParLevel1Api
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetParLevel1()
        {
            ParLevel1Selects parLevel1Selects = new ParLevel1Selects();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel1Selects.ParLevels1 = db.ParLevel1.ToList();
                parLevel1Selects.ParClusters = db.ParCluster.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParHeaderFields = db.ParHeaderField.Where(x => x.IsActive).ToList();
            }

            return Ok(parLevel1Selects);
        }

        // GET: api/ParLevel1Api/5
        //[ResponseType(typeof(ParLevel1Result))]
        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParLevel1(int id)
        {
            ParLevel1Result parlevel1Result = new ParLevel1Result();
            ParLevel1 parLevel1 = new ParLevel1();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel1 = db.ParLevel1.Find(id);

                if (parLevel1 == null)
                {
                    return NotFound();
                }

                parlevel1Result.Parlevel1 = parLevel1;
                parlevel1Result.ParLevel1XCluster = db.ParLevel1XCluster.Where(x => x.ParLevel1_Id == parLevel1.Id && x.IsActive).ToList();
                parlevel1Result.ParLevel1XHeaderFields = db.ParLevel1XHeaderField.Where(x => x.ParLevel1_Id == parLevel1.Id && x.IsActive).ToList();
            }

            return Ok(parlevel1Result);
        }

        // PUT: api/ParLevel1Api/5
        //[ResponseType(typeof(void))]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutParLevel1(ParLevel1 parLevel1)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != parLevel1.Id)
            //{
            //    return BadRequest();
            //}

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {

                db.Entry(parLevel1).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //if (!ParLevel1Exists(id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ParLevel1Api
        //[ResponseType(typeof(ParLevel1))]
        [HttpPost]
        [Route("Post")]
        public IHttpActionResult PostParLevel1(ParLevel1 parLevel1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.ParLevel1.Add(parLevel1);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = parLevel1.Id }, parLevel1);
        }

        //// DELETE: api/ParLevel1Api/5
        //[ResponseType(typeof(ParLevel1))]
        //public IHttpActionResult DeleteParLevel1(int id)
        //{
        //    ParLevel1 parLevel1 = db.ParLevel1.Find(id);
        //    if (parLevel1 == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ParLevel1.Remove(parLevel1);
        //    db.SaveChanges();

        //    return Ok(parLevel1);
        //}

        protected override void Dispose(bool disposing)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {

                if (disposing)
                {
                    db.Dispose();
                }

                base.Dispose(disposing);
            }
        }

        private bool ParLevel1Exists(int id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                return db.ParLevel1.Count(e => e.Id == id) > 0;
            }
        }

        //Criar um model para os retornos das classes
        public class ParLevel1Result
        {
            public ParLevel1 Parlevel1 { get; set; }
            public List<ParLevel1XHeaderField> ParLevel1XHeaderFields { get; set; }
            public List<ParLevel1XCluster> ParLevel1XCluster { get; set; }

        }

        public class ParLevel1Selects
        {
            public List<ParLevel1> ParLevels1 { get; set; }
            public List<ParHeaderField> ParHeaderFields { get; set; }
            public List<ParCluster> ParClusters { get; set; }
        }
    }
}

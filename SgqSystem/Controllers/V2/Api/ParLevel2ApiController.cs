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
    [RoutePrefix("api/ParLevel2Api")]
    public class ParLevel2ApiController : BaseApiController
    {
        //private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParLevel1Api
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetParLevel2()
        {
            ParLevel2Selects parLevel2Selects = new ParLevel2Selects();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel2Selects.ParLevels2 = db.ParLevel2.ToList();
            }

            return Ok(parLevel2Selects);
        }

        // GET: api/ParLevel1Api/5
        //[ResponseType(typeof(ParLevel1Result))]
        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParLevel1(int id)
        {
            ParLevel2Result parlevel1Result = new ParLevel2Result();
            ParLevel2 parLevel2 = new ParLevel2();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel2 = db.ParLevel2.Find(id);

                if (parLevel2 == null)
                {
                    return NotFound();
                }

                parlevel1Result.Parlevel2 = parLevel2;
                parlevel1Result.Parlevel2.ParEvaluation = db.ParEvaluation.Where(x => x.IsActive && x.ParLevel2_Id == parLevel2.Id).ToList();
            }

            return Ok(parlevel1Result);
        }

        private bool ParLevel1Exists(int id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                return db.ParLevel1.Count(e => e.Id == id) > 0;
            }
        }

        //Criar um model para os retornos das classes
        public class ParLevel2Result
        {
            public ParLevel2 Parlevel2 { get; set; }
        }

        public class ParLevel2Selects
        {
            public List<ParLevel2> ParLevels2 { get; set; }
        }


        [HttpPost]
        [Route("PostParEvaluation")]
        public IHttpActionResult PostParEvaluation(ParEvaluation parEvaluation)
        {
            if (!SaveOrUpdateParEvaluation(parEvaluation))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool SaveOrUpdateParEvaluation(ParEvaluation parEvaluation)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parEvaluation.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parEvaluationOld = db.ParEvaluation.Find(parEvaluation.Id);
                        parEvaluationOld.Number = parEvaluation.Number;
                        parEvaluationOld.Sample = parEvaluation.Sample;
                        parEvaluationOld.ParLevel1_Id = parEvaluation.ParLevel1_Id;
                        parEvaluationOld.ParLevel2_Id = parEvaluation.ParLevel2_Id;
                        parEvaluationOld.ParCluster_Id = parEvaluation.ParCluster_Id;
                        parEvaluationOld.ParCompany_Id = parEvaluation.ParCompany_Id;
                        parEvaluationOld.IsActive = parEvaluation.IsActive;
                    }
                    else
                    {
                        db.ParEvaluation.Add(parEvaluation);
                    }

                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    return false;
                }

                return true;
            }
        }

    }
}

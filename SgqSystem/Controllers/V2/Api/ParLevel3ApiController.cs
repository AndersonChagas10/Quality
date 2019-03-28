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
                parlevel3Result.Parlevel3.ParLevel3EvaluationSample =
                    db.ParLevel3EvaluationSample
                    .Where(x => x.IsActive == true && x.ParLevel3_Id == parLevel3.Id)
                    .ToList();
                parlevel3Result.Parlevel3.ParVinculoPeso =
                    db.ParVinculoPeso
                    .Where(x => x.IsActive == true && x.ParLevel3_Id == parLevel3.Id)
                    .ToList();
                
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

        [HttpPost]
        [Route("PostParEvaluationSample")]
        public IHttpActionResult PostParEvaluation(ParLevel3EvaluationSample parLevel3EvaluationSample)
        {
            if (!SaveOrUpdateParLevel3EvaluationSample(parLevel3EvaluationSample))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool SaveOrUpdateParLevel3EvaluationSample(ParLevel3EvaluationSample parLevel3EvaluationSample)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parLevel3EvaluationSample.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parEvaluationOld = db.ParLevel3EvaluationSample.Find(parLevel3EvaluationSample.Id);
                        parEvaluationOld.EvaluationNumber = parLevel3EvaluationSample.EvaluationNumber;
                        parEvaluationOld.SampleNumber = parLevel3EvaluationSample.SampleNumber;
                        parEvaluationOld.ParLevel1_Id = parLevel3EvaluationSample.ParLevel1_Id;
                        parEvaluationOld.ParLevel2_Id = parLevel3EvaluationSample.ParLevel2_Id;
                        parEvaluationOld.ParCompany_Id = parLevel3EvaluationSample.ParCompany_Id;
                        parEvaluationOld.IsActive = parLevel3EvaluationSample.IsActive;
                    }
                    else
                    {
                        parLevel3EvaluationSample.EvaluationInterval = "";
                        db.ParLevel3EvaluationSample.Add(parLevel3EvaluationSample);
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


        [HttpPost]
        [Route("PostParVinculoPeso")]
        public IHttpActionResult PostParVinculoPeso(ParVinculoPeso parVinculoPeso)
        {
            if (!SaveOrUpdateParVinculoPeso(parVinculoPeso))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool SaveOrUpdateParVinculoPeso(ParVinculoPeso parVinculoPeso)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parVinculoPeso.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parVinculoPesoOld = db.ParVinculoPeso.Find(parVinculoPeso.Id);
                        parVinculoPesoOld.Peso = parVinculoPeso.Peso;
                        parVinculoPesoOld.ParCompany_Id = parVinculoPeso.ParCompany_Id;
                        parVinculoPesoOld.ParDepartment_Id = parVinculoPeso.ParDepartment_Id;
                        parVinculoPesoOld.ParLevel1_Id = parVinculoPeso.ParLevel1_Id;
                        parVinculoPesoOld.ParLevel2_Id = parVinculoPeso.ParLevel2_Id;
                        parVinculoPesoOld.ParLevel3_Id = parVinculoPeso.ParLevel3_Id;
                        parVinculoPesoOld.ParGroupParLevel1_Id = parVinculoPeso.ParGroupParLevel1_Id;
                        parVinculoPesoOld.IsActive = parVinculoPeso.IsActive;
                    }
                    else
                    {
                        parVinculoPeso.Name = "";
                        db.ParVinculoPeso.Add(parVinculoPeso);
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

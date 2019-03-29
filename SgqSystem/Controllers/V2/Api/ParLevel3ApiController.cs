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
                parlevel3Result.Parlevel3.ParLevel3Value =
                    db.ParLevel3Value
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
        [Route("PostParLevel3")]
        public IHttpActionResult PostParLevel3(ParLevel3 parLevel3)
        {
            if (!SaveOrUpdateParLevel3(parLevel3))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool SaveOrUpdateParLevel3(ParLevel3 parLevel3)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parLevel3.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parLevel2Old = db.ParLevel3.Find(parLevel3.Id);
                        parLevel2Old.Name = parLevel3.Name;
                        parLevel2Old.Description = parLevel3.Description;
                        parLevel2Old.IsActive = parLevel3.IsActive;
                        parLevel2Old.HasTakePhoto = parLevel3.HasTakePhoto;
                    }
                    else
                    {
                        db.ParLevel3.Add(parLevel3);
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

        [HttpPost]
        [Route("PostParTipoDado")]
        public IHttpActionResult PostParTipoDado(ParLevel3Value parLevel3Value)
        {
            if (!SaveOrUpdateParLevel3Value(parLevel3Value))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool SaveOrUpdateParLevel3Value(ParLevel3Value parLevel3Value)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parLevel3Value.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parVinculoPesoOld = db.ParLevel3Value.Find(parLevel3Value.Id);
                        parVinculoPesoOld.ParLevel3InputType_Id = parLevel3Value.ParLevel3InputType_Id;
                        parVinculoPesoOld.ParCompany_Id = parLevel3Value.ParCompany_Id;
                        parVinculoPesoOld.ParLevel1_Id = parLevel3Value.ParLevel1_Id;
                        parVinculoPesoOld.ParLevel2_Id = parLevel3Value.ParLevel2_Id;
                        parVinculoPesoOld.ParLevel3_Id = parLevel3Value.ParLevel3_Id;
                        parVinculoPesoOld.ParLevel3BoolTrue_Id = parLevel3Value.ParLevel3BoolTrue_Id;
                        parVinculoPesoOld.ParLevel3BoolFalse_Id = parLevel3Value.ParLevel3BoolFalse_Id;
                        parVinculoPesoOld.ParMeasurementUnit_Id = parLevel3Value.ParMeasurementUnit_Id;
                        parVinculoPesoOld.IntervalMin = parLevel3Value.IntervalMin;
                        parVinculoPesoOld.IntervalMax = parLevel3Value.IntervalMax;
                        parVinculoPesoOld.DynamicValue = parLevel3Value.DynamicValue;
                        parVinculoPesoOld.IsActive = parLevel3Value.IsActive;
                    }
                    else
                    {
                        db.ParLevel3Value.Add(parLevel3Value);
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

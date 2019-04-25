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
                parlevel3Result.Parlevel3.ParLevel3XHelp =
                    db.ParLevel3XHelp
                    .Where(x => x.IsActive == true && x.ParLevel3_Id == parLevel3.Id)
                    .ToList();

                foreach (var item in parlevel3Result.Parlevel3.ParLevel3Value)
                {
                    item.ParInputTypeValues = db.ParInputTypeValues.Where(x => x.ParLevel3Value_Id == item.Id).ToList();
                }

            }

            return Ok(parlevel3Result);
        }

        [HttpGet]
        [Route("GetParLevel3Vinculados/{ParLevel1_Id}/{ParLevel2_Id}")]
        public IHttpActionResult GetParLevel3Vinculados(int ParLevel1_Id, int ParLevel2_Id)
        {
            var select = new Select();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                select.Options = new List<Option>();

                var linkedName = "Vinculado"; //"Linked"
                var notLinkedName = "Sem vinculo";

                var vinculados = db.Database.SqlQuery<Option>($@"SELECT DISTINCT L3.Id as Value, L3.Name as Text, '{linkedName}' as GroupName FROM ParLevel3 L3
                                                                RIGHT JOIN ParLevel3Level2 L32 ON L32.ParLevel3_Id = L3.Id
                                                                INNER JOIN ParLevel3Level2Level1 L321 ON L321.ParLevel3Level2_Id = L32.Id
                                                                WHERE L321.ParLevel1_Id = {ParLevel1_Id} AND L32.ParLevel2_Id = {ParLevel2_Id}").ToList();

                var naoVinculados = db.Database.SqlQuery<Option>($@"SELECT Id as Value, Name as Text,'{notLinkedName}' as GroupName FROM ParLevel3 
                                                                WHERE Id NOT IN (SELECT DISTINCT L3.Id as Value FROM ParLevel3 L3
                                                                                 RIGHT JOIN ParLevel3Level2 L32 ON L32.ParLevel3_Id = L3.Id
                                                                                 INNER JOIN ParLevel3Level2Level1 L321 ON L321.ParLevel3Level2_Id = L32.Id
                                                                                 WHERE L321.ParLevel1_Id = {ParLevel1_Id} AND L32.ParLevel2_Id = {ParLevel2_Id})
                                                                ORDER BY Id").ToList();

                select.Options.AddRange(vinculados);
                select.Options.AddRange(naoVinculados);

            }

            return Ok(select);
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

            return Ok(parLevel3.Id);
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
                        parLevel2Old.CaracteristicaDeRisco = parLevel3.CaracteristicaDeRisco;
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
                        parVinculoPesoOld.ParLevel3Group_Id = parVinculoPeso.ParLevel3Group_Id;
                        parVinculoPesoOld.ParCargo_Id = parVinculoPeso.ParCargo_Id;
                        parVinculoPesoOld.IsActive = parVinculoPeso.IsActive;
                        parVinculoPesoOld.Sample = parVinculoPeso.Sample;
                        parVinculoPesoOld.Evaluation = parVinculoPeso.Evaluation;
                        parVinculoPesoOld.ParFrequencyId = parVinculoPeso.ParFrequencyId;
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

                         foreach (var item in parLevel3Value.ParInputTypeValues)
                        {
                            item.ParLevel3Value_Id = parLevel3Value.Id;
                            if (item.Id > 0)
                            {
                                db.Entry(item).State = EntityState.Modified;
                            }
                            else
                            {
                                db.ParInputTypeValues.Add(item);
                            }
                        } 
                    }
                    else
                    {
                        var parLevel3ValueSalvo = db.ParLevel3Value.Add(parLevel3Value);
                        db.SaveChanges();

                        foreach (var item in parLevel3Value.ParInputTypeValues)
                        {
                            item.IsActive = true;
                            item.ParLevel3Value_Id = parLevel3ValueSalvo.Id;
                            if (item.Id > 0)
                            {
                                db.Entry(item).State = EntityState.Modified;
                            }
                            else
                            {
                                db.ParInputTypeValues.Add(item);
                            }
                        } 
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
        [Route("PostParLevel3XHelp")]
        public IHttpActionResult PostParLevel3XHelp(ParLevel3XHelp parLevel3XHelp)
        {
            if (!SaveOrUpdateParLevel3XHelp(parLevel3XHelp))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool SaveOrUpdateParLevel3XHelp(ParLevel3XHelp parLevel3XHelp)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parLevel3XHelp.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parVinculoPesoOld = db.ParLevel3XHelp.Find(parLevel3XHelp.Id);
                        parVinculoPesoOld.AlterDate = DateTime.Now;
                        parVinculoPesoOld.IsActive = parLevel3XHelp.IsActive;
                        parVinculoPesoOld.Titulo = parLevel3XHelp.Titulo;
                        parVinculoPesoOld.Corpo = parLevel3XHelp.Corpo;
                    }
                    else
                    {
                        db.ParLevel3XHelp.Add(parLevel3XHelp);
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

        public class Select
        {
            public List<Option> Options { get; set; }
        }

        public class Option
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public string GroupName { get; set; }

        }
    }
}

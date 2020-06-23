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
        private static int[] parInputTypesWithQualificacao = { 1, 6, 15 };

        // GET: api/ParLevel1Api
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetParLevel3()
        {
            InicioRequisicao();
            ParLevel3Selects parLevel3Selects = new ParLevel3Selects();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel3Selects.ParLevels3 = db.ParLevel3.ToList();
                parLevel3Selects.ParFieldTypes = db.ParFieldType.Where(x => x.IsActive).ToList();
            }

            return Ok(parLevel3Selects);
        }
        [HttpPost]
        [Route("PostParHeaderFieldXMultipleValues")]
        public IHttpActionResult PostParHeaderFieldXMultipleValues(ParHeaderFieldGeral saveParHeaderFieldGeral)
        {
            InicioRequisicao();

            SaveOrUpdateParHeaderField(saveParHeaderFieldGeral);

            SaveOrUpdateParMultipleValues(saveParHeaderFieldGeral);

            return StatusCode(HttpStatusCode.NoContent);

        }

        private int SaveOrUpdateParHeaderField(ParHeaderFieldGeral parHeaderFieldGeral)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parHeaderFieldGeral.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parHeaderFieldToUpdate = db.ParHeaderFieldGeral.Find(parHeaderFieldGeral.Id);
                        parHeaderFieldToUpdate.Name = parHeaderFieldGeral.Name;
                        parHeaderFieldToUpdate.ParFieldType_Id = parHeaderFieldGeral.ParFieldType_Id;
                        parHeaderFieldToUpdate.LinkNumberEvaluation = parHeaderFieldGeral.LinkNumberEvaluation;
                        parHeaderFieldToUpdate.Description = parHeaderFieldGeral.Description;
                        parHeaderFieldToUpdate.IsActive = parHeaderFieldGeral.IsActive;
                        parHeaderFieldToUpdate.IsRequired = parHeaderFieldGeral.IsRequired;
                        parHeaderFieldToUpdate.Duplicate = parHeaderFieldGeral.Duplicate;
                        parHeaderFieldToUpdate.AlterDate = DateTime.Now;
                        parHeaderFieldToUpdate.Generic_Id = parHeaderFieldGeral.Generic_Id;
                        parHeaderFieldToUpdate.ParLevelHeaderField_Id = parHeaderFieldGeral.ParLevelHeaderField_Id;
                    }
                    else
                    {
                        parHeaderFieldGeral.AddDate = DateTime.Now;
                        parHeaderFieldGeral.Description = parHeaderFieldGeral.Description ?? "";
                        parHeaderFieldGeral.IsActive = true;

                        if (parHeaderFieldGeral.ParLevelHeaderField_Id != 4)
                            parHeaderFieldGeral.ParLevelHeaderField_Id = 3;// ParLevelHeaderField.Id = 3 - ParDeparment

                        for (int i = 0; i < parHeaderFieldGeral.ParMultipleValuesGeral.Count; i++)
                        {
                            db.Entry(parHeaderFieldGeral.ParMultipleValuesGeral.ElementAt(i)).State = EntityState.Detached;
                        }
                        if (parHeaderFieldGeral.ParLevelHeaderField != null)
                            db.Entry(parHeaderFieldGeral.ParLevelHeaderField).State = EntityState.Detached;
                        db.ParHeaderFieldGeral.Add(parHeaderFieldGeral);
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return 0;
                }

                return parHeaderFieldGeral.Id;
            }
        }

        private bool SaveOrUpdateParMultipleValues(ParHeaderFieldGeral parHeaderFieldGeral)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parHeaderFieldGeral.ParMultipleValuesGeral.Count > 0)
                    {
                        foreach (var parMultipleValue in parHeaderFieldGeral.ParMultipleValuesGeral)
                        {
                            if (parMultipleValue.Id > 0)
                            {
                                parMultipleValue.ParHeaderFieldGeral = null;
                                db.Entry(parMultipleValue).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                parMultipleValue.ParHeaderFieldGeral_Id = parHeaderFieldGeral.Id;
                                parMultipleValue.AddDate = DateTime.Now;
                                parMultipleValue.Description = "";
                                db.ParMultipleValuesGeral.Add(parMultipleValue);
                            }
                        }

                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }
        }

        [HttpGet]
        [Route("GetParLevel3Value/{id}")]
        public IHttpActionResult GetParLevel3Value(int id)
        {
            InicioRequisicao();
            ParLevel3Result parlevel3Result = new ParLevel3Result();
           // ParLevel3Value parLevel3Value = new ParLevel3Value();
           // ParLevel3 parLevel3 = new ParLevel3();
            List<ParHeaderFieldGeral> listParHeaderFieldGeral = new List<ParHeaderFieldGeral>();
            List<ParMultipleValuesGeral> parMultipleValuesGeral = new List<ParMultipleValuesGeral>();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                parMultipleValuesGeral = db.ParMultipleValuesGeral.Where(x => x.IsActive).ToList();
                //if (parLevel3 == null)
                //{
                //    return NotFound();
                //}

                try
                {
                    //parLevel3Value = db.ParLevel3Value.Where(x => x.Id == id).FirstOrDefault();

                   /// parLevel3 = db.ParLevel3.Where(x => x.Id == parLevel3Value.ParLevel3_Id).FirstOrDefault();

                    listParHeaderFieldGeral = db.ParHeaderFieldGeral.Where(x => x.Generic_Id == id && x.IsActive).ToList();

                    foreach (var item in listParHeaderFieldGeral)
                    {
                        item.ParFieldType = db.ParFieldType.Where(x => x.Id == item.ParFieldType_Id && x.IsActive).FirstOrDefault();
                    }

                    foreach (var item in listParHeaderFieldGeral)
                    {
                        foreach (var multipleValues in parMultipleValuesGeral)
                        {
                            if (multipleValues.ParHeaderFieldGeral_Id == item.Id)
                                item.ParMultipleValuesGeral.Add(multipleValues);
                        }
                    }


                }
                catch (Exception ex)
                {

                }

                //foreach (var item in parlevel3Result.Parlevel3.ParLevel3Value)
                //{
                //    item.ParInputTypeValues = db.ParInputTypeValues.Where(x => x.ParLevel3Value_Id == item.Id).ToList();
                //}

            }

            return Ok(listParHeaderFieldGeral);
        }

        // GET: api/ParLevel1Api/5
        //[ResponseType(typeof(ParLevel1Result))]
        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParLevel1(int id)
        {
            InicioRequisicao();
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

                try
                {
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


                }
                catch (Exception ex)
                {

                }


                foreach (var item in parlevel3Result.Parlevel3.ParLevel3Value)
                {
                    item.ParInputTypeValues = db.ParInputTypeValues.Where(x => x.ParLevel3Value_Id == item.Id).ToList();
                }

            }

            return Ok(parlevel3Result);
        }

        [HttpGet]
        [Route("GetPargroupQualificationXParLevel3Value/{level3Value_id}")]
        public IHttpActionResult GetPargroupQualificationXParLevel3Value(int level3Value_id)
        {
            InicioRequisicao();

            var lista = new List<PargroupQualificationXParLevel3Value>();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                lista = db.PargroupQualificationXParLevel3Value
                    .Where(x => x.ParLevel3Value_Id == level3Value_id && x.IsActive)
                    .ToList();

                foreach (var item in lista)
                {
                    item.ParLevel3Value = db.ParLevel3Value.Where(x => x.Id == item.ParLevel3Value_Id).FirstOrDefault();

                    item.PargroupQualification = db.PargroupQualification.Where(x => x.Id == item.PargroupQualification_Id).FirstOrDefault();
                }
            }

            return Ok(lista);
        }

        [HttpGet]
        [Route("GetParLevel3Vinculados/{ParLevel1_Id}/{ParLevel2_Id}")]
        public IHttpActionResult GetParLevel3Vinculados(int ParLevel1_Id, int ParLevel2_Id)
        {
            InicioRequisicao();
            var select = new Select();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                select.Options = new List<Option>();

                var linkedName = "Vinculado"; //"Linked"
                var notLinkedName = "Sem vinculo";

                var vinculados = db.Database.SqlQuery<Option>($@"SELECT
                                                                DISTINCT
                                                                	l3.Id AS Value
                                                                   ,l3.Name AS Text
                                                                   ,'{linkedName}' AS GroupName
                                                                FROM ParVinculoPeso vp WITH (NOLOCK)
                                                                INNER JOIN ParLevel3 l3 WITH (NOLOCK)
                                                                	ON vp.ParLevel3_Id = l3.Id
                                                                WHERE 1 = 1
                                                                AND vp.ParLevel1_Id = {ParLevel1_Id}
                                                                AND vp.ParLevel2_Id = {ParLevel2_Id}
                                                                AND vp.IsActive = 1").ToList();


                var naoVinculados = db.Database.SqlQuery<Option>($@"SELECT
                                                                	Id AS Value
                                                                   ,Name AS Text
                                                                   ,'{notLinkedName}' AS GroupName
                                                                FROM ParLevel3 WITH (NOLOCK)
                                                                WHERE Id NOT IN (SELECT
                                                                DISTINCT
                                                                	l3.Id
                                                                FROM ParVinculoPeso vp WITH (NOLOCK)
                                                                INNER JOIN ParLevel3 l3 WITH (NOLOCK)
                                                                	ON vp.ParLevel3_Id = l3.Id
                                                                WHERE 1 = 1
                                                                AND vp.ParLevel1_Id = {ParLevel1_Id}
                                                                AND vp.ParLevel2_Id = {ParLevel2_Id}
                                                                AND vp.IsActive = 1)
                                                                AND IsActive = 1").ToList();

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
            InicioRequisicao();
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
                        parLevel2Old.ParRiskCharacteristicType_Id = parLevel3.ParRiskCharacteristicType_Id;
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
            InicioRequisicao();
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

            InicioRequisicao();
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
                        parVinculoPesoOld.ParCluster_Id = parVinculoPeso.ParCluster_Id;
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
                    LogSystem.LogErrorBusiness.Register(ex, parVinculoPeso);
                    return false;
                }

                return true;
            }
        }

        [HttpPost]
        [Route("PostPargroupQualificationXParLevel3Value")]
        public IHttpActionResult PostPargroupQualificationXParLevel3Value(PargroupQualificationXParLevel3Value form)
        {
            InicioRequisicao();
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (!SaveOrUpdatePargroupQualificationXParLevel3Value(form))
                    {
                        return Ok(new { mensagem = "Já existe um vinculo com os dados inseridos." });
                    }

                    return StatusCode(HttpStatusCode.NoContent);
                }
                catch (Exception ex)
                {

                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
        }

        private bool SaveOrUpdatePargroupQualificationXParLevel3Value(PargroupQualificationXParLevel3Value form)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (validaDuplicidade(form))
                    {
                        if (form.Id > 0)
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            var pargroupQualificationXParLevel3ValueOld = db.PargroupQualificationXParLevel3Value.Find(form.Id);
                            pargroupQualificationXParLevel3ValueOld.PargroupQualification_Id = form.PargroupQualification_Id;
                            pargroupQualificationXParLevel3ValueOld.ParLevel3Value_Id = form.ParLevel3Value_Id;
                            pargroupQualificationXParLevel3ValueOld.Value = form.Value;
                            pargroupQualificationXParLevel3ValueOld.IsActive = form.IsActive;
                            pargroupQualificationXParLevel3ValueOld.IsRequired = form.IsRequired;
                        }
                        else
                        {
                            var pargroupQualificationXParLevel3ValueSalvo = db.PargroupQualificationXParLevel3Value.Add(form);
                            db.SaveChanges();
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {

                    return false;
                }

                return true;
            }
        }

        private void InactiveAllPargroupQualificationXParLevel3Value(int level3Value_id)
        {
            var lista = new List<PargroupQualificationXParLevel3Value>();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                lista = db.PargroupQualificationXParLevel3Value.Where(x => x.ParLevel3Value_Id == level3Value_id && x.IsActive).ToList();

                foreach (var item in lista)
                {
                    item.IsActive = false;

                    SaveOrUpdatePargroupQualificationXParLevel3Value(item);
                }
            }

        }

        private bool validaDuplicidade(PargroupQualificationXParLevel3Value form)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                var objSalvo = db.PargroupQualificationXParLevel3Value
                    .Where(x => x.PargroupQualification_Id == form.PargroupQualification_Id
                    && x.ParLevel3Value_Id == form.ParLevel3Value_Id
                    && x.Value == form.Value
                    && x.IsActive
                    && x.Id != form.Id).FirstOrDefault();

                if (objSalvo == null)
                    return true;
                else
                    return false;
            }
        }

        [HttpPost]
        [Route("PostParTipoDado")]
        public IHttpActionResult PostParTipoDado(ParLevel3Value parLevel3Value)
        {
            InicioRequisicao();
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
                        parVinculoPesoOld.ParCargo_Id = parLevel3Value.ParCargo_Id;
                        parVinculoPesoOld.ParDepartment_Id = parLevel3Value.ParDepartment_Id;
                        parVinculoPesoOld.ShowLevel3Limits = parLevel3Value.ShowLevel3Limits;
                        parVinculoPesoOld.IsRequired = parLevel3Value.IsRequired;
                        parVinculoPesoOld.IsDefaultAnswer = parLevel3Value.IsDefaultAnswer;
                        parVinculoPesoOld.ParCluster_Id = parLevel3Value.ParCluster_Id;
                        parVinculoPesoOld.IsAtiveNA = parLevel3Value.IsAtiveNA;
                        parVinculoPesoOld.DefaultMessageText = parLevel3Value.DefaultMessageText;
                        parVinculoPesoOld.StringSizeAllowed = parLevel3Value.StringSizeAllowed;
                        parVinculoPesoOld.LimiteNC = parLevel3Value.LimiteNC;
                        parVinculoPesoOld.IsNCTextRequired = parLevel3Value.IsNCTextRequired;

                        foreach (var item in parLevel3Value.ParInputTypeValues)
                        {
                            if (item.Valor > 0)
                            {
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
                        }

                        //Se não for Binário, Binario Com Texto e Binário com múltipla escolha - Excluir qualificações vinculadas
                        if (!parInputTypesWithQualificacao.Contains(parLevel3Value.ParLevel3InputType_Id))
                        {
                            InactiveAllPargroupQualificationXParLevel3Value(parLevel3Value.Id);
                        }
                    }
                    else
                    {
                        var parLevel3ValueSalvo = db.ParLevel3Value.Add(parLevel3Value);
                        db.SaveChanges();

                        foreach (var item in parLevel3Value.ParInputTypeValues)
                        {
                            if (item.Valor > 0)
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
            InicioRequisicao();
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

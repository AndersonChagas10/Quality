using Dominio;
using SgqSystem.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/ParLevel1Api")]
    public class ParLevel1ApiController : BaseApiController
    {

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetParLevel1()
        {
            InicioRequisicao();
            ParLevel1Selects parLevel1Selects = new ParLevel1Selects();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parLevel1Selects.ParLevels1 = db.ParLevel1.ToList();
            }

            return Ok(parLevel1Selects);
        }

        [Route("GetSelectsL1")]
        public IHttpActionResult GetSelectsParLevel1()
        {
            InicioRequisicao();
            ParLevel1Selects parLevel1Selects = new ParLevel1Selects();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                parLevel1Selects.ParCriticalLevels = db.ParCriticalLevel.Where(x => x.IsActive == true).ToList();
                parLevel1Selects.ParClusters = db.ParCluster.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParConsolidationTypes = db.ParConsolidationType.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParScoreTypes = db.ParScoreType.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParFrequencies = db.ParFrequency.Where(x => x.IsActive).ToList();

                //Tabelas
                parLevel1Selects.ParHeaderFieldsGeral = db.ParHeaderFieldGeral.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParFieldTypes = db.ParFieldType.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParLevelDefinitons = db.ParLevelDefiniton.Where(x => x.IsActive).ToList();

                //Avam
                parLevel1Selects.ParCompanys = db.ParCompany.Where(x => x.IsActive).ToList();

                //Tipo dado entrada
                parLevel1Selects.ParLevel3InputTypes = db.ParLevel3InputType.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParLevel3BoolFalses = db.ParLevel3BoolFalse.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParLevel3BoolTrues = db.ParLevel3BoolTrue.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParMeasurementUnits = db.ParMeasurementUnit.Where(x => x.IsActive).ToList();

                parLevel1Selects.ParRiskCharacteristicTypes = db.ParRiskCharacteristicType.Where(x => x.IsActive).ToList();

                //Peso
                var listaDepartamentos = db.ParDepartment.Where(x => x.Active).ToList();
                //parLevel1Selects.ParDepartments = listaDepartamentos.Where(y => !listaDepartamentos.Any(y1 => y1.Parent_Id == y.Id)).ToList();

                parLevel1Selects.ParGroupParLevel1s = db.ParGroupParLevel1.Where(x => x.IsActive).ToList();
                //parLevel1Selects.ParCargos = db.ParCargo.Where(x => x.IsActive).ToList();

                //parLevel1Selects.ParCargoXDepartments = db.ParCargoXDepartment.Where(x => x.IsActive).ToList();
                parLevel1Selects.RotinaIntegracao = db.RotinaIntegracao.Where(x => x.IsActive).ToList();

                //Familia
                parLevel1Selects.ParFamiliaProdutos = db.ParFamiliaProduto.Where(x => x.IsActive).ToList();

                //grupo qualificação
                parLevel1Selects.PargroupQualification = db.PargroupQualification.Where(x => x.IsActive).ToList();
            }

            return Ok(parLevel1Selects);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParLevel1(int id)
        {
            InicioRequisicao();
            ParLevel1Result parlevel1Result = new ParLevel1Result();
            ParLevel1 parLevel1 = new ParLevel1();
            List<ParMultipleValuesGeral> parMultipleValuesGeral = new List<ParMultipleValuesGeral>();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                parLevel1 = db.ParLevel1.Where(x => x.Id == id).FirstOrDefault();

                parLevel1.ParLevel1XCluster = db.ParLevel1XCluster.Where(x => x.IsActive && x.ParLevel1_Id == parLevel1.Id).ToList();
                parLevel1.ParHeaderFieldsGeral = db.ParHeaderFieldGeral
                    .Where(x => x.IsActive && x.ParLevelHeaderField_Id == 1 && x.Generic_Id == parLevel1.Id)
                    //.Include("ParMultipleValuesGeral")
                    .Include("ParFieldType")
                    .ToList();

                parMultipleValuesGeral = db.ParMultipleValuesGeral.Where(x => x.IsActive).ToList();

                parLevel1.ParLevel1XParFamiliaProduto = db.ParLevel1XParFamiliaProduto.Where(x => x.IsActive && x.ParLevel1_Id == parLevel1.Id).ToList();

                foreach (var item in parLevel1.ParLevel1XCluster)
                {
                    item.ParCluster = db.ParCluster.Where(x => x.IsActive && x.Id == item.ParCluster_Id).FirstOrDefault();
                    item.ParCriticalLevel = db.ParCriticalLevel.Where(x => x.IsActive == true && x.Id == item.ParCriticalLevel_Id).FirstOrDefault();
                }

                foreach (var item in parLevel1.ParHeaderFieldsGeral)
                {
                    foreach (var multipleValues in parMultipleValuesGeral)
                    {
                        if (multipleValues.ParHeaderFieldGeral_Id == item.Id)
                            item.ParMultipleValuesGeral.Add(multipleValues);
                    }
                }

                if (parLevel1 == null)
                {
                    return NotFound();
                }

            }

            return Ok(parLevel1);
        }

        //Salvar ou Editar
        [HttpPost]
        [Route("PostParLevel1")]
        public IHttpActionResult PostParLevel1(ParLevel1 parLevel1)
        {
            InicioRequisicao();
            SaveOrUpdateParLevel1(parLevel1);

            return Ok(parLevel1.Id);
        }

        [HttpPost]
        [Route("PostParLevel1Avancados")]
        public IHttpActionResult PostParLevel1Avancados(ParLevel1 parLevel1)
        {
            InicioRequisicao();
            SaveOrUpdateParLevel1(parLevel1, true);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1Familia")]
        public IHttpActionResult PostParLevel1Familia(Dominio.Seara.ParLevel1XParFamiliaProduto parLevel1XParFamiliaProduto)
        {
            InicioRequisicao();
            SaveOrUpdatParLevel1XParFamiliaProduto(parLevel1XParFamiliaProduto);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParHeaderFieldGeral")]
        public IHttpActionResult PostParHeaderField(ParHeaderFieldGeral saveParHeaderFieldGeral)
        {
            InicioRequisicao();
            SaveOrUpdateParHeaderField(saveParHeaderFieldGeral);

            SaveOrUpdateParMultipleValues(saveParHeaderFieldGeral);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1XCluster")]
        public IHttpActionResult PostParLevel1XCluster(ParLevel1XCluster parLevel1XCluster)
        {
            InicioRequisicao();
            if (!SaveOrUpdateParLevel1XCluster(parLevel1XCluster))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("GetParCargoXDepartmentByUnit/{id}")]
        public IHttpActionResult GetDepartmentsByUnit(int id)
        {
            InicioRequisicao();
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var ParDepartmentsPai = db.ParDepartment.Where(x => x.ParCompany_Id == id)
                    .Select(x => x.Parent_Id)
                    .ToList();

                var ParDepartmentsFilhos = db.ParDepartment
                    .Where(x => x.ParCompany_Id == id && x.Parent_Id != null && !ParDepartmentsPai.Any(y => y == x.Id))
                    .Include(x => x.ParDepartmentPai).Where(x => x.ParCompany_Id == id && x.Parent_Id != null)
                    .ToList();

                var departamentosIds = ParDepartmentsFilhos.Select(x => x.Id).ToList();

                var ParCargoXDepartment = db.ParCargoXDepartment.Where(x => departamentosIds.Contains(x.ParDepartment_Id)).ToList();

                var ParCargoIds = ParCargoXDepartment.Select(x => x.ParCargo_Id).ToList();

                var ParCargos = db.ParCargo.Where(x => ParCargoIds.Contains(x.Id)).ToList();

                dynamic retorno = new ExpandoObject();

                retorno.ParDepartments = ParDepartmentsFilhos;
                retorno.ParCargos = ParCargos;
                retorno.ParCargoXDepartments = ParCargoXDepartment;

                return Ok(retorno);
            }
        }

        private bool SaveOrUpdateParLevel1(ParLevel1 parLevel1, bool isAvancado = false)
        {
            if (parLevel1.Id > 0)//Alter
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;

                    var parLevel1Old = db.ParLevel1.Find(parLevel1.Id);

                    if (isAvancado)
                    {
                        parLevel1Old.IsLimitedEvaluetionNumber = parLevel1.IsLimitedEvaluetionNumber;
                        parLevel1Old.HasTakePhoto = parLevel1.HasTakePhoto;
                    }
                    else
                    {
                        parLevel1Old.Name = parLevel1.Name;
                        parLevel1Old.Description = parLevel1.Description;
                        parLevel1Old.ParConsolidationType_Id = parLevel1.ParConsolidationType_Id;
                        parLevel1Old.ParFrequency_Id = parLevel1.ParFrequency_Id;
                        parLevel1Old.ParScoreType_Id = parLevel1.ParScoreType_Id;
                        parLevel1Old.IsActive = parLevel1.IsActive;

                    }

                    parLevel1Old.AlterDate = DateTime.Now;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            else //Insert
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    parLevel1.AddDate = DateTime.Now;

                    db.ParLevel1.Add(parLevel1);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }

            return true;
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
                    }
                    else
                    {
                        parHeaderFieldGeral.AddDate = DateTime.Now;
                        parHeaderFieldGeral.Description = parHeaderFieldGeral.Description ?? "";
                        parHeaderFieldGeral.IsActive = true;
                        parHeaderFieldGeral.ParLevelHeaderField_Id = 1;// ParLevelHeaderField.Id = 1 - ParLevel1
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

        private bool SaveOrUpdateParLevel1XCluster(ParLevel1XCluster parLevel1XClusters)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parLevel1XClusters.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parLevel1XClusterToUpdate = db.ParLevel1XCluster.Find(parLevel1XClusters.Id);
                        parLevel1XClusterToUpdate.IsActive = parLevel1XClusters.IsActive;
                        parLevel1XClusterToUpdate.ParCluster_Id = parLevel1XClusters.ParCluster_Id;
                        parLevel1XClusterToUpdate.ParCriticalLevel_Id = parLevel1XClusters.ParCriticalLevel_Id;
                        parLevel1XClusterToUpdate.Points = parLevel1XClusters.Points;
                        parLevel1XClusterToUpdate.ValidoApartirDe = parLevel1XClusters.ValidoApartirDe;
                        parLevel1XClusterToUpdate.EffectiveDate = parLevel1XClusters.EffectiveDate;
                        db.Entry(parLevel1XClusterToUpdate).State = EntityState.Modified;
                    }
                    else
                    {
                        db.ParLevel1XCluster.Add(parLevel1XClusters);
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

        private bool ParLevel1Exists(int id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                return db.ParLevel1.Count(e => e.Id == id) > 0;
            }
        }

        private bool SaveOrUpdatParLevel1XParFamiliaProduto(Dominio.Seara.ParLevel1XParFamiliaProduto parLevel1XParFamiliaProduto)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {

                var parLevel1xParFamiliaProdutoOld = db.ParLevel1XParFamiliaProduto.Where(x => x.ParLevel1_Id == parLevel1XParFamiliaProduto.ParLevel1_Id).FirstOrDefault();

                if (parLevel1xParFamiliaProdutoOld == null) //Add
                {

                    if (parLevel1XParFamiliaProduto.ParFamiliaProduto_Id == 0) //salvou sem vinculo
                        return true;

                    parLevel1XParFamiliaProduto.AddDate = DateTime.Now;
                    parLevel1XParFamiliaProduto.IsActive = true;

                    db.ParLevel1XParFamiliaProduto.Add(parLevel1XParFamiliaProduto);
                }
                else //Update or delete
                {

                    if (parLevel1XParFamiliaProduto.ParFamiliaProduto_Id == 0) //Delete
                        db.ParLevel1XParFamiliaProduto.Remove(parLevel1xParFamiliaProdutoOld);

                    else //Update
                    {

                        parLevel1xParFamiliaProdutoOld.AlterDate = DateTime.Now;
                        parLevel1xParFamiliaProdutoOld.ParFamiliaProduto_Id = parLevel1XParFamiliaProduto.ParFamiliaProduto_Id;
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }

            }

            return true;
        }

        public class ParLevel1Result
        {
            public ParLevel1 Parlevel1 { get; set; }
            //public List<ParLevel1XHeaderField> ParLevel1XHeaderFields { get; set; }
            public List<ParLevel1XCluster> ParLevel1XClusters { get; set; }
            public List<ParHeaderFieldGeral> ParHeaderFieldGeral { get; set; }
        }

        public class ParLevel1Selects
        {
            public List<ParLevel1> ParLevels1 { get; set; }
            public List<ParHeaderFieldGeral> ParHeaderFieldsGeral { get; set; }
            public List<ParCluster> ParClusters { get; set; }
            public List<ParCriticalLevel> ParCriticalLevels { get; set; }
            public List<ParFieldType> ParFieldTypes { get; set; }
            public List<ParLevelDefiniton> ParLevelDefinitons { get; set; }
            public List<ParConsolidationType> ParConsolidationTypes { get; set; }
            public List<ParFrequency> ParFrequencies { get; set; }
            public List<ParScoreType> ParScoreTypes { get; set; }

            public List<ParCompany> ParCompanys { get; set; }
            public List<ParDepartment> ParDepartments { get; set; }
            public List<ParGroupParLevel1> ParGroupParLevel1s { get; set; }
            public List<ParRiskCharacteristicType> ParRiskCharacteristicTypes { get; set; }

            public List<ParLevel3InputType> ParLevel3InputTypes { get; set; }
            public List<ParLevel3BoolTrue> ParLevel3BoolTrues { get; set; }
            public List<ParLevel3BoolFalse> ParLevel3BoolFalses { get; set; }
            public List<ParMeasurementUnit> ParMeasurementUnits { get; set; }

            public IEnumerable<SelectListItem> SelectLevel2Vinculados { get; set; }
            public List<ParCargo> ParCargos { get; internal set; }

            public List<ParCargoXDepartment> ParCargoXDepartments { get; set; }
            public List<RotinaIntegracao> RotinaIntegracao { get; set; }

            public List<Dominio.Seara.ParFamiliaProduto> ParFamiliaProdutos { get; set; }

            public List<PargroupQualification> PargroupQualification { get; set; }
        }

        public class SaveParHeaderField
        {

            public int ParLevel1_Id { get; set; }
            public ParHeaderField ParHeaderField { get; set; }

        }


        public class SelectListItem
        {
            public bool Disabled { get; set; }
            public SelectListGroup Group { get; set; }
            public bool Selected { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public class SelectListGroup
        {
            public bool Disabled { get; set; }
            public string Name { get; set; }
        }

        //public class ParLevel1ViewModel
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public int ParConsolidationType_Id { get; set; }
        //    public int ParFrequency_Id { get; set; }
        //    public int? ParScoreType_Id { get; set; }
        //    public bool IsLimitedEvaluetionNumber { get; set; }
        //    public bool HasTakePhoto { get; set; }
        //    public bool IsActive { get; set; }
        //}
    }
}

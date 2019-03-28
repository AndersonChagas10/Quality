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

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetParLevel1()
        {
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
                //parLevel1Selects.ParHeaderFields = db.ParHeaderField.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParFieldTypes = db.ParFieldType.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParLevelDefinitons = db.ParLevelDefiniton.Where(x => x.IsActive).ToList();

                //Avam
                parLevel1Selects.ParCompanys = db.ParCompany.Where(x => x.IsActive).ToList();

            }

            return Ok(parLevel1Selects);
        }

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
                parlevel1Result.ParLevel1XClusters = db.ParLevel1XCluster
                    .Include(x => x.ParCluster)
                    .Include(x => x.ParCriticalLevel)
                    .Where(x => x.ParLevel1_Id == parLevel1.Id && x.IsActive).ToList();
                parlevel1Result.ParLevel1XHeaderFields = db.ParLevel1XHeaderField.Where(x => x.ParLevel1_Id == parLevel1.Id && x.IsActive).ToList();
                var headerFieldsId = parlevel1Result.ParLevel1XHeaderFields.Select(xx => xx.ParHeaderField_Id).ToList();
                parlevel1Result.ParHeaderFields = db.ParHeaderField.Where(x => x.IsActive && headerFieldsId.Contains(x.Id))
                    .Include(x => x.ParLevelDefiniton)
                    .Include(x => x.ParFieldType)
                    .Include(x => x.ParMultipleValues).ToList();
            }

            return Ok(parlevel1Result);
        }

        [HttpPost]
        [Route("PostParLevel1")]
        public IHttpActionResult PostParLevel1(ParLevel1 parLevel1)
        {
            SaveOrUpdateParLevel1(parLevel1);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1Avancados")]
        public IHttpActionResult PostParLevel1Avancados(ParLevel1 parLevel1)
        {
            SaveOrUpdateParLevel1(parLevel1, true);

            return StatusCode(HttpStatusCode.NoContent);
        }


        [HttpPost]
        [Route("PostParHeaderField")]
        public IHttpActionResult PostParHeaderField(SaveParHeaderField saveParHeaderField)
        {
            if (saveParHeaderField.ParHeaderField.Id > 0)
            {
                SaveOrUpdateParHeaderField(saveParHeaderField.ParHeaderField);
            }
            else
            {
                SaveOrUpdateParHeaderField(saveParHeaderField.ParHeaderField);
                SaveOrUpdateParLevel1XHeaderField(null, saveParHeaderField.ParHeaderField.Id, saveParHeaderField.ParLevel1_Id);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1XHeaderField")]
        public IHttpActionResult PostParLevel1XHeaderField(ParLevel1XHeaderField parLevel1XHeaderField)
        {
            SaveOrUpdateParLevel1XHeaderField(parLevel1XHeaderField, 0, 0);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1XCluster")]
        public IHttpActionResult PostParLevel1XCluster(ParLevel1XCluster parLevel1XCluster)
        {
            if (!SaveOrUpdateParLevel1XCluster(parLevel1XCluster))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
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
                    catch (Exception)
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
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int SaveOrUpdateParHeaderField(ParHeaderField parHeaderField)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parHeaderField.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parHeaderFieldToUpdate = db.ParHeaderField.Find(parHeaderField.Id);
                        parHeaderFieldToUpdate.Name = parHeaderField.Name;
                        parHeaderFieldToUpdate.ParFieldType_Id = parHeaderField.ParFieldType_Id;
                        parHeaderFieldToUpdate.LinkNumberEvaluetion = parHeaderField.LinkNumberEvaluetion;
                        parHeaderFieldToUpdate.ParLevelDefinition_Id = parHeaderField.ParLevelDefinition_Id;
                        parHeaderFieldToUpdate.Description = parHeaderField.Description;
                        parHeaderFieldToUpdate.IsActive = parHeaderField.IsActive;
                        parHeaderFieldToUpdate.IsRequired = parHeaderField.IsRequired;
                        parHeaderFieldToUpdate.duplicate = parHeaderField.duplicate;
                        parHeaderFieldToUpdate.CheckBox = parHeaderField.CheckBox;
                        parHeaderFieldToUpdate.AlterDate = DateTime.Now;
                    }
                    else
                    {
                        parHeaderField.AddDate = DateTime.Now;
                        parHeaderField.Description = "";
                        db.ParHeaderField.Add(parHeaderField);
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return 0;
                }

                return parHeaderField.Id;
            }
        }

        private bool SaveOrUpdateParLevel1XHeaderField(ParLevel1XHeaderField parLevel1XHeaderField, int parHeaderField_Id, int parLevel1_Id)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;

                    if (parLevel1XHeaderField != null && parLevel1XHeaderField.Id > 0) //Update ou Inactive
                    {

                        //Update todos para isActive = false
                        var parLevel1XHeaderFieldToModfy = db.ParLevel1XHeaderField.Find(parLevel1XHeaderField.Id);

                        parLevel1XHeaderFieldToModfy.IsActive = parLevel1XHeaderField.IsActive;
                        parLevel1XHeaderFieldToModfy.ParHeaderField_Id = parLevel1XHeaderField.ParHeaderField_Id;
                        parLevel1XHeaderFieldToModfy.IsRequired = parLevel1XHeaderField.IsRequired;
                        parLevel1XHeaderFieldToModfy.DefaultSelected = parLevel1XHeaderField.DefaultSelected;
                        parLevel1XHeaderFieldToModfy.HeaderFieldGroup = parLevel1XHeaderFieldToModfy.HeaderFieldGroup;
                        parLevel1XHeaderFieldToModfy.AlterDate = DateTime.Now;

                    }
                    else //Insert
                    {
                        parLevel1XHeaderField = new ParLevel1XHeaderField()
                        {
                            AddDate = DateTime.Now,
                            IsActive = true,
                            ParHeaderField_Id = parHeaderField_Id,
                            ParLevel1_Id = parLevel1_Id
                        };

                        db.ParLevel1XHeaderField.Add(parLevel1XHeaderField);

                    }

                    db.SaveChanges();
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
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

        private bool ParLevel1Exists(int id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                return db.ParLevel1.Count(e => e.Id == id) > 0;
            }
        }

        public class ParLevel1Result
        {
            public ParLevel1 Parlevel1 { get; set; }
            public List<ParLevel1XHeaderField> ParLevel1XHeaderFields { get; set; }
            public List<ParLevel1XCluster> ParLevel1XClusters { get; set; }
            public List<ParHeaderField> ParHeaderFields { get; set; }
        }

        public class ParLevel1Selects
        {
            public List<ParLevel1> ParLevels1 { get; set; }
            public List<ParHeaderField> ParHeaderFields { get; set; }
            public List<ParCluster> ParClusters { get; set; }
            public List<ParCriticalLevel> ParCriticalLevels { get; set; }
            public List<ParFieldType> ParFieldTypes { get; set; }
            public List<ParLevelDefiniton> ParLevelDefinitons { get; set; }
            public List<ParConsolidationType> ParConsolidationTypes { get; set; }
            public List<ParFrequency> ParFrequencies { get; set; }
            public List<ParScoreType> ParScoreTypes { get; set; }

            public List<ParCompany> ParCompanys { get; set; }
        }

        public class SaveParHeaderField {

            public int ParLevel1_Id { get; set; }
            public ParHeaderField ParHeaderField { get; set; }

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

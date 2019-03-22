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
                parLevel1Selects.ParClusters = db.ParCluster.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParHeaderFields = db.ParHeaderField.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParFieldTypes = db.ParFieldType.Where(x => x.IsActive).ToList();
                parLevel1Selects.ParLevelDefinitons = db.ParLevelDefiniton.Where(x => x.IsActive).ToList();
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
                parlevel1Result.ParLevel1XClusters = db.ParLevel1XCluster.Where(x => x.ParLevel1_Id == parLevel1.Id && x.IsActive).ToList();
                parlevel1Result.ParLevel1XHeaderFields = db.ParLevel1XHeaderField.Where(x => x.ParLevel1_Id == parLevel1.Id && x.IsActive).ToList();
                parlevel1Result.ParHeaderFields = db.ParHeaderField.Where(x => x.IsActive && parlevel1Result.ParLevel1XHeaderFields.Select(xx => xx.ParHeaderField_Id).Contains(x.Id)).ToList();
            }

            return Ok(parlevel1Result);
        }

        [HttpPost]
        [Route("PostParLevel1")]
        public IHttpActionResult PostParLevel1(ParLevel1Result parLevel1Result)
        {
            SaveOrUpdateParLevel1(parLevel1Result.Parlevel1);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParHeaderField")]
        public IHttpActionResult PostParHeaderField(ParLevel1Result parLevel1Result)
        {
            SaveOrUpdateParHeaderField(parLevel1Result.ParHeaderFields);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1XHeaderField")]
        public IHttpActionResult PostParLevel1XHeaderField(ParLevel1Result parLevel1Result)
        {
            SaveOrUpdateParLevel1XHeaderField(parLevel1Result.ParLevel1XHeaderFields, parLevel1Result.Parlevel1.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("PostParLevel1XCluster")]
        public IHttpActionResult PostParLevel1XCluster(ParLevel1Result parLevel1Result)
        {
            SaveOrUpdateParLevel1XCluster(parLevel1Result.ParLevel1XClusters, parLevel1Result.Parlevel1.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool SaveOrUpdateParLevel1(ParLevel1 parLevel1)
        {
            if (parLevel1.Id > 0)//Alter
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {

                    db.Entry(parLevel1).State = EntityState.Modified;
                    parLevel1.AlterDate = DateTime.Now;

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

        private bool SaveOrUpdateParHeaderField(List<ParHeaderField> ParHeaderFields)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    db.ParHeaderField.AddRange(ParHeaderFields.Where(x => x.Id <= 0));//add
                    //Colocar o AddDate

                    foreach (var parHeaderField in db.ParHeaderField.Where(x => x.Id > 0)) //update
                    {
                        db.Entry(parHeaderField).State = EntityState.Modified;
                        parHeaderField.AlterDate = DateTime.Now;
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

        private bool SaveOrUpdateParLevel1XHeaderField(List<ParLevel1XHeaderField> parLevel1XHeaderFielsdAdd, int parLevel_Id)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    //Update todos para isActive = false
                    var parLevel1XHeaderFields = db.ParLevel1XHeaderField.Where(x => x.ParLevel1_Id == parLevel_Id);
                    foreach (var parLevel1XHeaderField in parLevel1XHeaderFields)
                    {
                        parLevel1XHeaderField.AlterDate = DateTime.Now;
                        parLevel1XHeaderField.IsActive = false;
                    }

                    //Insere novos relacionamentos
                    db.ParLevel1XHeaderField.AddRange(parLevel1XHeaderFielsdAdd);
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
            }
        }

        private bool SaveOrUpdateParLevel1XCluster(List<ParLevel1XCluster> ParLevel1XClustersAdd, int parLevel_Id)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    //Update todos para isActive = false
                    var parLevel1XClusters = db.ParLevel1XCluster.Where(x => x.ParLevel1_Id == parLevel_Id);
                    foreach (var parLevel1XHeaderField in parLevel1XClusters)
                    {
                        parLevel1XHeaderField.AlterDate = DateTime.Now;
                        parLevel1XHeaderField.IsActive = false;
                    }

                    //Insere novos relacionamentos
                    db.ParLevel1XCluster.AddRange(ParLevel1XClustersAdd);
                }
                catch (Exception)
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

        //Criar um model para os retornos das classes
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
            public List<ParFieldType> ParFieldTypes { get; set; }
            public List<ParLevelDefiniton> ParLevelDefinitons { get; set; }
        }
    }
}

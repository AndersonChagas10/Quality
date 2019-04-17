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
    [RoutePrefix("api/ParEvaluationXDepartmentXCargoApi")]
    public class ParEvaluationXDepartmentXCargoApiController : BaseApiController
    {

        public class ParEvaluationXDepartmentXCargoResult
        {
            public ParDepartment ParDepartment { get; set; }
            public List<ParEvaluationXDepartmentXCargo> ParEvaluationXDepartmentXCargo { get; set; }
            public List<ParCargo> ParCargo { get; set; }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParDepartment(int id)
        {
            ParEvaluationXDepartmentXCargoResult parEvaluationXDepartmentXCargoResult = new ParEvaluationXDepartmentXCargoResult();
            ParDepartment parEvaluationXDepartmentXCargo = new ParDepartment();

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                parEvaluationXDepartmentXCargo = db.ParDepartment.Find(id);

                if (parEvaluationXDepartmentXCargo == null)
                {
                    return NotFound();
                }

                parEvaluationXDepartmentXCargoResult.ParDepartment = parEvaluationXDepartmentXCargo;
                parEvaluationXDepartmentXCargoResult.ParEvaluationXDepartmentXCargo =
                    db.ParEvaluationXDepartmentXCargo
                    .Where(x => x.IsActive == true
                    && x.ParDepartment_Id == parEvaluationXDepartmentXCargo.Id).ToList();

                    var parCargoXDepartment = db.ParCargoXDepartment.AsNoTracking()
                    .Where(x => x.IsActive == true
                    && x.ParDepartment_Id == parEvaluationXDepartmentXCargo.Id)
                    .ToList();

                parEvaluationXDepartmentXCargoResult.ParCargo = db.ParCargo
                    .AsNoTracking()
                    .ToList()
                    .Where(x => parCargoXDepartment.Any(y => y.ParCargo_Id == x.Id))
                    .ToList();

                foreach (var item in parEvaluationXDepartmentXCargoResult.ParEvaluationXDepartmentXCargo)
                {
                    item.ParEvaluationSchedule = db.ParEvaluationSchedule.Where(x => x.ParEvaluationXDepartmentXCargo_Id == item.Id).ToList();
                }
                  

            }

            return Ok(parEvaluationXDepartmentXCargoResult);
        }

        [HttpPost]
        [Route("PostParEvaluationXDepartmentXCargo")]
        public IHttpActionResult PostParEvaluationXDepartmentXCargo(ParEvaluationXDepartmentXCargo parEvaluationXDepartmentXCargo)
        {
            if (!SaveOrUpdateParEvaluationXDepartmentXCargo(parEvaluationXDepartmentXCargo))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool SaveOrUpdateParEvaluationXDepartmentXCargo(ParEvaluationXDepartmentXCargo parEvaluationXDepartmentXCargo)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    parEvaluationXDepartmentXCargo.ParCargo_Id = parEvaluationXDepartmentXCargo.ParCargo_Id > 0 ? parEvaluationXDepartmentXCargo.ParCargo_Id : null;
                    parEvaluationXDepartmentXCargo.ParCompany_Id = parEvaluationXDepartmentXCargo.ParCompany_Id > 0 ? parEvaluationXDepartmentXCargo.ParCompany_Id : null;

                    if (parEvaluationXDepartmentXCargo.Id > 0)
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        var parEvaluationXDepartmentXCargoOld = db.ParEvaluationXDepartmentXCargo.Find(parEvaluationXDepartmentXCargo.Id);
                        parEvaluationXDepartmentXCargoOld.ParDepartment_Id = parEvaluationXDepartmentXCargo.ParDepartment_Id;
                        parEvaluationXDepartmentXCargoOld.ParCargo_Id = parEvaluationXDepartmentXCargo.ParCargo_Id;
                        parEvaluationXDepartmentXCargoOld.ParCompany_Id = parEvaluationXDepartmentXCargo.ParCompany_Id;
                        parEvaluationXDepartmentXCargoOld.Evaluation = parEvaluationXDepartmentXCargo.Evaluation;
                        parEvaluationXDepartmentXCargoOld.Sample = parEvaluationXDepartmentXCargo.Sample;
                        parEvaluationXDepartmentXCargoOld.IsActive = parEvaluationXDepartmentXCargo.IsActive;
                        parEvaluationXDepartmentXCargoOld.ParFrequencyId = parEvaluationXDepartmentXCargo.ParFrequencyId;

                        foreach (var item in parEvaluationXDepartmentXCargo.ParEvaluationSchedule)
                        {
                            item.ParEvaluationXDepartmentXCargo_Id = parEvaluationXDepartmentXCargo.Id;
                            if (item.Intervalo != null)
                            {
                                item.Inicio = null;
                                item.Fim = null;
                            }
                          
                            if (item.Shift_Id <= 0)
                                item.Shift_Id = null;

                            if (item.Id > 0)
                            {
                                db.Entry(item).State = EntityState.Modified;
                            }
                            else
                            {
                                db.ParEvaluationSchedule.Add(item);
                            }
                            db.SaveChanges();
                           
                        }
                    }
                    else
                    {
                        var parEvaluationXDepartamentoXCargo = db.ParEvaluationXDepartmentXCargo.Add(parEvaluationXDepartmentXCargo);
                        db.SaveChanges();

                        foreach (var item in parEvaluationXDepartmentXCargo.ParEvaluationSchedule)
                        {
                            item.ParEvaluationXDepartmentXCargo_Id = parEvaluationXDepartamentoXCargo.Id;
                            if (item.Intervalo != null)
                            {
                                item.Inicio = null;
                                item.Fim = null;
                            }

                            if (item.Shift_Id <= 0)
                                item.Shift_Id = null;

                            if (item.Id > 0)
                            {
                                db.Entry(item).State = EntityState.Modified;
                            }
                            else
                            {
                                db.ParEvaluationSchedule.Add(item);
                            }
                            db.SaveChanges();
   
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
        
    }
}

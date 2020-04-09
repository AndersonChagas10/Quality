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
            public List<ParDepartmentXRotinaIntegracao> ParDepartmentXRotinaIntegracao { get; set; }
            //public List<ParDepartmentXHeaderField> ParDepartmentXHeaderField { get; set; }
            public List<ParHeaderFieldGeral> ParHeaderFieldGeral { get; set; }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetParDepartment(int id)
        {
            InicioRequisicao();
            ParEvaluationXDepartmentXCargoResult parEvaluationXDepartmentXCargoResult = new ParEvaluationXDepartmentXCargoResult();
            ParDepartment parEvaluationXDepartmentXCargo = new ParDepartment();
            List<ParMultipleValuesGeral> parMultipleValuesGeral = new List<ParMultipleValuesGeral>();

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

                parEvaluationXDepartmentXCargoResult.ParDepartmentXRotinaIntegracao = db.ParDepartmentXRotinaIntegracao.Where(x => x.ParDepartment_Id == id && x.IsActive).Include(y => y.RotinaIntegracao).ToList();

                //parEvaluationXDepartmentXCargoResult.ParDepartmentXHeaderField = db.ParDepartmentXHeaderField.Where(x => x.ParDepartment_Id == id && x.IsActive)
                //    .Include(y => y.ParHeaderField)
                //    .Include(u => u.ParHeaderField.ParLevelDefiniton)
                //    .Include(x => x.ParHeaderField.ParMultipleValues)
                //    .Include(x => x.ParHeaderField.ParFieldType)
                //    .ToList();

                const int parLevelHeaderField = 3; //Department

                parEvaluationXDepartmentXCargoResult.ParHeaderFieldGeral = db.ParHeaderFieldGeral.Where(x => x.ParLevelHeaderField_Id == parLevelHeaderField && x.Generic_Id == id && x.IsActive)
                    //.Include(x => x.ParMultipleValuesGeral)
                    .Include(x => x.ParFieldType)
                    .ToList();

                parMultipleValuesGeral = db.ParMultipleValuesGeral.Where(x => x.IsActive).ToList();

                foreach (var item in parEvaluationXDepartmentXCargoResult.ParHeaderFieldGeral)
                {
                    foreach (var multipleValues in parMultipleValuesGeral)
                    {
                        if (multipleValues.ParHeaderFieldGeral_Id == item.Id)
                            item.ParMultipleValuesGeral.Add(multipleValues);
                    }
                }

            }

            return Ok(parEvaluationXDepartmentXCargoResult);
        }

        [HttpPost]
        [Route("PostParEvaluationXDepartmentXCargo")]
        public IHttpActionResult PostParEvaluationXDepartmentXCargo(ParEvaluationXDepartmentXCargo parEvaluationXDepartmentXCargo)
        {
            InicioRequisicao();
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
                        parEvaluationXDepartmentXCargoOld.ParCluster_Id = parEvaluationXDepartmentXCargo.ParCluster_Id;
                        parEvaluationXDepartmentXCargoOld.IsPartialCollection = parEvaluationXDepartmentXCargo.IsPartialCollection;
                        parEvaluationXDepartmentXCargoOld.RedistributeWeight = parEvaluationXDepartmentXCargo.RedistributeWeight;

                        if (parEvaluationXDepartmentXCargo.IsAgendamento)
                        {
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
                    }
                    else
                    {
                        var parEvaluationXDepartamentoXCargoSalvo = db.ParEvaluationXDepartmentXCargo.Add(parEvaluationXDepartmentXCargo);
                        db.SaveChanges();

                        //SalvarEditarAgendamento(parEvaluationXDepartmentXCargo, parEvaluationXDepartamentoXCargoSalvo.Id);
                        if (parEvaluationXDepartmentXCargo.ParEvaluationSchedule != null && parEvaluationXDepartmentXCargo.IsAgendamento)
                        {
                            foreach (var item in parEvaluationXDepartmentXCargo.ParEvaluationSchedule)
                            {
                                item.ParEvaluationXDepartmentXCargo_Id = parEvaluationXDepartamentoXCargoSalvo.Id;
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
                                //db.SaveChanges();
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

        private void SalvarEditarAgendamento(ParEvaluationXDepartmentXCargo parEvaluationXDepartmentXCargo, int? parEvaluationXDepartamentoXCargoSalvo_Id)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                foreach (var item in parEvaluationXDepartmentXCargo.ParEvaluationSchedule)
                {
                    if (!parEvaluationXDepartamentoXCargoSalvo_Id.HasValue)
                        item.ParEvaluationXDepartmentXCargo_Id = parEvaluationXDepartmentXCargo.Id;
                    else
                        item.ParEvaluationXDepartmentXCargo_Id = parEvaluationXDepartamentoXCargoSalvo_Id;

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
        }
    }
}

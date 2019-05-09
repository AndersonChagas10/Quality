using Dominio;
using SgqSystem.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/DepartmentXRotinaApi")]
    public class DepartmentXRotinaApiController : BaseApiController
    {
        [HttpPost]
        [Route("PostRotinaIntegracao")]
        public IHttpActionResult PostRotinaIntegracao(ParDepartmentXRotinaIntegracao parDepartmentXRotinaIntegracao)
        {
            if (parDepartmentXRotinaIntegracao.Id > 0)//Alter
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;

                    var parDepartmentXRotinaIntegracaoOld = db.ParDepartmentXRotinaIntegracao.Find(parDepartmentXRotinaIntegracao.Id);


                    parDepartmentXRotinaIntegracaoOld.NameRotina = parDepartmentXRotinaIntegracao.NameRotina;
                    parDepartmentXRotinaIntegracaoOld.ParDepartment_Id = parDepartmentXRotinaIntegracao.ParDepartment_Id;
                    parDepartmentXRotinaIntegracaoOld.RotinaIntegracao_Id = parDepartmentXRotinaIntegracao.RotinaIntegracao_Id;
                    parDepartmentXRotinaIntegracaoOld.IsActive = parDepartmentXRotinaIntegracao.IsActive;

                    parDepartmentXRotinaIntegracaoOld.AlterDate = DateTime.Now;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            else //Insert
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    parDepartmentXRotinaIntegracao.AddDate = DateTime.Now;

                    db.ParDepartmentXRotinaIntegracao.Add(parDepartmentXRotinaIntegracao);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

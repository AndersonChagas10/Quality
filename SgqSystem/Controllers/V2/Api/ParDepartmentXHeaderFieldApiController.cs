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
    [RoutePrefix("api/ParDepartmentXHeaderFieldApi")]
    public class ParDepartmentXHeaderFieldApiController : BaseApiController
    {
        [HttpPost]
        [Route("PostParDepartmentxHeaderField")]
        public IHttpActionResult PostParDepartmentxHeaderField(ParHeaderFieldGeral saveParHeaderFieldGeral)
        {

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

                        if(parHeaderFieldGeral.ParLevelHeaderField_Id != 4)
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
    }
}

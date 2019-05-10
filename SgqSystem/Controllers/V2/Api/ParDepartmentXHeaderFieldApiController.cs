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
        public IHttpActionResult PostParDepartmentxHeaderField(ParDepartmentXHeaderField parDepartmentXHeaderField)
        {
            if (parDepartmentXHeaderField.ParHeaderField.Id > 0)
            {
                SaveOrUpdateParHeaderField(parDepartmentXHeaderField.ParHeaderField);
                SaveOrUpdateParDepartmentXHeaderField(parDepartmentXHeaderField, parDepartmentXHeaderField.ParHeaderField.Id, parDepartmentXHeaderField.ParDepartment_Id);
            }
            else
            {
                SaveOrUpdateParHeaderField(parDepartmentXHeaderField.ParHeaderField);
                SaveOrUpdateParDepartmentXHeaderField(null, parDepartmentXHeaderField.ParHeaderField.Id, parDepartmentXHeaderField.ParDepartment_Id);
            }
            SaveOrUpdateParMultipleValues(parDepartmentXHeaderField.ParHeaderField);

            return StatusCode(HttpStatusCode.NoContent);

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
                        parHeaderField.Description = parHeaderField.Description ?? "";
                        parHeaderField.IsActive = true;
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

        private bool SaveOrUpdateParDepartmentXHeaderField(ParDepartmentXHeaderField pardepartmentXHeaderField, int parHeaderField_Id, int parDepartment_Id)
        {

            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;

                    if (pardepartmentXHeaderField != null && pardepartmentXHeaderField.Id > 0) //Update ou Inactive
                    {

                        //Update todos para isActive = false
                        var parDepartmentXHeaderFieldToModfy = db.ParDepartmentXHeaderField.Find(pardepartmentXHeaderField.Id);

                        parDepartmentXHeaderFieldToModfy.IsActive = pardepartmentXHeaderField.IsActive;
                        parDepartmentXHeaderFieldToModfy.ParHeaderField_Id = pardepartmentXHeaderField.ParHeaderField_Id;
                        parDepartmentXHeaderFieldToModfy.IsRequired = pardepartmentXHeaderField.IsRequired;
                        parDepartmentXHeaderFieldToModfy.DefaultSelected = pardepartmentXHeaderField.DefaultSelected;
                        parDepartmentXHeaderFieldToModfy.HeaderFieldGroup = pardepartmentXHeaderField.HeaderFieldGroup;
                        parDepartmentXHeaderFieldToModfy.AlterDate = DateTime.Now;

                    }
                    else //Insert
                    {
                        pardepartmentXHeaderField = new ParDepartmentXHeaderField()
                        {
                            AddDate = DateTime.Now,
                            IsActive = true,
                            ParHeaderField_Id = parHeaderField_Id,
                            ParDepartment_Id = parDepartment_Id,
                        };

                        db.ParDepartmentXHeaderField.Add(pardepartmentXHeaderField);

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

        private bool SaveOrUpdateParMultipleValues(ParHeaderField parHeaderField)
        {
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                try
                {
                    if (parHeaderField.ParMultipleValues.Count > 0)
                    {
                        foreach (var parMultipleValue in parHeaderField.ParMultipleValues)
                        {
                            if (parMultipleValue.Id > 0)
                            {
                                //db.Configuration.LazyLoadingEnabled = false;
                                //var parMultipleValueToUpdate = db.ParMultipleValues.Find(parHeaderField.Id);
                                //parMultipleValueToUpdate.Name = parMultipleValue.Name;
                                //parMultipleValueToUpdate.Description = parMultipleValue.Description;
                                //parMultipleValueToUpdate.IsActive = parMultipleValue.IsActive;
                                //parMultipleValueToUpdate.AlterDate = DateTime.Now;
                                //parMultipleValueToUpdate.IsDefaultOption = parMultipleValue.IsDefaultOption;
                                //parMultipleValueToUpdate.ParHeaderField_Id = parHeaderField.Id;
                                //parMultipleValueToUpdate.PunishmentValue = parMultipleValue.PunishmentValue;
                                parMultipleValue.ParHeaderField = null;
                                db.Entry(parMultipleValue).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                parMultipleValue.ParHeaderField_Id = parHeaderField.Id;
                                parMultipleValue.AddDate = DateTime.Now;
                                parMultipleValue.Description = "";
                                db.ParMultipleValues.Add(parMultipleValue);
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

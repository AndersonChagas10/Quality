﻿using Dominio;
using Dominio.Interfaces.Repositories;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO.Params;

namespace Data.Repositories
{
    public class ParamsRepository : IParamsRepository
    {

        #region Construtor

        /// <summary>
        /// Instancia do DataBase.
        /// </summary>
        protected readonly SgqDbDevEntities db;

        /// <summary>
        /// Construtor
        /// </summary>
        public ParamsRepository(SgqDbDevEntities Db)
        {
            db = Db;
        }

        #endregion

        #region ParLevel1 ParHeadField ParLevel1XCluster ParMultipleValue

        public void SaveParLevel1(ParLevel1 paramLevel1, List<ParHeaderField> listaParHeadField, List<ParLevel1XCluster> listaParLevel1XCluster, List<int> removerHeadField, List<int> removerCluster)
        {
            using (var ts = db.Database.BeginTransaction())
            {
                SalvaParLevel1(paramLevel1); /*Salva paramLevel1*/

                if (listaParLevel1XCluster != null)
                {
                    foreach (var Level1XCluster in listaParLevel1XCluster)
                        SalvaLevel1XCluster(Level1XCluster, paramLevel1.Id); /*Salva NxN Level1XCluster*/
                }

                if (listaParHeadField != null)
                {
                    foreach (var parHeadField in listaParHeadField)
                    {
                        var ListParMultipleValues = parHeadField.ParMultipleValues; /*Variavel temporaria*/
                        SalvaParHeadField(parHeadField); /*Salva ParHeadField*/

                        foreach (var parMultipleValues in ListParMultipleValues)
                            SalvaParMultipleValues(parMultipleValues, parHeadField.Id);

                        int idParLevel1HeaderField;/*Verifica se ja existe vinculo ParLevel1 e ParHEaderField na tabela NxN ParLevel1XHeaderField. */
                        ParLevel1XHeaderField parLevel1HeaderField;
                        CriaParLevel1HeaderField(paramLevel1, parHeadField, out idParLevel1HeaderField, out parLevel1HeaderField);
                        SalvaParLevel1HeaderField(idParLevel1HeaderField, parLevel1HeaderField);/*Salva ParLevel1XHeaderField*/
                    }

                }

                InativaCluster(paramLevel1, removerCluster);

                InativaHeadField(paramLevel1, removerHeadField);

                ts.Commit();
            }
        }

        private void InativaHeadField(ParLevel1 paramLevel1, List<int> removerHeadField)
        {
            if (removerHeadField != null)
            {
                if (removerHeadField.Count > 0)
                {
                    foreach (var i in removerHeadField)
                    {
                        var objetos = db.ParLevel1XHeaderField.Where(r => r.ParHeaderField_Id == i && r.ParLevel1_Id == paramLevel1.Id);

                        foreach (var marcarObjetoInativo in objetos)
                        {
                            marcarObjetoInativo.IsActive = false;
                            Guard.verifyDate(marcarObjetoInativo, "AlterDate");
                            db.ParLevel1XHeaderField.Attach(marcarObjetoInativo);
                            db.Entry(marcarObjetoInativo).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
            }
        }

        private void InativaCluster(ParLevel1 paramLevel1, List<int> removerCluster)
        {
            if (removerCluster != null)
            {
                if (removerCluster.Count > 0)
                {
                    foreach (var i in removerCluster)
                    {
                        var objetos = db.ParLevel1XCluster.Where(r => r.ParCluster_Id == i && r.ParLevel1_Id == paramLevel1.Id);

                        foreach (var marcarObjetoInativo in objetos)
                        {
                            marcarObjetoInativo.IsActive = false;
                            Guard.verifyDate(marcarObjetoInativo, "AlterDate");
                            db.ParLevel1XCluster.Attach(marcarObjetoInativo);
                            db.Entry(marcarObjetoInativo).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
            }
        }

        private void SalvaParLevel1(ParLevel1 paramLevel1)
        {
            if (paramLevel1.Id == 0)
            {
                db.ParLevel1.Add(paramLevel1);
            }
            else
            {
                Guard.verifyDate(paramLevel1, "AlterDate");
                db.ParLevel1.Attach(paramLevel1);
                db.Entry(paramLevel1).State = EntityState.Modified;
            }
            db.SaveChanges(); //Obtem Id do paramLevel1
        }

        private void SalvaParMultipleValues(ParMultipleValues parMultipleValues, int parHeadFieldId)
        {
            if (parMultipleValues.Description == null)
                parMultipleValues.Description = "";
            parMultipleValues.ParHeaderField_Id = parHeadFieldId;
            parMultipleValues.ParHeaderField = null;
            if (parMultipleValues.Id == 0)
            {
                db.ParMultipleValues.Add(parMultipleValues);
            }
            else
            {
                Guard.verifyDate(parMultipleValues, "AlterDate");
                db.ParMultipleValues.Attach(parMultipleValues);
                db.Entry(parMultipleValues).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        private void CriaParLevel1HeaderField(ParLevel1 paramLevel1, ParHeaderField parHeadField, out int idParLevel1HeaderField, out ParLevel1XHeaderField parLevel1HeaderField)
        {
            idParLevel1HeaderField = 0;
            var verificaSeJaExisteVinculo = db.ParLevel1XHeaderField.FirstOrDefault(r => r.ParHeaderField_Id == parHeadField.Id && r.ParLevel1_Id == paramLevel1.Id);
            if (verificaSeJaExisteVinculo != null)
                idParLevel1HeaderField = verificaSeJaExisteVinculo.Id;

            parLevel1HeaderField = new ParLevel1XHeaderField()
            {
                Id = idParLevel1HeaderField,
                ParLevel1_Id = paramLevel1.Id,
                ParHeaderField_Id = parHeadField.Id,
                IsActive = true
            };
        }

        private void SalvaParLevel1HeaderField(int idParLevel1HeaderField, ParLevel1XHeaderField parLevel1HeaderField)
        {
            if (idParLevel1HeaderField == 0)
            {
                db.ParLevel1XHeaderField.Add(parLevel1HeaderField);
            }
            else
            {
                Guard.verifyDate(parLevel1HeaderField, "AlterDate");
                db.ParLevel1XHeaderField.Attach(parLevel1HeaderField);
                db.Entry(parLevel1HeaderField).State = EntityState.Modified;
            }
            db.SaveChanges(); /*Obtem id do ParLevel1XHeaderField*/
        }

        private void SalvaParHeadField(ParHeaderField parHeadField)
        {
            if (parHeadField.Description == null)
                parHeadField.Description = "";
            parHeadField.ParMultipleValues = null;
            parHeadField.ParFieldType = null;
            parHeadField.ParLevelDefiniton = null;
            parHeadField.ParLevel1XHeaderField = null;

            if (parHeadField.Id == 0)
            {
                db.ParHeaderField.Add(parHeadField);
            }
            else
            {
                Guard.verifyDate(parHeadField, "AlterDate");
                db.ParHeaderField.Attach(parHeadField);
                db.Entry(parHeadField).State = EntityState.Modified;
            }
            db.SaveChanges(); /*Obtem id do ParHeadField*/
        }

        private void SalvaLevel1XCluster(ParLevel1XCluster Level1XCluster, int paramLevel1Id)
        {
            Level1XCluster.ParLevel1_Id = paramLevel1Id;
            Level1XCluster.ParLevel1 = null;
            Level1XCluster.ParCluster = null;

            if (Level1XCluster.Id == 0)
            {
                db.ParLevel1XCluster.Add(Level1XCluster);
            }
            else
            {
                Guard.verifyDate(Level1XCluster, "AlterDate");
                db.ParLevel1XCluster.Attach(Level1XCluster);
                db.Entry(Level1XCluster).State = EntityState.Modified;
            }
            db.SaveChanges(); /*Obtem ID do NxN Level1XCluster*/
        }

        #endregion

        public void AddUpdateParLevel2(ParLevel2 paramLevel2)
        {
            if (paramLevel2.Id == 0)
            {
                db.ParLevel2.Add(paramLevel2);
            }
            else
            {
                Guard.verifyDate(paramLevel2, "AlterDate");
                db.ParLevel2.Attach(paramLevel2);
                db.Entry(paramLevel2).State = EntityState.Modified;
            }
        }

        public void SaveParLevel2(ParLevel2 saveParamLevel2, List<ParLevel3Group> listaParLevel3Group, List<ParCounterXLocal> listParCounterXLocal)
        {
            using (var ts = db.Database.BeginTransaction())
            {
                AddUpdateParLevel2(saveParamLevel2); /*Salva paramLevel1*/
                db.SaveChanges(); //Obtem Id do paramLevel1

                foreach (var Level3Group in listaParLevel3Group)
                {
                    AddUpdateParLevel3Group(Level3Group, saveParamLevel2.Id); /*Salva NxN Level1XCluster*/
                    db.SaveChanges(); /*Obtem ID do NxN Level1XCluster*/
                }

                foreach (var ParCounterXLocal in listParCounterXLocal)
                {
                    AddUpdateParCounterXLocal(ParCounterXLocal, saveParamLevel2.Id); /*Salva NxN Level1XCluster*/
                    db.SaveChanges(); /*Obtem ID do NxN Level1XCluster*/
                }
                ts.Commit();
            }
        }

        private void AddUpdateParCounterXLocal(ParCounterXLocal parCounterXLocal, int ParLevel2_Id)
        {
            parCounterXLocal.ParLevel2_Id = ParLevel2_Id;
            if (parCounterXLocal.Id == 0)
            {
                db.ParCounterXLocal.Add(parCounterXLocal);
            }
            else
            {
                Guard.verifyDate(parCounterXLocal, "AlterDate");
                db.ParCounterXLocal.Attach(parCounterXLocal);
                db.Entry(parCounterXLocal).State = EntityState.Modified;
            }
        }

        private void AddUpdateParLevel3Group(ParLevel3Group paramLevel3Group, int ParLevel2_Id)
        {
            paramLevel3Group.ParLevel2_Id = ParLevel2_Id;
            //if(paramLevel3Group.Description == null)
            //{
            //    paramLevel3Group.Description = string.Empty;
            //}
            if (paramLevel3Group.Id == 0)
            {
                db.ParLevel3Group.Add(paramLevel3Group);
            }
            else
            {
                Guard.verifyDate(paramLevel3Group, "AlterDate");
                db.ParLevel3Group.Attach(paramLevel3Group);
                db.Entry(paramLevel3Group).State = EntityState.Modified;
            }
        }

        public void SaveParLocal(ParLocal paramLocal)
        {
            if (paramLocal.Id == 0)
            {
                db.ParLocal.Add(paramLocal);
            }
            else
            {
                Guard.verifyDate(paramLocal, "AlterDate");
                db.ParLocal.Attach(paramLocal);
                db.Entry(paramLocal).State = EntityState.Modified;
            }
        }

        public void SaveParCounter(ParCounter paramCounter)
        {
            if (paramCounter.Id == 0)
            {
                db.ParCounter.Add(paramCounter);
            }
            else
            {
                Guard.verifyDate(paramCounter, "AlterDate");
                db.ParCounter.Attach(paramCounter);
                db.Entry(paramCounter).State = EntityState.Modified;
            }
        }

        public void SaveParCounterXLocal(ParCounterXLocal paramCounterXLocal)
        {
            if (paramCounterXLocal.Id == 0)
            {
                db.ParCounterXLocal.Add(paramCounterXLocal);
            }
            else
            {
                Guard.verifyDate(paramCounterXLocal, "AlterDate");
                db.ParCounterXLocal.Attach(paramCounterXLocal);
                db.Entry(paramCounterXLocal).State = EntityState.Modified;
            }
        }

        public void SaveParRelapse(ParRelapse paramRelapse)
        {
            if (paramRelapse.Id == 0)
            {
                db.ParRelapse.Add(paramRelapse);
            }
            else
            {
                Guard.verifyDate(paramRelapse, "AlterDate");
                db.ParRelapse.Attach(paramRelapse);
                db.Entry(paramRelapse).State = EntityState.Modified;
            }
        }

        public void SaveParNotConformityRule(ParNotConformityRule paramNotConformityRule)
        {
            if (paramNotConformityRule.Id == 0)
            {
                db.ParNotConformityRule.Add(paramNotConformityRule);
            }
            else
            {
                Guard.verifyDate(paramNotConformityRule, "AlterDate");
                db.ParNotConformityRule.Attach(paramNotConformityRule);
                db.Entry(paramNotConformityRule).State = EntityState.Modified;
            }
        }

        public void SaveParNotConformityRuleXLevel(ParNotConformityRuleXLevel paramNotConformityRuleXLevel)
        {
            if (paramNotConformityRuleXLevel.Id == 0)
            {
                db.ParNotConformityRuleXLevel.Add(paramNotConformityRuleXLevel);
            }
            else
            {
                Guard.verifyDate(paramNotConformityRuleXLevel, "AlterDate");
                db.ParNotConformityRuleXLevel.Attach(paramNotConformityRuleXLevel);
                db.Entry(paramNotConformityRuleXLevel).State = EntityState.Modified;
            }
        }


        public void SaveParCompany(ParCompany paramCompany)
        {

            if (paramCompany.Id == 0)
            {
                db.ParCompany.Add(paramCompany);
            }
            else
            {
                Guard.verifyDate(paramCompany, "AlterDate");
                db.ParCompany.Attach(paramCompany);
                db.Entry(paramCompany).State = EntityState.Modified;
            }
        }

      

        #region Não implementado

        //private DbSet<ParLevel1> EntityParLevel1 { get { return db.Set<ParLevel1>(); } }
        //private void SaveOrUpdate<T>(T obj, DbSet context)
        //{
        //    if (obj.GetType().GetProperty("Id") != null)
        //    {
        //        var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
        //        if (id > 0)
        //            Update(obj, context);
        //        else
        //            Add(obj, context);
        //    }
        //    db.SaveChanges();
        //}

        //public void Update<T>(T obj, DbSet context)
        //{
        //    Guard.verifyDate(obj, "AlterDate");
        //    context.Attach(obj);
        //    db.Entry<ParLevel1>(obj).State = EntityState.Modified;
        //}

        //public void Add<T>(T obj, DbSet context)
        //{
        //    Guard.verifyDate(obj, "AddDate");
        //    context.Add(obj);
        //} 

        #endregion
    }
}

using Dominio;
using Dominio.Interfaces.Repositories;
using DTO.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;
using System.Data;

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

        #region ParLevel1

        public void SaveParLevel1(ParLevel1 paramLevel1, List<ParHeaderField> listaParHeadField, List<ParLevel1XCluster> listaParLevel1XCluster, List<int> removerHeadField, List<ParCounterXLocal> listaParCounterLocal, List<ParNotConformityRuleXLevel> listNonCoformitRule, List<ParRelapse> reincidencia, List<ParGoal> listParGoal)
        {
            using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {

                SalvaParLevel1(paramLevel1); /*Salva paramLevel1*/

                //Clusters
                if (listaParLevel1XCluster != null)
                    foreach (var Level1XCluster in listaParLevel1XCluster)
                        SalvaLevel1XCluster(Level1XCluster, paramLevel1.Id);

                //Meta
                if (listParGoal != null)
                    foreach (var goal in listParGoal)
                        SalvaGoal(goal, paramLevel1.Id);

                //Counters
                if (listaParCounterLocal != null)
                    foreach (var counterDoLevel1 in listaParCounterLocal)
                        SalvaCounterLocalDoLevel1(paramLevel1, counterDoLevel1);

                //Alerta (Regra de NC)
                if (listNonCoformitRule != null)
                    foreach (var nonCoformitRule in listNonCoformitRule)
                        SaveNonConformityRule(nonCoformitRule, paramLevel1.Id);

                //Reincidencia
                if (reincidencia != null)
                    foreach (var parRelapse in reincidencia)
                        SaveReincidencia(parRelapse, paramLevel1.Id);

                //Header Fields
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

                InativaHeadField(paramLevel1, removerHeadField);

                ts.Commit();
            }
        }

        #region Auxiliares do level1

        private void SaveNonConformityRule(ParNotConformityRuleXLevel nonCoformitRule, int parLevel1Id)
        {
            if (nonCoformitRule.ParNotConformityRule_Id >= 0)
            {
                //MOCK
                nonCoformitRule.ParCompany_Id = 1;

                nonCoformitRule.ParLevel1_Id = parLevel1Id;

                if (nonCoformitRule.Id == 0)
                {
                    db.ParNotConformityRuleXLevel.Add(nonCoformitRule);
                }
                else
                {
                    Guard.verifyDate(nonCoformitRule, "AlterDate");
                    db.ParNotConformityRuleXLevel.Attach(nonCoformitRule);

                    if (nonCoformitRule.ParCompany_Id == 0)
                        nonCoformitRule.ParCompany_Id = null;

                    db.Entry(nonCoformitRule).State = EntityState.Modified;
                    db.Entry(nonCoformitRule).Property(e => e.AddDate).IsModified = false;
                }
                db.SaveChanges();
            }
        }


        private void SalvaCounterLocalDoLevel1(ParLevel1 paramLevel1, ParCounterXLocal counterDoLevel1)
        {
            counterDoLevel1.ParLevel1_Id = paramLevel1.Id;

            if (counterDoLevel1.Id == 0)
            {
                db.ParCounterXLocal.Add(counterDoLevel1);
            }
            else
            {
                Guard.verifyDate(counterDoLevel1, "AlterDate");
                var entry = db.ParCounterXLocal.Attach(counterDoLevel1);
                db.Entry(counterDoLevel1).State = EntityState.Modified;
                db.Entry(counterDoLevel1).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges();
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
                db.Entry(paramLevel1).Property(e => e.AddDate).IsModified = false;
                //if(paramLevel1.ParScoreType_Id.GetValueOrDefault() <= 0)
                //    db.Entry(paramLevel1).Property(e => e.ParScoreType_Id).IsModified = false;

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
                db.Entry(parMultipleValues).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(parLevel1HeaderField).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(parHeadField).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(Level1XCluster).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges(); /*Obtem ID do NxN Level1XCluster*/
        }

        private void SalvaGoal(ParGoal goal, int paramLevel1Id)
        {
            if (paramLevel1Id > 0)
                goal.ParLevel1_Id = paramLevel1Id;

            if (goal.ParCompany_Id == -1 || goal.ParCompany_Id == 0)
                goal.ParCompany_Id = null;

            goal.ParLevel1 = null;
            goal.ParCompany = null;

            if (goal.Id == 0)
            {
                db.ParGoal.Add(goal);
            }
            else
            {
                Guard.verifyDate(goal, "AlterDate");
                db.ParGoal.Attach(goal);
                db.Entry(goal).State = EntityState.Modified;
                db.Entry(goal).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges();
        }

        private void SaveReincidencia(ParRelapse parRelapse, int parLevel1Id)
        {
            parRelapse.ParLevel1_Id = parLevel1Id;

            if (parRelapse.Id == 0)
            {
                db.ParRelapse.Add(parRelapse);
            }
            else
            {
                Guard.verifyDate(parRelapse, "AlterDate");
                db.ParRelapse.Attach(parRelapse);
                db.Entry(parRelapse).State = EntityState.Modified;
                db.Entry(parRelapse).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges();
        }

        #endregion

        #endregion

        #region ParLevel2

        public void SaveParLevel2(ParLevel2 paramLevel2, List<ParLevel3Group> listaParLevel3Group, List<ParCounterXLocal> listParCounterXLocal, List<ParNotConformityRuleXLevel> listParamNotConformityRuleXLevel, List<ParEvaluation> listParamEvaluation, List<ParSample> listParamSample, List<ParRelapse> listParRelapse)
        {
            using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                AddUpdateParLevel2(paramLevel2); /*Salva paramLevel1*/
                db.SaveChanges(); //Obtem Id do paramLevel1

                if (listParamNotConformityRuleXLevel != null)
                    foreach (var paramNotConformityRuleXLevel in listParamNotConformityRuleXLevel)
                        AddUpdateParNotConformityRuleXLevel(paramNotConformityRuleXLevel, 2, ParLevel2_Id: paramLevel2.Id);

                if (listParamEvaluation.IsNotNull())
                    foreach (var paramEvaluation in listParamEvaluation)
                        AddUpdateParEvaluation(paramEvaluation, paramLevel2.Id);

                if (listParamSample.IsNotNull())
                    foreach (var paramSample in listParamSample)
                        AddUpdateParSample(paramSample, paramLevel2.Id);

                if (listaParLevel3Group != null)
                    foreach (var Level3Group in listaParLevel3Group)
                        AddUpdateParLevel3Group(Level3Group, paramLevel2.Id); /*Salva NxN Level1XCluster*/

                if (listParCounterXLocal != null)
                    foreach (var ParCounterXLocal in listParCounterXLocal)
                        AddUpdateParCounterXLocal(ParCounterXLocal, paramLevel2.Id); /*Salva NxN Level1XCluster*/

                if (listParRelapse != null)
                    foreach (var ParRelapse in listParRelapse)
                        AddUpdateParRelapse(ParRelapse, paramLevel2.Id); /*Salva NxN Level1XCluster*/

                db.SaveChanges();
                ts.Commit();
            }
        }

        #region Auxiliares ParLevel2

        public void AddUpdateParLevel2(ParLevel2 paramLevel2)
        {
            //Mock
            if (paramLevel2.ParDepartment_Id == null)
            {
                paramLevel2.ParDepartment_Id = 1;
            }
            if (paramLevel2.Id == 0)
            {
                db.ParLevel2.Add(paramLevel2);
            }
            else
            {
                Guard.verifyDate(paramLevel2, "AlterDate");
                db.ParLevel2.Attach(paramLevel2);
                db.Entry(paramLevel2).State = EntityState.Modified;
                db.Entry(paramLevel2).Property(e => e.AddDate).IsModified = false;
            }
        }

        private void AddUpdateParLevel3Group(ParLevel3Group paramLevel3Group, int ParLevel2_Id)
        {
            paramLevel3Group.ParLevel2_Id = ParLevel2_Id;
            if (paramLevel3Group.Id == 0)
            {
                db.ParLevel3Group.Add(paramLevel3Group);
            }
            else
            {
                paramLevel3Group.ParLevel2 = null;
                paramLevel3Group.ParLevel3Level2 = null;
                Guard.verifyDate(paramLevel3Group, "AlterDate");
                db.ParLevel3Group.Attach(paramLevel3Group);
                db.Entry(paramLevel3Group).State = EntityState.Modified;
                db.Entry(paramLevel3Group).Property(e => e.AddDate).IsModified = false;
            }
        }

        private void AddUpdateParNotConformityRuleXLevel(ParNotConformityRuleXLevel paramNotConformityRuleXLevel, int Level, int? ParLevel1_Id = null, int? ParLevel2_Id = null, int? ParLevel3_Id = null)
        {
            //if (ParLevel1_Id == null && ParLevel2_Id == null && ParLevel3_Id == null)
            //{
            //    throw new Exception("É necessário Informar O Id do Level1 ou Level2 ou Level3");
            //}
            //paramNotConformityRuleXLevel.Level = Level;
            //paramNotConformityRuleXLevel.ParLevel2_Id = ParLevel2_Id;

            //paramNotConformityRuleXLevel.ParLevel1_Id = ParLevel1_Id;
            paramNotConformityRuleXLevel.ParLevel2_Id = ParLevel2_Id;
            //paramNotConformityRuleXLevel.ParLevel3_Id = ParLevel3_Id;

            //MOCK
            paramNotConformityRuleXLevel.ParCompany_Id = 1;
            if (paramNotConformityRuleXLevel.Id == 0)
            {
                db.ParNotConformityRuleXLevel.Add(paramNotConformityRuleXLevel);
            }
            else
            {
                Guard.verifyDate(paramNotConformityRuleXLevel, "AlterDate");
                db.ParNotConformityRuleXLevel.Attach(paramNotConformityRuleXLevel);
                db.Entry(paramNotConformityRuleXLevel).State = EntityState.Modified;
                db.Entry(paramNotConformityRuleXLevel).Property(e => e.AddDate).IsModified = false;
            }
        }

        private void AddUpdateParEvaluation(ParEvaluation parEvaluation, int ParLevel2_Id)
        {
            parEvaluation.ParLevel2_Id = ParLevel2_Id;

            //var todasUnidades = db.ParEvaluation.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel2_Id == ParLevel2_Id);
            //if (todasUnidades != null)
            //    if (parEvaluation.Number == todasUnidades.Number)
            //        return;

            if (parEvaluation.Id == 0)
            {
                db.ParEvaluation.Add(parEvaluation);
            }
            else
            {
                Guard.verifyDate(parEvaluation, "AlterDate");
                db.ParEvaluation.Attach(parEvaluation);
                db.Entry(parEvaluation).State = EntityState.Modified;
                db.Entry(parEvaluation).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges();
        }

        private void AddUpdateParSample(ParSample parSample, int ParLevel2_Id)
        {
            parSample.ParLevel2_Id = ParLevel2_Id;

            //var todasUnidades = db.ParSample.FirstOrDefault(r => r.ParCompany_Id == null && r.ParLevel2_Id == ParLevel2_Id);
            //if (todasUnidades != null)
            //    if (parSample.Number == todasUnidades.Number)
            //        return;

            if (parSample.Id == 0)
            {
                db.ParSample.Add(parSample);
            }
            else
            {
                Guard.verifyDate(parSample, "AlterDate");
                db.ParSample.Attach(parSample);
                db.Entry(parSample).State = EntityState.Modified;
                //db.Entry(parSample).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges();
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
                db.Entry(parCounterXLocal).Property(e => e.AddDate).IsModified = false;
            }
        }

        private void AddUpdateParRelapse(ParRelapse parRelapse, int ParLevel2_Id)
        {
            parRelapse.ParLevel2_Id = ParLevel2_Id;
            if (parRelapse.Id == 0)
            {
                db.ParRelapse.Add(parRelapse);
            }
            else
            {
                Guard.verifyDate(parRelapse, "AlterDate");
                db.ParRelapse.Attach(parRelapse);
                db.Entry(parRelapse).State = EntityState.Modified;
                db.Entry(parRelapse).Property(e => e.AddDate).IsModified = false;
            }
        }

        #endregion

        #endregion

        #region ParLevel3

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramLevel3"></param>
        /// <param name="paramLevel3Value"></param>
        public void SaveParLevel3(ParLevel3 paramLevel3, List<ParLevel3Value> listParamLevel3Value, List<ParRelapse> listParRelapse, List<ParLevel3Level2> parLevel3Level2pontos, int level1Id)
        {
            using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                AddUpdateParLevel3(paramLevel3); /*Salva paramLevel1*/
                db.SaveChanges(); //Obtem Id do paramLevel1

                if (listParamLevel3Value != null)
                    if (listParamLevel3Value.Count() > 0)
                        AddUpdateParLevel3Value(listParamLevel3Value, paramLevel3.Id);

                if (listParRelapse != null)
                    foreach (var parRelapse in listParRelapse)
                        SaveReincidenciaLevel3(parRelapse, paramLevel3.Id);

                if (parLevel3Level2pontos != null)
                    if (parLevel3Level2pontos.Count() > 0)
                        AddUpdateParLevel3Level2(parLevel3Level2pontos, level1Id);

                db.SaveChanges();
                ts.Commit();
            }
        }

        private void SaveReincidenciaLevel3(ParRelapse parRelapse, int ParLevel3_Id)
        {
            parRelapse.ParLevel3_Id = ParLevel3_Id;

            if (parRelapse.Id == 0)
            {
                db.ParRelapse.Add(parRelapse);
            }
            else
            {
                Guard.verifyDate(parRelapse, "AlterDate");
                db.ParRelapse.Attach(parRelapse);
                db.Entry(parRelapse).State = EntityState.Modified;
                db.Entry(parRelapse).Property(e => e.AddDate).IsModified = false;
            }
            db.SaveChanges();
        }

        public void AddUpdateParLevel3(ParLevel3 paramLevel3)
        {
            if (paramLevel3.Id == 0)
            {
                db.ParLevel3.Add(paramLevel3);
            }
            else
            {
                Guard.verifyDate(paramLevel3, "AlterDate");
                db.ParLevel3.Attach(paramLevel3);
                db.Entry(paramLevel3).State = EntityState.Modified;
                db.Entry(paramLevel3).Property(e => e.AddDate).IsModified = false;
            }
        }

        public void AddUpdateParLevel3Value(List<ParLevel3Value> paramLevel3Value, int ParLevel3_Id)
        {
            paramLevel3Value.ForEach(r => r.ParLevel3_Id = ParLevel3_Id);

            if (paramLevel3Value.Any(r => r.Id == 0))
            {
                db.ParLevel3Value.AddRange(paramLevel3Value.Where(r => r.Id == 0));
            }

            if (paramLevel3Value.Any(r => r.Id > 0))
            {
                foreach (var i in paramLevel3Value.Where(r => r.Id > 0))
                {
                    Guard.verifyDate(i, "AlterDate");
                    db.ParLevel3Value.Attach(i);
                    db.Entry(i).State = EntityState.Modified;
                    db.Entry(i).Property(e => e.AddDate).IsModified = false;
                }
            }

            db.SaveChanges();
        }

        private string VerificaNulo<T>(T prop)
        {
            return prop.IsNull() ? "null" : prop.ToString();
        }

        #endregion

        #region Global

        public void SaveVinculoL3L2L1(int idLevel1, int idLevel2, int idLevel3, int? userId, int? companyId)
        {
            using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ParLevel2Level1 existenteL2L1;
                ParLevel3Level2 existenteL3L2;
                ParLevel3Level2Level1 existenteL3L2L1;
                ParLevel3Level2 salvarL3L2;
                ParLevel2Level1 salvarL2L1;
                UserSgq user;

                if (userId > 0)
                {
                    user = db.UserSgq.FirstOrDefault(r => r.Id == userId);
                    if (user.Role == null || !user.Role.ToLowerInvariant().Contains("Admin".ToLowerInvariant()))
                        companyId = user.ParCompany_Id;

                }

                /**/
                existenteL2L1 = db.ParLevel2Level1.FirstOrDefault(r => r.ParLevel1_Id == idLevel1 && r.ParLevel2_Id == idLevel2 && r.ParCompany_Id == companyId);

                if (existenteL2L1 == null)
                {
                    salvarL2L1 = new ParLevel2Level1() { ParLevel1_Id = idLevel1, ParLevel2_Id = idLevel2, AddDate = DateTime.Now, IsActive = true, ParCompany_Id = companyId };
                    db.ParLevel2Level1.Add(salvarL2L1);
                    db.SaveChanges();
                }
                if (existenteL2L1.IsActive == false)
                {
                    existenteL2L1.IsActive = true;
                    var old = db.ParLevel2Level1.Find(existenteL2L1.Id);
                    var entry = db.Entry(existenteL2L1);
                    entry.CurrentValues.SetValues(existenteL2L1);
                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (idLevel3 > 0)
                {
                    /**/
                    var idL3L2 = 0;
                    existenteL3L2 = db.ParLevel3Level2.FirstOrDefault(r => r.ParLevel3_Id == idLevel3 && r.ParLevel2_Id == idLevel2 && r.ParCompany_Id == companyId);
                    if (existenteL3L2 == null)
                    {
                        //throw new Exception("");
                        salvarL3L2 = new ParLevel3Level2() { ParLevel2_Id = idLevel2, ParLevel3_Id = idLevel3, ParCompany_Id = companyId, IsActive = true, Weight = 1 };
                        db.ParLevel3Level2.Add(salvarL3L2);
                        db.SaveChanges();
                        idL3L2 = salvarL3L2.Id;
                    }
                    else
                    {
                        idL3L2 = existenteL3L2.Id;
                    }
                    if (existenteL3L2.IsActive == false)
                    {
                        existenteL3L2.IsActive = true;
                        var old = db.ParLevel3Level2.Find(existenteL3L2.Id);
                        var entry = db.Entry(existenteL3L2);
                        entry.CurrentValues.SetValues(existenteL3L2);
                        //db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }

                    /**/
                    existenteL3L2L1 = db.ParLevel3Level2Level1.FirstOrDefault(r => r.ParLevel1_Id == idLevel1 && r.ParLevel3Level2_Id == idL3L2 && r.ParCompany_Id == companyId);
                    if (existenteL3L2L1 == null)
                    {
                        var salvarL3L2L1 = new ParLevel3Level2Level1() { ParLevel1_Id = idLevel1, ParLevel3Level2_Id = idL3L2, ParCompany_Id = companyId, Active = true };
                        db.ParLevel3Level2Level1.Add(salvarL3L2L1);
                        db.SaveChanges();
                    }
                    if (existenteL3L2L1.Active == false)
                    {
                        existenteL3L2L1.Active = true;
                        var old = db.ParLevel3Level2Level1.Find(existenteL3L2L1.Id);
                        var entry = db.Entry(existenteL3L2L1);
                        entry.CurrentValues.SetValues(existenteL3L2L1);
                        //db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                    }
                    ts.Commit();

                }
            }


        }

        #endregion

        #region Inativadores

        private void InativaCounter(ParLevel1 paramLevel1, List<int> removeCounter)
        {
            if (removeCounter != null)
            {
                if (removeCounter.Count > 0)
                {
                    foreach (var idCounter in removeCounter)
                    {
                        var objetos = db.ParCounterXLocal.Where(r => r.Id == idCounter);

                        foreach (var marcarObjetoInativo in objetos)
                        {
                            marcarObjetoInativo.IsActive = false;
                            Guard.verifyDate(marcarObjetoInativo, "AlterDate");
                            db.ParCounterXLocal.Attach(marcarObjetoInativo);
                            db.Entry(marcarObjetoInativo).State = EntityState.Modified;
                            db.Entry(marcarObjetoInativo).Property(e => e.AddDate).IsModified = false;
                            db.SaveChanges();
                        }

                    }
                }
            }
        }

        private void InativaHeadField(ParLevel1 paramLevel1, List<int> removerHeadField)
        {
            if (removerHeadField != null)
            {
                if (removerHeadField.Count > 0)
                {
                    foreach (var idHeadField in removerHeadField)
                    {
                        var objetos = db.ParLevel1XHeaderField.Where(r => r.Id == idHeadField);

                        foreach (var marcarObjetoInativo in objetos)
                        {
                            marcarObjetoInativo.IsActive = false;
                            Guard.verifyDate(marcarObjetoInativo, "AlterDate");
                            db.ParLevel1XHeaderField.Attach(marcarObjetoInativo);
                            db.Entry(marcarObjetoInativo).State = EntityState.Modified;
                            db.Entry(marcarObjetoInativo).Property(e => e.AddDate).IsModified = false;
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
                    foreach (var idCluster in removerCluster)
                    {
                        var objetos = db.ParLevel1XCluster.Where(r => r.Id == idCluster);

                        foreach (var marcarObjetoInativo in objetos)
                        {
                            marcarObjetoInativo.IsActive = false;
                            Guard.verifyDate(marcarObjetoInativo, "AlterDate");
                            db.ParLevel1XCluster.Attach(marcarObjetoInativo);
                            db.Entry(marcarObjetoInativo).State = EntityState.Modified;
                            db.Entry(marcarObjetoInativo).Property(e => e.AddDate).IsModified = false;
                            db.SaveChanges();
                        }

                    }
                }
            }
        }

        private void InativaReincidencia(List<int> removeReincidencia)
        {
            if (removeReincidencia != null)
            {
                if (removeReincidencia.Count > 0)
                {
                    foreach (var idReincidencia in removeReincidencia)
                    {
                        var objetos = db.ParRelapse.Where(r => r.Id == idReincidencia);

                        foreach (var marcarObjetoInativo in objetos)
                        {
                            marcarObjetoInativo.IsActive = false;
                            Guard.verifyDate(marcarObjetoInativo, "AlterDate");
                            db.ParRelapse.Attach(marcarObjetoInativo);
                            db.Entry(marcarObjetoInativo).State = EntityState.Modified;
                            db.Entry(marcarObjetoInativo).Property(e => e.AddDate).IsModified = false;
                            db.SaveChanges();
                        }

                    }
                }
            }
        }

        #endregion

        public void RemoveParLevel03Group(int Id)
        {
            var parLevel3Group = db.ParLevel3Group.Where(r => r.Id == Id).FirstOrDefault();
            if (parLevel3Group != null)
            {
                parLevel3Group.IsActive = false;
                AddUpdateParLevel3Group(parLevel3Group, parLevel3Group.ParLevel2_Id);
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
                db.Entry(paramLocal).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(paramCounter).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(paramCounterXLocal).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(paramRelapse).Property(e => e.AddDate).IsModified = false;

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
                db.Entry(paramNotConformityRule).Property(e => e.AddDate).IsModified = false;
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
                db.Entry(paramNotConformityRuleXLevel).Property(e => e.AddDate).IsModified = false;
            }
        }


        #region ParLevel3Level2

        public void AddUpdateParLevel3Level2(List<ParLevel3Level2> paramParLevel3Level2, int? level1Id = null)
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            if (level1Id.IsNull())
                throw new Exception("é necessário selectionar um level1 antes de criar um novo vinculo de peso para o level3.");

            if (paramParLevel3Level2.Any(r => r.Id == 0))
            {
                db.ParLevel3Level2.AddRange(paramParLevel3Level2.Where(r => r.Id == 0));
                db.SaveChanges();
                foreach (var i in paramParLevel3Level2)
                {
                    db.ParLevel3Level2Level1.Add(new ParLevel3Level2Level1() { ParCompany_Id = i.ParCompany_Id, ParLevel1_Id = level1Id.GetValueOrDefault(), ParLevel3Level2_Id = i.Id, Active = true, AddDate = DateTime.Now });
                    db.ParLevel2Level1.Add(new ParLevel2Level1() { ParCompany_Id = i.ParCompany_Id, ParLevel2_Id = i.ParLevel2_Id, ParLevel1_Id = level1Id.GetValueOrDefault(), IsActive = true, AddDate = DateTime.Now });
                }
                db.SaveChanges();
            }
            else
            {
                foreach (var i in paramParLevel3Level2)
                {
                    Guard.verifyDate(i, "AlterDate");
                    db.ParLevel3Level2.Attach(i);
                    db.Entry(i).State = EntityState.Modified;
                    db.Entry(i).Property(e => e.AddDate).IsModified = false;

                    if (i.IsActive == false)
                    {
                        var inativarParLevel3Level2 = db.ParLevel3Level2.Include("ParLevel3Level2Level1").FirstOrDefault(r => r.Id == i.Id).ParLevel3Level2Level1.FirstOrDefault(r => r.ParCompany_Id == i.ParCompany_Id);
                        if (inativarParLevel3Level2.IsNotNull())
                        {
                            inativarParLevel3Level2.Active = false;
                            Guard.verifyDate(inativarParLevel3Level2, "AlterDate");
                            db.ParLevel3Level2Level1.Attach(inativarParLevel3Level2);
                            db.Entry(inativarParLevel3Level2).State = EntityState.Modified;
                            db.Entry(inativarParLevel3Level2).Property(e => e.AddDate).IsModified = false;

                            var inativarParLevel2Level1 = db.ParLevel2Level1.FirstOrDefault(r => r.ParCompany_Id == i.ParCompany_Id && r.ParLevel2_Id == i.ParLevel2_Id && r.ParLevel1_Id == inativarParLevel3Level2.ParLevel1_Id);
                            if (inativarParLevel2Level1.IsNotNull())
                            {
                                inativarParLevel2Level1.IsActive = false;
                                Guard.verifyDate(inativarParLevel2Level1, "AlterDate");
                                db.ParLevel2Level1.Attach(inativarParLevel2Level1);
                                db.Entry(inativarParLevel2Level1).State = EntityState.Modified;
                                db.Entry(inativarParLevel2Level1).Property(e => e.AddDate).IsModified = false;
                            }
                        }

                        db.SaveChanges();
                    }


                }
            }
            //}

            db.SaveChanges();

        }

        public void SaveParLevel3Level2(ParLevel3Level2 paramLevel3Level2)
        {
            using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                var save = new List<ParLevel3Level2>();
                save.Add(paramLevel3Level2);
                AddUpdateParLevel3Level2(save); /*Salva paramLevel1*/
                db.SaveChanges(); //Obtem Id do paramLevel1
                ts.Commit();
            }
        }

        public ParLevel2XHeaderField SaveParHeaderLevel2(ParLevel2XHeaderField parLevel2HeaderField)
        {
            using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                if (parLevel2HeaderField.Id == 0)
                {
                    parLevel2HeaderField = db.ParLevel2XHeaderField.Add(parLevel2HeaderField);
                }
                else
                {
                    parLevel2HeaderField.IsActive = false;
                    Guard.verifyDate(parLevel2HeaderField, "AlterDate");
                    db.ParLevel2XHeaderField.Attach(parLevel2HeaderField);
                    db.Entry(parLevel2HeaderField).State = EntityState.Modified;
                    db.Entry(parLevel2HeaderField).Property(e => e.AddDate).IsModified = false;
                }
                db.SaveChanges();
                ts.Commit();
            }
            return parLevel2HeaderField;
        }

        #endregion

        public void RemoveParLevel3Group(ParLevel3Group paramLevel03group)
        {
            Guard.verifyDate(paramLevel03group, "AlterDate");
            db.ParLevel3Group.Attach(paramLevel03group);
            db.Entry(paramLevel03group).State = EntityState.Modified;
            db.Entry(paramLevel03group).Property(e => e.AddDate).IsModified = false;

            db.SaveChanges();
        }

        public void ExecuteSql(string sql)
        {
            db.Database.ExecuteSqlCommand(sql);
        }

        public void SaveParCompany(ParCompany paramCompany)
        {
            throw new NotImplementedException();
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

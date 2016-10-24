﻿using Dominio;
using Dominio.Interfaces.Repositories;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ParamsRepository : IParamsRepository
    {

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

        public void SaveParLevel1(ParLevel1 paramLevel1, List<ParHeaderField> listaParHeadField, List<ParLevel1XCluster> listaParLevel1XCluster)
        {
            using (var ts = db.Database.BeginTransaction())
            {

                SalvaParLevel1(paramLevel1); /*Salva paramLevel1*/
                db.SaveChanges(); //Obtem Id do paramLevel1

                foreach (var Level1XCluster in listaParLevel1XCluster)
                {
                    SalvaLevel1XCluster(Level1XCluster, paramLevel1.Id); /*Salva NxN Level1XCluster*/
                    db.SaveChanges(); /*Obtem ID do NxN Level1XCluster*/
                }

                foreach (var parHeadField in listaParHeadField)
                {
                    var ListParMultipleValues = parHeadField.ParMultipleValues;
                    SalvaParHeadField(parHeadField); /*Salva ParHeadField*/
                    db.SaveChanges(); /*Obtem id do ParHeadField*/

                    foreach (var parMultipleValues in ListParMultipleValues)
                    {
                        SalvaParMultipleValues(parMultipleValues, parHeadField.Id);
                        db.SaveChanges();
                    }

                    int idParLevel1HeaderField;
                    ParLevel1HeaderField parLevel1HeaderField;
                    /*Verifica se ja existe vinculo ParLevel1 e ParHEaderField na tabela NxN ParLevel1XHeaderField. */
                    CriaParLevel1HeaderField(paramLevel1, parHeadField, out idParLevel1HeaderField, out parLevel1HeaderField);

                    SalvaParLevel1HeaderField(idParLevel1HeaderField, parLevel1HeaderField);/*Salva ParLevel1XHeaderField*/
                    db.SaveChanges(); /*Obtem id do ParLevel1XHeaderField*/
                }

                ts.Commit();
            }
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
        }

        private void CriaParLevel1HeaderField(ParLevel1 paramLevel1, ParHeaderField parHeadField, out int idParLevel1HeaderField, out ParLevel1HeaderField parLevel1HeaderField)
        {
            idParLevel1HeaderField = 0;
            var verificaSeJaExisteVinculo = db.ParLevel1HeaderField.FirstOrDefault(r => r.ParHeaderField_Id == parHeadField.Id && r.ParLevel1_Id == paramLevel1.Id);
            if (verificaSeJaExisteVinculo != null)
                idParLevel1HeaderField = verificaSeJaExisteVinculo.Id;

            parLevel1HeaderField = new ParLevel1HeaderField()
            {
                Id = idParLevel1HeaderField,
                ParLevel1_Id = paramLevel1.Id,
                ParHeaderField_Id = parHeadField.Id
            };
        }

        private void SalvaParLevel1HeaderField(int idParLevel1HeaderField, ParLevel1HeaderField parLevel1HeaderField)
        {
            if (idParLevel1HeaderField == 0)
            {
                db.ParLevel1HeaderField.Add(parLevel1HeaderField);
            }
            else
            {
                Guard.verifyDate(parLevel1HeaderField, "AlterDate");
                db.ParLevel1HeaderField.Attach(parLevel1HeaderField);
                db.Entry(parLevel1HeaderField).State = EntityState.Modified;
            }
        }

        private void SalvaParHeadField(ParHeaderField parHeadField)
        {
            if (parHeadField.Description == null)
                parHeadField.Description = "";
            parHeadField.ParMultipleValues = null;
            parHeadField.ParFieldType = null;
            parHeadField.ParLevelDefiniton = null;
            parHeadField.ParLevel1HeaderField = null;

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
        }

        private void SalvaLevel1XCluster(ParLevel1XCluster Level1XCluster, int paramLevel1Id)
        {
            Level1XCluster.ParLevel1_Id = paramLevel1Id;
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
        }

        public void SaveParLevel2(ParLevel2 paramLevel2)
        {
            if(paramLevel2.Id == 0)
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

        public void SaveParLevel2(ParLevel2 paramLevel2, List<ParDepartment> ListParDepartment, List<ParFrequency> listParFrequancy)
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

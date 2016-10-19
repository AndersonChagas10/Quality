using Dominio;
using Dominio.Interfaces.Repositories;
using DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Repositories
{
    public class SaveCollectionRepo : ISaveCollectionRepo
    {

        /// <summary>
        /// Instancia do DataBase.
        /// </summary>
        protected readonly SgqDbDevEntities db;

        /// <summary>
        /// Objeto T em memória volátil pela chamada de sua Interface.
        /// </summary>
        //private DbSet<T> Entity { get { return db.Set<T>(); } }

        public SaveCollectionRepo(SgqDbDevEntities Db)
        {
            db = Db;
        }

        public void SaveAllLevel(List<ConsolidationLevel01> listConsolidationLelve1
            , List<ConsolidationLevel02> listConsolidationLelve2
            , List<CollectionLevel02> listCollectionLelve2
            , List<CollectionLevel03> listCollectionLelve3
            , List<CorrectiveAction> listCorrectiveAction)
        {


            foreach (var consolidationLevel1 in listConsolidationLelve1)
            {

                InsereConsolidacaoLevel01(consolidationLevel1);

                foreach (var consolidationLevel02 in listConsolidationLelve2)
                {
                    InsereConsolidationLevel02(consolidationLevel02, consolidationLevel1.Id);
                    foreach (var collectionLevel02 in listCollectionLelve2.Where(r=>r.Level02Id == consolidationLevel02.Level02Id))
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                InserecollectionLevel02ECollectionLevel03(collectionLevel02, listCollectionLelve3, consolidationLevel02.Id);
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                new CreateLog(new Exception("Erro Ao salvar Level2 e Level03 Colelction", e), collectionLevel02);
                                transaction.Rollback();
                            }
                        }
                    }

                }


            }

           // return db.ConsolidationLevel01.FirstOrDefault(r => r.Id == listConsolidationLelve1[0].Id);
        }

        private void InserecollectionLevel02ECollectionLevel03(CollectionLevel02 collectionLevel02ToSave, List<CollectionLevel03> listCollectionLelve3, int consolidationLevel02Id)
        {
            //foreach (var collectionLevel02ToSave in listCollectionLelve2)
            //{
            var level03ToSave = collectionLevel02ToSave.CollectionLevel03;
            collectionLevel02ToSave.ConsolidationLevel02Id = consolidationLevel02Id;
            collectionLevel02ToSave.CollectionLevel03 = null;

            if (collectionLevel02ToSave.Id > 0)
            {
                verifyDate(collectionLevel02ToSave, "AlterDate");
                db.CollectionLevel02.Attach(collectionLevel02ToSave);
                db.Entry(collectionLevel02ToSave).State = EntityState.Modified;
            }
            else
            {
                db.CollectionLevel02.Add(collectionLevel02ToSave);
            }

            db.SaveChanges();

            foreach (var collectionLevel03InLevel02ToSave in level03ToSave)
            {

                collectionLevel03InLevel02ToSave.CollectionLevel02Id = collectionLevel02ToSave.Id;

                if (collectionLevel03InLevel02ToSave.Id > 0)
                {
                    verifyDate(collectionLevel03InLevel02ToSave, "AlterDate");
                    db.CollectionLevel03.Attach(collectionLevel03InLevel02ToSave);
                    db.Entry(collectionLevel03InLevel02ToSave).State = EntityState.Modified;
                }
                else
                {
                    db.CollectionLevel03.Add(collectionLevel03InLevel02ToSave);
                }
            }

            db.SaveChanges();
            // }
        }

        private void InsereConsolidationLevel02(ConsolidationLevel02 consolidationLevel02, int consolidationLevel01Id)
        {

            if (consolidationLevel02.Level01ConsolidationId == 0)
                consolidationLevel02.Level01ConsolidationId = consolidationLevel01Id;

            if (consolidationLevel02.Id > 0)
            {
                verifyDate(consolidationLevel02, "AlterDate");
                db.ConsolidationLevel02.Attach(consolidationLevel02);
                db.Entry(consolidationLevel02).State = EntityState.Modified;
                //db.ConsolidationLevel02.Add(consolidationLevel02);
            }
            else
            {
                db.ConsolidationLevel02.Add(consolidationLevel02);
            }

            db.SaveChanges();
        }

        private void InsereConsolidacaoLevel01(ConsolidationLevel01 consolidationLevel1)
        {
            if (consolidationLevel1.Id > 0)
            {
                verifyDate(consolidationLevel1, "AlterDate");
                db.ConsolidationLevel01.Attach(consolidationLevel1);
                db.Entry(consolidationLevel1).State = EntityState.Modified;
                //db.ConsolidationLevel01.Add(consolidationLevel1);
            }
            else
            {
                db.ConsolidationLevel01.Add(consolidationLevel1);
            }
            db.SaveChanges();
        }

        #region Auxiliares

        private void verifyDate<T>(T obj, string property)
        {
            try
            {
                if (obj.GetType().GetProperty(property) != null)
                {
                    var date = (DateTime)obj.GetType().GetProperty(property).GetValue(obj, null);
                    if (date.IsNull())
                    {
                        obj.GetType().GetProperty(property).SetValue(obj, DateTime.Now);
                    }
                    else
                    {
                        if (date == DateTime.MinValue)
                            obj.GetType().GetProperty(property).SetValue(obj, DateTime.Now);
                    }
                }
            }
            catch (Exception)
            {
                obj.GetType().GetProperty(property).SetValue(obj, DateTime.Now);
            }
        } 

        #endregion
    }
}

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCollectionLelve2">CollectionLevel02</param>
        /// <param name="listCollectionLelve3">CollectionLevel03 DA CollectionLevel02</param>
        /// <param name="listCorrectiveAction">CorrectiveAction da CollectionLevel02</param>
        public void SaveAllLevel(
             //List<ConsolidationLevel01> listConsolidationLelve1
             //, List<ConsolidationLevel02> listConsolidationLelve2
             List<CollectionLevel02> listCollectionLelve2
            , List<CollectionLevel03> listCollectionLelve3
            , List<CorrectiveAction> listCorrectiveAction)
        {

            foreach (var collectionLevel02 in listCollectionLelve2)
            {
                try
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            /**/
                            InserecollectionLevel02ECollectionLevel03(collectionLevel02, listCollectionLelve3);//listCollectionLelve3 DA CollectionLevel02
                            transaction.Commit();

                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            //transaction.Dispose();
                            throw e;
                        }
                    }
                }
                catch (Exception e)
                {
                    var copiaDaCollection = collectionLevel02;
                    var erro = "Erro Ao salvar Level2 e Level03 Colelction";
                    /**/
                    if (e.InnerException != null)
                    {
                        /*SE FOR EXCEPTION DE ELEMENTO INEXISTENTE NO DB*/
                        if (e.InnerException.Message.IndexOf("rows (0)") > 0)
                        {
                            /*Se possir ID no level02 remove o level02, e se este possuir level03 remove também*/
                            if (collectionLevel02.Id > 0)
                            {
                                try
                                {
                                    using (var db2 = new SgqDbDevEntities())
                                    {
                                        var result = db2.CollectionLevel02.FirstOrDefault(r => r.Id == collectionLevel02.Id);

                                        if (result != null)
                                        {
                                            /*Remove objetos encontrados no Banco de dados do level03.*/
                                            RemoveLevel3QueFoiEncontradoNoDbComLevel2(result);

                                            /*Remove objetos que foram enviados para inserção do level03 no Banco de dados.*/
                                            RemovLevel03QueFoiEnviadoComID(listCollectionLelve3, db2);

                                            /*Remove objeto do level02 encontrado no DB*/
                                            var queryLevel2 = string.Format("delete from CollectionLevel02 where id = " + result.Id);
                                            db.Database.ExecuteSqlCommand(queryLevel2);

                                            /*Remove corrective action*/
                                            RemoveCorrectiveAction(result);
                                            erro += "Removidos objetos Level02 e Level03, Level02 Existia em DB, provavel causa ausencia de um ou mais Collctionslevel03 da CollectionLEvel02 no DB";
                                        }
                                        else /*Caso seja o level02 que não exista em DB*/
                                        {
                                            /*Remove objetos que foram enviados para inserção do level03 no Banco de dados.*/
                                            RemovLevel03QueFoiEnviadoComID(listCollectionLelve3, db2);
                                            erro += "Removidos objetos Level03, não existiam Level02 até esta remoção, provavel causa ausencia do CollectionLevel02 no DB.";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    RemovePropVirtual(copiaDaCollection);
                                    new CreateLog(new Exception("Erro ao tentar excluir elemento INVALIDO ausente em DB: " + erro, ex), copiaDaCollection);
                                }
                            }
                        }
                    }

                    RemovePropVirtual(copiaDaCollection);
                    new CreateLog(new Exception(erro, e), copiaDaCollection);
                }
            }

        }

        private static void RemovePropVirtual(CollectionLevel02 collectionLevel02)
        {
            collectionLevel02.ConsolidationLevel02 = null;
            collectionLevel02.Level01 = null;
            collectionLevel02.Level02 = null;
            collectionLevel02.UserSgq = null;

            if (collectionLevel02.CorrectiveAction != null)
            {
                foreach (var i in collectionLevel02.CorrectiveAction)
                {
                    i.CollectionLevel02 = null;
                    i.UserSgq = null;
                    i.UserSgq2 = null;
                    i.UserSgq1 = null;
                }
            }

            if (collectionLevel02.CollectionLevel03 != null)
            {
                foreach (var i in collectionLevel02.CollectionLevel03)
                {
                    i.CollectionLevel02 = null;
                    i.Level03 = null;
                }
            }
        }

        private void RemoveCorrectiveAction(CollectionLevel02 result)
        {
            foreach (var ca in result.CorrectiveAction)
            {
                var queryCa = string.Format("delete from CollectionLevel02 where id = " + ca.Id);
                db.Database.ExecuteSqlCommand(queryCa);
            }
        }

        private void InsereCorrectiveAction(int collectionLevel02Id, CorrectiveAction correctiveAction)
        {

            correctiveAction.CollectionLevel02Id = collectionLevel02Id;

            if (correctiveAction.Id > 0)
            {
                verifyDate(correctiveAction, "AlterDate");
                db.CorrectiveAction.Attach(correctiveAction);
                db.Entry(correctiveAction).State = EntityState.Modified;
            }
            else
            {
                db.CorrectiveAction.Add(correctiveAction);
            }

            db.SaveChanges();

        }

        private void InserecollectionLevel02ECollectionLevel03(CollectionLevel02 collectionLevel02ToSave, List<CollectionLevel03> listCollectionLelve3)
        {
            /**/
            var haveCorrectiveAction = collectionLevel02ToSave.CorrectiveAction.FirstOrDefault();
            var level03ToSave = collectionLevel02ToSave.CollectionLevel03;

            collectionLevel02ToSave.CorrectiveAction = null;
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

            /**/
            if (haveCorrectiveAction != null)
                InsereCorrectiveAction(collectionLevel02ToSave.Id, haveCorrectiveAction);

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

        #region Remove

        /// <summary>
        /// Caso seja enviado um objeto collectionLevel02 com CollectionsLevel03 , e não exista CollectionLevel03 mais no DB,
        /// Este metodo procura TODAS AS COLLECTIONSLEVEL03 DA COLLECTIONLEVEL02 no banco de dados e as remove.
        /// </summary>
        /// <param name="result">CollectionLevel02 Existente</param>
        private void RemoveLevel3QueFoiEncontradoNoDbComLevel2(CollectionLevel02 result)
        {
            if (result.CollectionLevel03 != null)
            {
                foreach (var cl3Remove in result.CollectionLevel03)
                {
                    var queryLevel3 = string.Format("delete from CollectionLevel03 where id = " + cl3Remove.Id);
                    db.Database.ExecuteSqlCommand(queryLevel3);
                }
            }
        }

        /// <summary>
        /// Caso seja enviado um objeto collectionLevel02 com CollectionsLevel03 , e não exista CollectionLevel02 ou CollectionLevel03 mais no DB,
        /// Este metodo procura todos os IDS ENVIADOS COM A COLLECTIONLEVEL3, e se existirem, deleta seus registros.
        /// </summary>
        /// <param name="listCollectionLelve3"></param>
        /// <param name="db2"></param>
        private void RemovLevel03QueFoiEnviadoComID(List<CollectionLevel03> listCollectionLelve3, SgqDbDevEntities db2)
        {
            foreach (var ii in listCollectionLelve3)
            {
                if (ii.Id > 0)
                {
                    var resultLevel03 = db2.CollectionLevel03.FirstOrDefault(r => r.Id == ii.Id);
                    if (resultLevel03 != null)
                    {
                        var queryLevel3 = string.Format("delete from CollectionLevel03 where id = " + resultLevel03.Id);
                        db.Database.ExecuteSqlCommand(queryLevel3);
                    }
                }
            }
        }

        #endregion
    }
}

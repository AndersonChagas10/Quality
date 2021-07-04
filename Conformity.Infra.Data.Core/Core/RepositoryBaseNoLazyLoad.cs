using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Conformity.Domain.Core.Interfaces;

namespace Conformity.Infra.Data.Core.Core
{
    /// <summary>
    /// Repositório Base, classe de gerencia do Banco de Dados.
    /// </summary>
    /// <typeparam name="T">Object reconhecido pelo DataBase: EntityBase</typeparam>
    public class RepositoryBaseNoLazyLoad<T> : IDisposable, IRepositoryNoLazyLoad<T> where T : class
    {

        /// <summary>
        /// Instancia do DataBase.
        /// </summary>
        protected readonly EntityContext _dbContext;

        /// <summary>
        /// Objeto T em memória volátil pela chamada de sua Interface.
        /// </summary>
        private DbSet<T> Entity { get { return _dbContext.Set<T>(); } }

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="Db"></param>
        public RepositoryBaseNoLazyLoad(EntityContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbContext.Configuration.LazyLoadingEnabled = false;
            //db.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMMITTED;");
        }

        public void Refresh(T obj)
        {
            _dbContext.Entry(obj).Reload();
        }

        #region Add

        public void AddNotCommit(T obj)
        {
            verifyDate(obj, "AddDate");
            Entity.Add(obj);
        }

        public void AddAllNotCommit(IEnumerable<T> obj)
        {
            foreach (var i in obj)
            {
                verifyDate(i, "AddDate");
                Entity.Add(i);
            }
        }

        public void Add(T obj)
        {
            verifyDate(obj, "AddDate");
            Entity.Add(obj);
            Commit();
        }

        public void AddAll(IEnumerable<T> obj)
        {
            foreach (var i in obj)
            {
                verifyDate(i, "AddDate");
                Entity.Add(i);
            }
            Commit();
        }

        #endregion

        #region Update / AddUpdate

        public void Update(T obj)
        {
            //VerificaElementoInexistenteEmDb(obj);
            verifyDate(obj, "AlterDate");
            Entity.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
            Commit();
        }

        //private void VerificaElementoInexistenteEmDb(T obj)
        //{
        //    if (obj.GetType().GetProperty("Id") != null)
        //    {
        //        var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
        //        if (id > 0)
        //        {
        //            if (GetById(id) == null)
        //            {

        //                return;
        //            }
        //        }
        //    }
        //}

        public void UpdateNotCommit(T obj)
        {
            //VerificaElementoInexistenteEmDb(obj);
            verifyDate(obj, "AlterDate");
            Entity.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
        }

        public void AddOrUpdateNotCommit(T obj)
        {
            if (obj.GetType().GetProperty("Id") != null)
            {
                var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
                if (id > 0)
                    UpdateNotCommit(obj);
                else
                    AddNotCommit(obj);
            }
        }

        public void AddOrUpdateAllNotCommit(IEnumerable<T> obj)
        {
            foreach (var i in obj)
                AddOrUpdateNotCommit(i);
        }

        public void UpdateAll(IEnumerable<T> listObj)
        {
            foreach (var i in listObj)
                UpdateNotCommit(i);

            Commit();
        }

        public void AddOrUpdate(T obj)
        {
            if (obj.GetType().GetProperty("Id") != null)
            {
                var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
                if (id > 0)
                    Update(obj);
                else
                    Add(obj);
            }
        }

        public void AddOrUpdateAll(IEnumerable<T> obj)
        {
            foreach (var i in obj)
                AddOrUpdate(i);
        }

        #endregion

        public void Dettach(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Detached;
        }

        #region Busca

        public T First()
        {
            return Entity.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            var aaa = Entity.AsQueryable();

            return Entity.ToList();
        }

        public IEnumerable<T> GetAllAsNoTracking()
        {
            var aaa = Entity.AsQueryable().AsNoTracking(); 

            return Entity.ToList();
        }

        public T GetById(int id)
        {
            T entity = Entity.Find(id);
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        //public IEnumerable<T> GetByDate(DateTime initalDate, DateTime finalDate)
        //{
        //    var tipo = typeof(T);
        //    if (tipo.Equals(typeof(EntityBase)))
        //    {

        //        Entity.Where(r=>)
        //    }
        //}
        #endregion

        #region Remove/Deleta

        public void Delete(int id)
        {
            Entity.Remove(GetById(id));
        }

        public void Remove(T obj)
        {
            Entity.Remove(obj);
        }

        public void RemoveAndCommit(T obj)
        {
            Entity.Remove(obj);
            Commit();
        }

        public void RemoveAll(IEnumerable<T> obj)
        {
            foreach (var i in obj)
                Entity.Remove(i);
        }

        #endregion

        public void Commit()
        {
            //using (var transaction = db.Database.BeginTransaction())
            //{
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                
                    //transaction.Rollback();
                    foreach (var i in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", i.Entry.Entity.GetType().Name, i.Entry.State);
                        foreach (var ve in i.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                    throw;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
           // }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private void verifyDate(T obj, string property)
        {
            try
            {
                if (obj.GetType().GetProperty(property) != null)
                {
                    var date = (DateTime)obj.GetType().GetProperty(property).GetValue(obj, null);
                    if (date == null)
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

        public void RemoveByIdAndCommit(int id)
        {
         
        }

        //public T ExecutaSql<T>(string sqlQuery, string sqlSelectLast)
        //{
        //    db.Database.ExecuteSqlCommand(sqlQuery);
        //    return db.Database.SqlQuery<T>(sqlSelectLast).FirstOrDefault();
        //}

    }
}

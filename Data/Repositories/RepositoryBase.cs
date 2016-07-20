using Dominio;
using Dominio.Interfaces.Repositories;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data.Repositories
{
    /// <summary>
    /// Repositório Base, classe de gerencia do Banco de Dados.
    /// </summary>
    /// <typeparam name="T">Object reconhecido pelo DataBase: EntityBase</typeparam>
    public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {

        /// <summary>
        /// Instancia do DataBase.
        /// </summary>
        protected readonly SgqDbDevEntities db;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="Db"></param>
        public RepositoryBase(SgqDbDevEntities Db)
        {
            db = Db;
        }

        /// <summary>
        /// Objeto T em memória volátil pela chamada de sua Interface.
        /// </summary>
        private DbSet<T> Entity { get { return db.Set<T>(); } }

        #region Adiciona e Atualiza

        public void Add(T obj)
        {
            Entity.Add(obj);
            Commit();
        }

        public void AddAll(IEnumerable<T> obj)
        {
            foreach (var i in obj)
            {
                Entity.Add(i);
            }
            Commit();
        }

        public void Update(T obj)
        {
            if (obj.GetType().GetProperty("AlterDate") != null)
            {
                var alterDate = (DateTime) obj.GetType().GetProperty("AlterDate").GetValue(obj, null);
                if (alterDate.IsNull())
                    obj.GetType().GetProperty("AlterDate").SetValue(obj, DateTime.Now);
            }

            db.Entry(obj).State = EntityState.Modified;
            Commit();
        }

        public void AddOrUpdate(T obj)
        {
            if (obj.GetType().GetProperty("Id") != null)
            {
                var id = (int) obj.GetType().GetProperty("Id").GetValue(obj, null);
                if (id > 0)
                    Update(obj);
                else
                    Add(obj);
            }
        }

        #endregion

        #region Busca

        public T First()
        {
            return Entity.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return Entity.ToList();
        }

        public T GetById(int id)
        {
            return Entity.Find(id);
        }

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

        public void RemoveAll(IEnumerable<T> obj)
        {
            foreach (var i in obj)
                Entity.Remove(i);
        }

        #endregion

        public void Commit()
        {
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
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
        }

        public void Dispose()
        {
            db.Dispose();
        }

       
    }
}

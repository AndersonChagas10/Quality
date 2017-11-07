using Dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SgqSystem.Controllers.Api.Recravacao
{
    public class RepoRecravacao<T> : IDisposable where T : class
    {
        /// <summary>
        /// Instancia do DataBase.
        /// </summary>
        protected readonly SgqDbDevEntities db;

        /// <summary>
        /// Objeto T em memória volátil pela chamada de sua Interface.
        /// </summary>
        private DbSet<T> Entity { get { return db.Set<T>(); } }

        public RepoRecravacao()
        {
            db = new SgqDbDevEntities();
        }

        public void Save(T obj)
        {
            if (obj.GetType().GetProperty("Id") != null)
            {
                var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
                if (id > 0)
                    Update(obj, id);
                else
                    Add(obj);
            }
        }

        private void Add(T obj)
        {
            Entity.Add(obj);
            Commit();
        }

        private void Update(T obj, int id)
        {
            var old = Entity.Find(id);
            var entry = db.Entry(old);
            entry.CurrentValues.SetValues(obj);
            Commit();
        }

        public void Delete(T obj)
        {
            if (obj.GetType().GetProperty("Id") != null)
            {
                var id = (int)obj.GetType().GetProperty("Id").GetValue(obj, null);
                var old = Entity.Find(id);
                Entity.Remove(old);
                Commit();
            }
        }

        public void ExecuteQuery(string query)
        {
            db.Database.ExecuteSqlCommand(query);
            Commit();
        }

        private void Commit()
        {
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
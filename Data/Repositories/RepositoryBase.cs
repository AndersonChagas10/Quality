using Dominio.Entities.BaseEntity;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data.Repositories
{
    public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : EntityBase
    {

        protected readonly DbContextSgq db;

        public RepositoryBase(DbContextSgq Db)
        {
            db = Db;
        }

        //Referencia no context a tabela referente o objeto.
        private DbSet<T> Entity { get { return db.Set<T>(); } }

        #region Adiciona e Atualiza

        public void Add(T obj)
        {
            obj.AddDate = DateTime.Now;
            Entity.Add(obj);
        }

        public void AddAll(IEnumerable<T> obj)
        {
            foreach (var i in obj)
                Entity.Add(i);
        }

        public void Update(T obj)
        {
            obj.AlterDate = DateTime.Now;
            db.Entry(obj).State = EntityState.Modified;
        }

        public void AddOrUpdate(T obj)
        {
            if (obj.Id > 0)
                Update(obj);
            else
                Add(obj);
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
        }

        public void Dispose()
        {
            db.Dispose();
        }

       
    }
}

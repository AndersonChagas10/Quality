using Dominio;
using Dominio.Interfaces.Repositories;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Data.Repositories
{
    public class SyncRepository<T> :  ISyncRepository<T> where T : class
    {
        private SgqDbDevEntities connection = new SgqDbDevEntities();
       
        public List<T> GetDataToSincyAudit()
        {
            return new RepositoryBase<T>(connection).GetAll().ToList();
            //connection.Dispose();
        }

        public void SetDataToSincyAudit(List<Coleta> coleta)
        {
            new RepositoryBase<Coleta>(connection).AddAll(coleta);
        }

        /// <summary>
        /// A tabela resultados não possui no momento relacionamentos externos de Foreingin Keys, foi necessário esta validação por como foi forçado relacionamento via INTEGER o banco não verifica automáticamente.
        /// </summary>
        /// <param name="r">Coleta Objeto.</param>
        public void ValidaFkResultado(List<Coleta> coleta)
        {
            var level1 = new RepositoryBase<Level1>(connection).GetAll();
            var level2 = new RepositoryBase<Level2>(connection).GetAll();
            var level3 = new RepositoryBase<Level3>(connection).GetAll();
            foreach (var i in coleta)
            {
                var existisL1 = level1.FirstOrDefault(r => r.Id == i.Id_Level1);
                var existisL2 = level2.FirstOrDefault(r => r.Id == i.Id_Level2);
                var existisL3 = level3.FirstOrDefault(r => r.Id == i.Id_Level3);
                if (existisL1 == null)
                    throw new ExceptionHelper("Invalida Primary Key for Level1");

                if (existisL2 == null)
                    throw new ExceptionHelper("Invalida Primary Key for Level2");

                if (existisL3 == null)
                    throw new ExceptionHelper("Invalida Primary Key for Level3");
            }
        }
    }
}

using Dominio;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ColetaRepository : RepositoryBase<Coleta>, IColetaRepository
    {

        #region Construtor e atributos privados

        private DbSet<Level1> _Level1 { get { return db.Set<Level1>(); } }
        private DbSet<Level2> _Level2 { get { return db.Set<Level2>(); } }
        private DbSet<Level3> _Level3 { get { return db.Set<Level3>(); } }

        public ColetaRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }

        #endregion

        #region Coleta De Dados

        /// <summary>
        /// Salva o resultado de uma coleta de dados na tabela Result old.
        /// </summary>
        /// <param name="r">Coleta com os parametros validados.</param>
        public void SalvarColeta(Coleta r)
        {
            Add(r);
        }

        /// <summary>
        /// Salva uma lista de resultados de coletas de dados na tabela Result old recursivamente.
        /// </summary>
        /// <param name="list">Lista de Coleta com os parametros validados.</param>
        public void SalvarListaColeta(List<Coleta> list)
        {
            AddAll(list);
        }


        #endregion

        #region Auxiliares.

        /// <summary>
        /// A tabela resultados não possui no momento relacionamentos externos de Foreingin Keys, foi necessário esta validação por como foi forçado relacionamento via INTEGER o banco não verifica automáticamente.
        /// </summary>
        /// <param name="r">Coleta Objeto.</param>
        public void ValidaFkColeta(Coleta r)
        {
            if (_Level1.FirstOrDefault(z => z.Id == r.Id_Level1) == null)
                throw new ExceptionHelper("Id Invalido para Operação");

            if (_Level2.FirstOrDefault(z => z.Id == r.Id_Level2) == null)
                throw new ExceptionHelper("Id Invalido para Level2");

            if (_Level3.FirstOrDefault(z => z.Id == r.Id_Level3) == null)
                throw new ExceptionHelper("Id Invalido para Level3");
        } 
        #endregion

    }
}

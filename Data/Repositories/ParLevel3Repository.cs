using Dominio.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace Data.Repositories
{
    public class ParLevel3Repository : IParLevel3Repository
    {

        #region Construtor

        /// <summary>
        /// Instancia do DataBase.
        /// </summary>
        protected readonly SgqDbDevEntities db;

        /// <summary>
        /// Construtor
        /// </summary>
        public ParLevel3Repository(SgqDbDevEntities Db)
        {
            db = Db;
        }

        #endregion

        public List<ParLevel3Level2> GetLevel3VinculadoLevel2(int idLevel1)
        {
            var query = string.Format("select * from ParLevel3Level2 p32 INNER JOIN ParLevel3Level2Level1 p321 ON p321.ParLevel3Level2_Id = p32.Id LEFT JOIN ParLevel3Group pg ON pg.Id = p32.ParLevel3Group_Id     WHERE p321.ParLevel1_Id = {0}", idLevel1);

            return db.Database.SqlQuery<ParLevel3Level2>(query).ToList();

        }
    }
}

using Dominio;
using Dominio.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class RelatorioColetaRepository : RepositoryBase<Coleta> , IRelatorioColetaRepository
    {
        public RelatorioColetaRepository(SgqDbDevEntities Db) : base(Db)
        {
        }

        public List<Coleta> GetColetas()
        {
            return GetAll().ToList();
        }
    }
}

using Data.PlanoDeAcao.Repositorio;
using Dominio;

namespace Data.PlanoDeAcao
{
    public class AcompanhamentoRepository : IAcompanhamentoRepository
    {
        public readonly SgqDbDevEntities _db;
        public AcompanhamentoRepository(SgqDbDevEntities db)
        {
            _db = db;
        }

        public void SalvarAcompanhamentoAcao(AcompanhamentoAcao acompanhamento)
        {
            _db.AcompanhamentoAcao.Add(acompanhamento);
            _db.SaveChanges();
        }
    }
}

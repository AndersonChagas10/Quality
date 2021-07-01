using Data.PlanoDeAcao.Repositorio;
using Dominio;

namespace Data.PlanoDeAcao
{
    public class AcompanhamentoAcaoRepository : Interfaces.IAcompanhamentoAcaoRepository
    {
        private readonly SgqDbDevEntities _db;
        public AcompanhamentoAcaoRepository(SgqDbDevEntities db)
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

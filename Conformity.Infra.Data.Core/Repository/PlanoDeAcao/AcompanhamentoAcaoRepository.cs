using Conformity.Domain.Core.Entities.PlanoDeAcao;

namespace Conformity.Infra.Data.Core.Repository.PlanoDeAcao
{
    public class AcompanhamentoAcaoRepository 
    {
        private readonly EntityContext _dbContext;
        public AcompanhamentoAcaoRepository(EntityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SalvarAcompanhamentoAcao(AcompanhamentoAcao acompanhamento)
        {
            _dbContext.AcompanhamentoAcao.Add(acompanhamento);
            _dbContext.SaveChanges();
        }
    }
}

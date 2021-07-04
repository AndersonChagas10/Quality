using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Interfaces;

namespace Conformity.Infra.Data.Core.Core
{
    public class HistoricoAlteracaoRepository
    {     
        private readonly EntityContext _dbContext;

        public HistoricoAlteracaoRepository(EntityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<HistoricoAlteracoesViewModel> GetAll(string tabelaAlterada, int entidadeId)
        {
                var sql = $@"select 
	                    usuario.username, 
	                    historico.datamodificacao, 
	                    historico.valorAnterior, 
	                    historico.valorAlterado,
                        historico.propriedade
                    from art.HistoricoAlteracoes historico with(nolock)
                    left join art.usuario usuario with(nolock) on
	                    usuario.id = historico.usuariomodificacaoId
                    where 1=1
                    and historico.TabelaAlterada = '{tabelaAlterada}'
                    and historico.EntidadeId = '{entidadeId}'";
                return _dbContext.Database.SqlQuery<HistoricoAlteracoesViewModel>(sql);
        }
    }
}

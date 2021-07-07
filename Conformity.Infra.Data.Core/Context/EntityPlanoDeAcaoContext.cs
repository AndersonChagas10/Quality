using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System.Data.Entity;

namespace Conformity.Infra.Data.Core
{
    public partial class EntityContext : DbContext
    {
        public virtual DbSet<EvidenciaConcluida> Acao { get; set; }
        public virtual DbSet<AcompanhamentoAcao> AcompanhamentoAcao { get; set; }
    }
}

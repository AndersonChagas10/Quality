using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System.Data.Entity;

namespace Conformity.Infra.Data.Core.Context
{
    public class EntityAcaoContext : DbContext
    {
        public virtual DbSet<Acao> Acao { get; set; }
    }
}

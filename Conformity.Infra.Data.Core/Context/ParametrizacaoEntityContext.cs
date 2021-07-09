using Conformity.Domain.Core.Entities.Parametrizacao;
using System.Data.Entity;

namespace Conformity.Infra.Data.Core
{
    public class ParametrizacaoEntityContext : EntityContext
    {
        public virtual DbSet<ParCompany> ParCompany { get; set; }
    }
}

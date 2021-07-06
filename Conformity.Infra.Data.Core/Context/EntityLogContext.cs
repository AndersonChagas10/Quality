using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Entities.Log;
using Conformity.Domain.Core.Entities.Parametrizacao;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Conformity.Infra.Data.Core
{
    public partial class EntityContext : DbContext
    {
        public virtual DbSet<EntityTrack> EntityTrack { get; set; }
    }
}

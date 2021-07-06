using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Entities.Log;
using Conformity.Domain.Core.Entities.Parametrizacao;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Conformity.Infra.Data.Core
{
    public partial class EntityContext : DbContext
    {
        public EntityContext(string connectionString = "name=DefaultConnection")
            : base(connectionString)
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}

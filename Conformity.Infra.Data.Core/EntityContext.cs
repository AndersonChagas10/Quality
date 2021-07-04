using Conformity.Domain.Core.Entities;
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

        public virtual DbSet<ParCompany> ParCompany { get; set; }
        public virtual DbSet<HistoricoAlteracoes> HistoricoAlteracoes { get; set; }
    }
}

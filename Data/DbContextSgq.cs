using Data.EntityConfig;
using Dominio.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Data
{
    public class DbContextSgq : DbContext
    {

        public DbContextSgq()
            : base("DbContextSgq")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.CreateIfNotExists();
        }

        //public DbSet<EntityBase> EntityBases { get; set; }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<ResultOld> Results { get; set; }
        //public DbSet<Result> Results { get; set; }
        //public DbSet<AuditCenter> AuditCenters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties().Where(r => r.Name == r.ReflectedType.Name + "Id").Configure(r => r.IsKey());

            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(70));


            modelBuilder.Configurations.Add(new UserConfig());
            //modelBuilder.Configurations.Add(new ResultConfig());
            //modelBuilder.Configurations.Add(new AuditCenterConfig());
            modelBuilder.Configurations.Add(new OperacaoConfig());
            modelBuilder.Configurations.Add(new MonitoramentoConfig());
            modelBuilder.Configurations.Add(new TarefaConfig());
            modelBuilder.Configurations.Add(new ResultOldConfig());

        }

    }
}

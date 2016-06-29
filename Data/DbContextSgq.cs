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
        }

        public DbSet<User> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties().Where(r => r.Name == r.ReflectedType.Name + "Id").Configure(r => r.IsKey());

            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(70));

            modelBuilder.Configurations.Add(new UserConfig());

        }

    }
}

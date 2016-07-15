using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class UserConfig : EntityTypeConfiguration<UserSgq>
    {

        public UserConfig()
        {

            HasKey(r => r.Id);
            Property(r => r.Name).IsRequired().HasMaxLength(50);
            Property(r => r.Password).IsRequired().HasMaxLength(50);
            Property(r => r.AcessDate).IsOptional();

        }

    }
}

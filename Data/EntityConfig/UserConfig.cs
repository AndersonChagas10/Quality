using Dominio.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class UserConfig : EntityTypeConfiguration<User>
    {

        public UserConfig()
        {

            HasKey(r => r.Id);
            Property(r => r.Name).IsRequired().HasMaxLength(50);
            Property(r => r.Password).IsRequired().HasMaxLength(50);


        }

    }
}

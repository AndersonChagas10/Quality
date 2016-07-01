using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    class AuditCenterConfig : EntityTypeConfiguration<AuditCenter>
    {
        public AuditCenterConfig()
        {
            HasKey(r => r.Id);
            Property(r => r.Name).IsRequired();
            Property(r => r.AddDate).IsRequired();
            Property(r => r.AlterDate).IsOptional();

        }
    }
}

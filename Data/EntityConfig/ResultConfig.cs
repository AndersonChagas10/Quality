using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class ResultConfig : EntityTypeConfiguration<Result>
    {
        public ResultConfig()
        {
            HasKey(r => r.Id);
            HasRequired(r => r.auditCenter)
                .WithMany()
                .HasForeignKey(r => r.Id_AuditCenter);
            //HasRequired(r => r.auditCenter)
            //   .WithRequiredPrincipal(r => r.result);
            Property(r => r.AddDate).IsRequired();
            Property(r => r.Evaluate).IsRequired();
            Property(r => r.NotConform).IsRequired();
            Property(r => r.AlterDate).IsOptional();
        }
    }
}

using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class AuditConfig : EntityTypeConfiguration<Audit>
    {
        public AuditConfig()
        {

            HasKey(r => r.Id);
            //Property(r => r.Name).IsRequired().HasMaxLength(50);
            HasRequired(r => r.auditCenter);//.WithRequiredDependent(r=>r.Id);
            Property(r => r.AddDate).IsRequired();
            Property(r => r.Evaluate).IsRequired();
            Property(r => r.NotConform).IsRequired();

            
        }
    }
}

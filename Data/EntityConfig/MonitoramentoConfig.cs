using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class MonitoramentoConfig : EntityTypeConfiguration<Monitoramento>
    {
        public MonitoramentoConfig()
        {
            HasKey(r => r.Id);
            Property(r => r.Name).IsRequired();
        }
    }
}

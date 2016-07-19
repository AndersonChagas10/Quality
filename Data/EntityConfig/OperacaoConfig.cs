using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class OperacaoConfig : EntityTypeConfiguration<Operacao>
    {
        public OperacaoConfig()
        {
            HasKey(r => r.Id);
            Property(r => r.Name).IsRequired();
        }
    }
}

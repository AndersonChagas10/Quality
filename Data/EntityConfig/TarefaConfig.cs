using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class TarefaConfig : EntityTypeConfiguration<Tarefa>
    {
        public TarefaConfig()
        {
            HasKey(r => r.Id);
            Property(r => r.Name).IsRequired().HasMaxLength(250);
        }
    }
}

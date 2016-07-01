using Dominio.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class ResultOldConfig : EntityTypeConfiguration<ResultOld>
    {
        public ResultOldConfig()
        {
            HasKey(r => r.Id);
            Property(r => r.Id_Monitoramento).IsRequired();
            Property(r => r.Id_Tarefa).IsRequired();
            Property(r => r.Id_Operacao).IsRequired();
            Property(r => r.AddDate).IsRequired();
            Property(r => r.AlterDate).IsOptional();
            Property(r => r.NotConform).IsOptional();
            Property(r => r.Evaluate).IsOptional();
        }
    }
}

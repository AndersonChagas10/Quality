using Dominio.Entities.BaseEntity;

namespace Dominio.Entities
{
    public class ResultOld : DataCollectionBase
    {
        public int Id_Tarefa { get; set; }
        public int Id_Operacao { get; set; }
        public int Id_Monitoramento { get; set; }
    }
}

using Dominio.Entities.BaseEntity;
using DTO.Helpers;

namespace Dominio.Entities
{
    public class ResultOld : DataCollectionBase
    {

        public int Id_Tarefa { get; set; }
        public int Id_Operacao { get; set; }
        public int Id_Monitoramento { get; set; }
        public int numero1 { get; set; }
        public int numero2 { get; set; }

        /// <summary>
        /// Ignoreds pelo Entity Framework.
        /// </summary>
        public string Operacao { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get; set; }

        /// <summary>
        /// Construtor para o Entity Framework.
        /// </summary>
        public ResultOld()
        {

        }

      
      
    }

   
}

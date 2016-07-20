using DTO.Entities.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class ResultOldDTO : DataCollectionBase
    {
        public int Id_Tarefa { get; set; }
        public int Id_Operacao { get; set; }
        public int Id_Monitoramento { get; set; }
        public int numero1 { get; set; }
        public int numero2 { get; set; }
        public int Period { get; set; }
        public int Reaudit { get; set; }
        public int Auditor { get; set; }

        /// <summary>
        /// Ignoreds pelo Entity Framework.
        /// </summary>
        public string Operacao { get; set; }
        public string Monitoramento { get; set; }
        public string Tarefa { get; set; }


        /// <summary>
        /// Constructor para nova avaliação.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_Tarefa"></param>
        /// <param name="id_Operacao"></param>
        /// <param name="id_Monitoramento"></param>
        /// <param name="evaluate"></param>
        /// <param name="notConform"></param>
        public void ValidaResultOLd()
        {
            Guard.ForNegative(Period, "The Period must be positive.");
            Guard.forValueZero(Period, "The Period must be diferent of zero.");

            Guard.ForNegative(Auditor, "The Auditor Id must be positive.");
            Guard.forValueZero(Auditor, "The Auditor Id must be diferent of zero.");

            Guard.ForValidFk(Id_Monitoramento, "Cannot insert the data because: The level1 Identity Key is Invalid or Null.");//O Id do Monitoramento está em formato Inválido ou Nulo
            Guard.ForValidFk(Id_Operacao, "Cannot insert the data because: The level2 Identity Key is Invalid or Null.");//O Id da Operação está em formato Inválido ou Nulo
            Guard.ForValidFk(Id_Tarefa, "Cannot insert the data because: The level3 Identity Key is Invalid or Null.");//O Id da Tarefa está em formato Inválido ou Nulo
            Guard.ForNegative(Id, "Cannot insert the data because: The level1 Identity Key is Negative. Please verify a positive value for Save data.");
            Guard.ForNegative(Evaluate, "Evaluate");
            Guard.ForNegative(NotConform, "Not Conform");

            //Id = id;
            //Id_Tarefa = id_Tarefa;
            //Id_Operacao = id_Operacao;
            //Id_Monitoramento = id_Monitoramento;
            //Evaluate = evaluate;
            //NotConform = notConform;
        }
    }
}

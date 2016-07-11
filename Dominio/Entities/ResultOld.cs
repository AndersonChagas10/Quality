using Dominio.Entities.BaseEntity;
using Dominio.Helpers;
using System;

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

        /// <summary>
        /// Constructor para nova avaliação.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_Tarefa"></param>
        /// <param name="id_Operacao"></param>
        /// <param name="id_Monitoramento"></param>
        /// <param name="evaluate"></param>
        /// <param name="notConform"></param>
        public ResultOld(int id, int id_Tarefa, int id_Operacao, int id_Monitoramento, decimal evaluate, decimal notConform)
        {
            Guard.ForValidFk(id_Monitoramento, "O Id do Monitoramento está em formato Inválido ou Nulo.");
            Guard.ForValidFk(id_Operacao, "O Id da Operação está em formato Inválido ou Nulo.");
            Guard.ForValidFk(id_Tarefa, "O Id da Tarefa está em formato Inválido ou Nulo.");
            Guard.ForNegative(id, "O Id do Registro");
            Guard.ForNegative(evaluate, "O Total Avaliado");
            Guard.ForNegative(notConform, "O Total Avaliado");

            Id = id;
            Id_Tarefa = id_Tarefa;
            Id_Operacao = id_Operacao;
            Id_Monitoramento = id_Monitoramento;
            Evaluate = evaluate;
            NotConform = notConform;
        }

      
    }

   
}

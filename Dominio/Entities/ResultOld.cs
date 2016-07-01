using Dominio.Entities.BaseEntity;
using Dominio.Helpers;
using System;

namespace Dominio.Entities
{
    public class ResultOld : DataCollectionBase
    {
        //public ResultOld(int id, int id_Tarefa, int id_Operacao, int id_Monitoramento)
        //{
        //    if (id.IsNull())
        //        throw new Exception("O Id não pode ser nulo.");
        //    if (id_Tarefa.IsNull())
        //        throw new Exception("A Tarefa não pode ser nulo.");
        //    if (id_Operacao.IsNull())
        //        throw new Exception("O Indicador não pode ser nulo.");
        //    if (id_Monitoramento.IsNull())
        //        throw new Exception("O Monitoramento não pode ser nulo.");

        //    Id = id;
        //    Id_Tarefa = id_Tarefa;
        //    Id_Operacao = id_Operacao;
        //    Id_Monitoramento = id_Monitoramento;
        //}

        public int Id_Tarefa { get; set; }
        public int Id_Operacao { get; set; }
        public int Id_Monitoramento { get; set; }



    }
}

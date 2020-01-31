namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TarefaMonitoramentos
    {
        public int Id { get; set; }

        public int Tarefa { get; set; }

        public int Monitoramento { get; set; }

        public int Sequencia { get; set; }

        public decimal Peso { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? Unidade { get; set; }

        public bool? NaoAvaliado { get; set; }

        public bool? ExibirReincidencia { get; set; }

        [ForeignKey("Monitoramento")]
        public virtual Monitoramentos Monitoramentos { get; set; }

        [ForeignKey("Tarefa")]
        public virtual Tarefas Tarefas { get; set; }

        [ForeignKey("Unidade")]
        public virtual Unidades Unidades { get; set; }
    }
}

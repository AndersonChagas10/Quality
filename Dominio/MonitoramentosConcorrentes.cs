namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MonitoramentosConcorrentes
    {
        [Key]
        public int MonitoramentoConcorrenteId { get; set; }

        public int? UnidadeId { get; set; }

        public int OperacaoId { get; set; }

        public int TarefaId { get; set; }

        public int MonitoramentoId { get; set; }

        public int ConcorrenteId { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Monitoramentos Monitoramentos { get; set; }

        public virtual Monitoramentos Monitoramentos1 { get; set; }

        public virtual Operacoes Operacoes { get; set; }

        public virtual Tarefas Tarefas { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}

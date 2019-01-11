namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TarefaAvaliacoes
    {
        public int Id { get; set; }

        public int Departamento { get; set; }

        public int Operacao { get; set; }

        public int Tarefa { get; set; }

        public int Avaliacao { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(10)]
        public string Acesso { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? Unidade { get; set; }

        [ForeignKey("Departamento")]
        public virtual Departamentos Departamentos { get; set; }

        [ForeignKey("Operacao")]
        public virtual Operacoes Operacoes { get; set; }

        [ForeignKey("Tarefa")]
        public virtual Tarefas Tarefas { get; set; }

        [ForeignKey("Unidade")]
        public virtual Unidades Unidades { get; set; }
    }
}

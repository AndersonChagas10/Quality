namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tarefas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tarefas()
        {
            TarefaAmostras = new HashSet<TarefaAmostras>();
            TarefaAvaliacoes = new HashSet<TarefaAvaliacoes>();
            TarefaMonitoramentos = new HashSet<TarefaMonitoramentos>();
        }

        public int Id { get; set; }

        public int Operacao { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20)]
        public string Amostragem { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? Departamento { get; set; }

        [StringLength(20)]
        public string Frequencia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Vigencia { get; set; }

        public int? Produto { get; set; }

        public bool? EditarAcesso { get; set; }

        public bool? ExibirAcesso { get; set; }

        [StringLength(100)]
        public string FormaAmostragem { get; set; }

        public bool? AvaliarProdutos { get; set; }

        public int? Sequencial { get; set; }

        public bool? InformarPesagem { get; set; }

        public virtual Departamentos Departamentos { get; set; }

        public virtual Operacoes Operacoes { get; set; }

        public virtual Produtos Produtos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaAmostras> TarefaAmostras { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaAvaliacoes> TarefaAvaliacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaMonitoramentos> TarefaMonitoramentos { get; set; }
    }
}

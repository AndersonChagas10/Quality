namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Operacoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Operacoes()
        {
            AcoesCorretivas = new HashSet<AcoesCorretivas>();
            Alertas = new HashSet<Alertas>();
            DepartamentoOperacoes = new HashSet<DepartamentoOperacoes>();
            FamiliaProdutos = new HashSet<FamiliaProdutos>();
            Horarios = new HashSet<Horarios>();
            Metas = new HashSet<Metas>();
            MonitoramentosConcorrentes = new HashSet<MonitoramentosConcorrentes>();
            ObservacoesPadroes = new HashSet<ObservacoesPadroes>();
            PacotesOperacoes = new HashSet<PacotesOperacoes>();
            Pontos = new HashSet<Pontos>();
            TarefaAvaliacoes = new HashSet<TarefaAvaliacoes>();
            Tarefas = new HashSet<Tarefas>();
            TipificacaoReal = new HashSet<TipificacaoReal>();
            VolumeProducao = new HashSet<VolumeProducao>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [StringLength(20)]
        public string Nivel { get; set; }

        [StringLength(20)]
        public string Frequencia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Vigencia { get; set; }

        public bool? ControleVP { get; set; }

        public bool? ADCAMPOVAZIO { get; set; }

        public bool? AvaliarEquipamentos { get; set; }

        public bool? AvaliarCamaras { get; set; }

        public bool? Especifico { get; set; }

        public bool? ExibirPColeta { get; set; }

        public bool? PadraoPerc { get; set; }

        public bool? ControleFP { get; set; }

        public bool? AvaliarSequencial { get; set; }

        [StringLength(100)]
        public string Criterio { get; set; }

        [StringLength(20)]
        public string FrequenciaAlerta { get; set; }

        public bool? ExibirData { get; set; }

        public bool? EmitirAlerta { get; set; }

        public bool? ControlarAvaliacoes { get; set; }

        public bool? AlterarAmostra { get; set; }

        public bool? IncluirTarefa { get; set; }

        public bool? IncluirAvaliacao { get; set; }

        public bool? AlertaAgruparAvaliacoes { get; set; }

        public int? QteFamiliaUnidade { get; set; }

        public int? QteFamiliaCorporativa { get; set; }

        public bool? ExibirTarefasAcumuladas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcoesCorretivas> AcoesCorretivas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alertas> Alertas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepartamentoOperacoes> DepartamentoOperacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamiliaProdutos> FamiliaProdutos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horarios> Horarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Metas> Metas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonitoramentosConcorrentes> MonitoramentosConcorrentes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ObservacoesPadroes> ObservacoesPadroes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PacotesOperacoes> PacotesOperacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pontos> Pontos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaAvaliacoes> TarefaAvaliacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tarefas> Tarefas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipificacaoReal> TipificacaoReal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeProducao> VolumeProducao { get; set; }
    }
}

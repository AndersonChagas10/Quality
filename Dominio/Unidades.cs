namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Unidades
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unidades()
        {
            Alertas = new HashSet<Alertas>();
            Equipamentos = new HashSet<Equipamentos>();
            FamiliaProdutos = new HashSet<FamiliaProdutos>();
            GrupoProjeto = new HashSet<GrupoProjeto>();
            Horarios = new HashSet<Horarios>();
            MonitoramentosConcorrentes = new HashSet<MonitoramentosConcorrentes>();
            PadraoMonitoramentos = new HashSet<PadraoMonitoramentos>();
            TarefaAmostras = new HashSet<TarefaAmostras>();
            TarefaAvaliacoes = new HashSet<TarefaAvaliacoes>();
            TarefaMonitoramentos = new HashSet<TarefaMonitoramentos>();
            TipificacaoReal = new HashSet<TipificacaoReal>();
            Usuarios = new HashSet<Usuarios>();
            UsuarioUnidades = new HashSet<UsuarioUnidades>();
            VerificacaoTipificacao = new HashSet<VerificacaoTipificacao>();
            VolumeProducao = new HashSet<VolumeProducao>();
        }

        public int Id { get; set; }

        public int? Identificador { get; set; }

        [StringLength(5)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? SIF { get; set; }

        public int? Estado { get; set; }

        public int? Regional { get; set; }

        public int? Cluster { get; set; }

        public int? Codigo { get; set; }

        [StringLength(50)]
        public string EnderecoIP { get; set; }

        [StringLength(50)]
        public string NomeDatabase { get; set; }

        public bool? Ativa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alertas> Alertas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipamentos> Equipamentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamiliaProdutos> FamiliaProdutos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoProjeto> GrupoProjeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horarios> Horarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonitoramentosConcorrentes> MonitoramentosConcorrentes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoMonitoramentos> PadraoMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaAmostras> TarefaAmostras { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaAvaliacoes> TarefaAvaliacoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaMonitoramentos> TarefaMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipificacaoReal> TipificacaoReal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuarios> Usuarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioUnidades> UsuarioUnidades { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VerificacaoTipificacao> VerificacaoTipificacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolumeProducao> VolumeProducao { get; set; }
    }
}

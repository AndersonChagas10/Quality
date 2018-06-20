namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Monitoramentos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Monitoramentos()
        {
            GrupoTipoAvaliacaoMonitoramentos = new HashSet<GrupoTipoAvaliacaoMonitoramentos>();
            MonitoramentoEquipamentos = new HashSet<MonitoramentoEquipamentos>();
            MonitoramentosConcorrentes = new HashSet<MonitoramentosConcorrentes>();
            MonitoramentosConcorrentes1 = new HashSet<MonitoramentosConcorrentes>();
            PadraoMonitoramentos = new HashSet<PadraoMonitoramentos>();
            PadraoTolerancias = new HashSet<PadraoTolerancias>();
            TarefaMonitoramentos = new HashSet<TarefaMonitoramentos>();
            VerificacaoTipificacaoTarefaIntegracao = new HashSet<VerificacaoTipificacaoTarefaIntegracao>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Nome { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [StringLength(20)]
        public string Frequencia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Vigencia { get; set; }

        [StringLength(5)]
        public string SiglaContusao { get; set; }

        [StringLength(5)]
        public string SiglaFalhaOperacional { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoTipoAvaliacaoMonitoramentos> GrupoTipoAvaliacaoMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonitoramentoEquipamentos> MonitoramentoEquipamentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonitoramentosConcorrentes> MonitoramentosConcorrentes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonitoramentosConcorrentes> MonitoramentosConcorrentes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoMonitoramentos> PadraoMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PadraoTolerancias> PadraoTolerancias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaMonitoramentos> TarefaMonitoramentos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VerificacaoTipificacaoTarefaIntegracao> VerificacaoTipificacaoTarefaIntegracao { get; set; }
    }
}

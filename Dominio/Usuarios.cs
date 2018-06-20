namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuarios()
        {
            AcompanhamentoTarefa = new HashSet<AcompanhamentoTarefa>();
            Cabecalho = new HashSet<Cabecalho>();
            NiveisUsuarios = new HashSet<NiveisUsuarios>();
            PlanoDeAcaoQuemQuandoComo = new HashSet<PlanoDeAcaoQuemQuandoComo>();
            TarefaPA = new HashSet<TarefaPA>();
            UsuarioUnidades = new HashSet<UsuarioUnidades>();
            VinculoCampoCabecalho = new HashSet<VinculoCampoCabecalho>();
            VinculoCampoTarefa = new HashSet<VinculoCampoTarefa>();
            VinculoParticipanteMultiplaEscolha = new HashSet<VinculoParticipanteMultiplaEscolha>();
            VinculoParticipanteProjeto = new HashSet<VinculoParticipanteProjeto>();
        }

        public int Id { get; set; }

        public int? Identificador { get; set; }

        public int Unidade { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Senha { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [StringLength(30)]
        public string Funcao { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public bool? DefineFamilia { get; set; }

        public bool? DefineParametros { get; set; }

        public bool? EditaUsuarios { get; set; }

        public bool? ReceberAlerta { get; set; }

        public int? Regional { get; set; }

        public DateTime DataAlteracaoSenha { get; set; }

        public bool? ConfiguraSistema { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcompanhamentoTarefa> AcompanhamentoTarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cabecalho> Cabecalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NiveisUsuarios> NiveisUsuarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanoDeAcaoQuemQuandoComo> PlanoDeAcaoQuemQuandoComo { get; set; }

        public virtual Regionais Regionais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarefaPA> TarefaPA { get; set; }

        public virtual Unidades Unidades { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioUnidades> UsuarioUnidades { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoCabecalho> VinculoCampoCabecalho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoCampoTarefa> VinculoCampoTarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoParticipanteMultiplaEscolha> VinculoParticipanteMultiplaEscolha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VinculoParticipanteProjeto> VinculoParticipanteProjeto { get; set; }
    }
}

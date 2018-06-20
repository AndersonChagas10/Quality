namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_participante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pa_participante()
        {
            pa_tarefa = new HashSet<pa_tarefa>();
            pa_vinculo_campo_tarefa = new HashSet<pa_vinculo_campo_tarefa>();
            pa_vinculo_participante_multipla_escolha = new HashSet<pa_vinculo_participante_multipla_escolha>();
            pa_vinculo_participante_projeto = new HashSet<pa_vinculo_participante_projeto>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string nome { get; set; }

        [Required]
        [StringLength(50)]
        public string senha { get; set; }

        public bool ativo { get; set; }

        public int codigo { get; set; }

        [Required]
        [StringLength(150)]
        public string email { get; set; }

        [StringLength(50)]
        public string telefone { get; set; }

        public int id_empresa { get; set; }

        public byte? perfil_acesso { get; set; }

        public virtual pa_empresa pa_empresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_tarefa> pa_tarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_campo_tarefa> pa_vinculo_campo_tarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_participante_multipla_escolha> pa_vinculo_participante_multipla_escolha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_participante_projeto> pa_vinculo_participante_projeto { get; set; }
    }
}

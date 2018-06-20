namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_campo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pa_campo()
        {
            pa_multipla_escolha = new HashSet<pa_multipla_escolha>();
            pa_vinculo_campo_tarefa = new HashSet<pa_vinculo_campo_tarefa>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string nome { get; set; }

        [Required]
        [StringLength(20)]
        public string tipo { get; set; }

        public bool agrupador { get; set; }

        public int? sequencia { get; set; }

        public bool obrigatorio { get; set; }

        public bool ativo { get; set; }

        public int id_projeto { get; set; }

        public bool predefinido { get; set; }

        public bool modificavel { get; set; }

        public virtual pa_projeto pa_projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_multipla_escolha> pa_multipla_escolha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_campo_tarefa> pa_vinculo_campo_tarefa { get; set; }
    }
}

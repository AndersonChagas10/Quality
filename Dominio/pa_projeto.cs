namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_projeto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pa_projeto()
        {
            pa_campo = new HashSet<pa_campo>();
            pa_tarefa = new HashSet<pa_tarefa>();
            pa_vinculo_participante_projeto = new HashSet<pa_vinculo_participante_projeto>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string nome { get; set; }

        public int id_empresa { get; set; }

        public int? id_grupo_projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_campo> pa_campo { get; set; }

        public virtual pa_empresa pa_empresa { get; set; }

        public virtual pa_grupo_projeto pa_grupo_projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_tarefa> pa_tarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_participante_projeto> pa_vinculo_participante_projeto { get; set; }
    }
}

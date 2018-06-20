namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_grupo_projeto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pa_grupo_projeto()
        {
            pa_projeto = new HashSet<pa_projeto>();
        }

        [Required]
        [StringLength(150)]
        public string nome { get; set; }

        public int id { get; set; }

        public int? sequencia { get; set; }

        public int? id_empresa { get; set; }

        public virtual pa_empresa pa_empresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_projeto> pa_projeto { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_multipla_escolha
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pa_multipla_escolha()
        {
            pa_vinculo_campo_tarefa = new HashSet<pa_vinculo_campo_tarefa>();
            pa_vinculo_participante_multipla_escolha = new HashSet<pa_vinculo_participante_multipla_escolha>();
        }

        public int id { get; set; }

        public int id_campo { get; set; }

        public string nome { get; set; }

        public int? id_externo { get; set; }

        [StringLength(50)]
        public string cor { get; set; }

        public int? id_pai_externo { get; set; }

        public virtual pa_campo pa_campo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_campo_tarefa> pa_vinculo_campo_tarefa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_participante_multipla_escolha> pa_vinculo_participante_multipla_escolha { get; set; }
    }
}

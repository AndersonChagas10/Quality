namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_tarefa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pa_tarefa()
        {
            pa_acompanhamento_tarefa = new HashSet<pa_acompanhamento_tarefa>();
            pa_vinculo_campo_tarefa = new HashSet<pa_vinculo_campo_tarefa>();
        }

        public int id { get; set; }

        public int id_projeto { get; set; }

        public int? id_participante_criador { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? data_criacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_acompanhamento_tarefa> pa_acompanhamento_tarefa { get; set; }

        public virtual pa_participante pa_participante { get; set; }

        public virtual pa_projeto pa_projeto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pa_vinculo_campo_tarefa> pa_vinculo_campo_tarefa { get; set; }
    }
}

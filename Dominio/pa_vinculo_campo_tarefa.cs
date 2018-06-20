namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_vinculo_campo_tarefa
    {
        public int id { get; set; }

        public int? id_multipla_escolha { get; set; }

        public int id_campo { get; set; }

        public int id_tarefa { get; set; }

        [StringLength(150)]
        public string valor { get; set; }

        public int? id_participante { get; set; }

        public virtual pa_campo pa_campo { get; set; }

        public virtual pa_multipla_escolha pa_multipla_escolha { get; set; }

        public virtual pa_participante pa_participante { get; set; }

        public virtual pa_tarefa pa_tarefa { get; set; }
    }
}

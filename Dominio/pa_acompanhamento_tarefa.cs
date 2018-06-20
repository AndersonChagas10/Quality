namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_acompanhamento_tarefa
    {
        public int id { get; set; }

        public int id_tarefa { get; set; }

        public DateTime data_envio { get; set; }

        [StringLength(200)]
        public string comentario { get; set; }

        [StringLength(50)]
        public string enviado { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        [StringLength(200)]
        public string nome_participante_envio { get; set; }

        public virtual pa_tarefa pa_tarefa { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AcompanhamentoTarefa")]
    public partial class AcompanhamentoTarefa
    {
        public int Id { get; set; }

        public int IdTarefa { get; set; }

        public DateTime DataEnvio { get; set; }

        public string Comentario { get; set; }

        public string Enviado { get; set; }

        public string Status { get; set; }

        public string NomeParticipanteEnvio { get; set; }

        public int IdParticipanteEnvio { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        public bool Ativo { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual TarefaPA TarefaPA { get; set; }
    }
}

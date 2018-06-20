namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConfiguracaoEmailPA")]
    public partial class ConfiguracaoEmailPA
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string Host { get; set; }

        public int Port { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }
    }
}

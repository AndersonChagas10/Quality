namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConfiguracaoEmail")]
    public partial class ConfiguracaoEmail
    {
        public int Id { get; set; }

        [Required]
        public string Assunto { get; set; }

        [Required]
        public string Corpo1 { get; set; }

        [Required]
        public string Corpo2 { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}

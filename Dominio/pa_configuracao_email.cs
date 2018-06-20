namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pa_configuracao_email
    {
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string email { get; set; }

        [Required]
        [StringLength(200)]
        public string senha { get; set; }

        [Required]
        [StringLength(50)]
        public string host { get; set; }

        public int port { get; set; }
    }
}

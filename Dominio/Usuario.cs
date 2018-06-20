namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        public decimal nCdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string cNmUsuario { get; set; }

        [Required]
        [StringLength(20)]
        public string cSigla { get; set; }

        [StringLength(50)]
        public string cEMail { get; set; }

        [StringLength(30)]
        public string cTelefone { get; set; }

        [StringLength(30)]
        public string cCelular { get; set; }
    }
}

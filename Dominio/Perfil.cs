namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Perfil")]
    public partial class Perfil
    {
        [Key]
        public decimal nCdPerfil { get; set; }

        [StringLength(100)]
        [Required(AllowEmptyStrings = true)]
        public string cNmPerfil { get; set; }
    }
}

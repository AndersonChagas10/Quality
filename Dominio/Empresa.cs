namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Empresa")]
    public partial class Empresa
    {
        [Key]
        public decimal nCdEmpresa { get; set; }

        [Required]
        [StringLength(50)]
        public string cNmEmpresa { get; set; }

        [Required]
        [StringLength(3)]
        public string cSgEmpresa { get; set; }

        [StringLength(10)]
        public string cCdOrgaoRegulador { get; set; }
    }
}

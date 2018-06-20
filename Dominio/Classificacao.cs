namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Classificacao")]
    public partial class Classificacao
    {
        [Key]
        public decimal nCdClassificacao { get; set; }

        [Required]
        [StringLength(50)]
        public string cNmClassificacao { get; set; }

        [Required]
        [StringLength(10)]
        public string cNrClassificacao { get; set; }
    }
}

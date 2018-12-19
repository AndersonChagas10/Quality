namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AreasParticipantes
    {
        [Key]
        public decimal nCdCaracteristica { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(50)]
        public string cNmCaracteristica { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(10)]
        public string cNrCaracteristica { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(3)]
        public string cSgCaracteristica { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(30)]
        public string cIdentificador { get; set; }
    }
}

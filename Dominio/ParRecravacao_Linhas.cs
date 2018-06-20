namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParRecravacao_Linhas
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParRecravacao_TypeLata_Id { get; set; }

        public int? NumberOfHeads { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool? IsActive { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLataImagens
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Required]
        public byte[] Imagem { get; set; }

        public int ParRecravacao_TipoLata_Id { get; set; }

        [Required]
        [StringLength(500)]
        public string PathFile { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        public int PontoIndex { get; set; }
    }
}

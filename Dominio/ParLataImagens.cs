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

        [Required(AllowEmptyStrings = true)]
        public byte[] Imagem { get; set; }

        public int ParRecravacao_TipoLata_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(500)]
        public string PathFile { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string FileName { get; set; }

        public int PontoIndex { get; set; }
    }
}

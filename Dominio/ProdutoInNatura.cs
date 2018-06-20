namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProdutoInNatura")]
    public partial class ProdutoInNatura
    {
        [Key]
        public decimal nCdProduto { get; set; }

        [Required]
        [StringLength(60)]
        public string cNmProduto { get; set; }
    }
}

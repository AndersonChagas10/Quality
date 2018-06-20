namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClassificacaoProduto")]
    public partial class ClassificacaoProduto
    {
        [Key]
        [Column(Order = 0)]
        public decimal nCdProduto { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal nCdClassificacao { get; set; }
    }
}

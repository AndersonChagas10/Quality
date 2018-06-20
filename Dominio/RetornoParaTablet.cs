namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RetornoParaTablet")]
    public partial class RetornoParaTablet
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Key]
        [Column(Order = 1)]
        public string UsuariosDaUnidade { get; set; }

        public string ParteDaTela { get; set; }
    }
}

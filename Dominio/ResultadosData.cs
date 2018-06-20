namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ResultadosData")]
    public partial class ResultadosData
    {
        public int Id { get; set; }

        public string Chave { get; set; }

        public string Campo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Valor { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VerificacaoContagem")]
    public partial class VerificacaoContagem
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        [Required]
        public string Status { get; set; }
    }
}

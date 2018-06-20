namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTVerificacaoTipificacaoComparacao")]
    public partial class VTVerificacaoTipificacaoComparacao
    {
        public int id { get; set; }

        public int Sequencial { get; set; }

        public int Banda { get; set; }

        [Required]
        [StringLength(30)]
        public string Identificador { get; set; }

        public int NumCaracteristica { get; set; }

        public DateTime DataHora { get; set; }

        public int? valorSGQ { get; set; }

        public int? valorJBS { get; set; }

        public int? nCdEmpresa { get; set; }

        public bool? Conforme { get; set; }
    }
}

namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VTVerificacaoTipificacaoValidacao")]
    public partial class VTVerificacaoTipificacaoValidacao
    {
        public int id { get; set; }

        public int nCdEmpresa { get; set; }

        public DateTime? dMovimento { get; set; }

        public int iSequencial { get; set; }

        public int iSequencialTipificacao { get; set; }

        public int iBanda { get; set; }

        [Required]
        [StringLength(30)]
        public string cIdentificadorTipificacao { get; set; }

        public int nCdCaracteristicaTipificacao { get; set; }
    }
}

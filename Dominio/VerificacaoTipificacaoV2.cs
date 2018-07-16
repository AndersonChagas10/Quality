namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VerificacaoTipificacaoV2
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CollectionDate { get; set; }

        public int? Sequencial { get; set; }

        public int? Banda { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? UserSgq_Id { get; set; }

        [StringLength(250)]
        public string cSgCaracteristica { get; set; }

        public int? GRT_nCdCaracteristicaTipificacao { get; set; }

        public int? JBS_nCdCaracteristicaTipificacao { get; set; }

        public bool? ResultadoComparacaoGRT_JBS { get; set; }

        [StringLength(250)]
        public string cIdentificadorTipificacao { get; set; }

        [StringLength(250)]
        public string cNmCaracteristica { get; set; }

        [StringLength(250)]
        public string Key { get; set; }
    }
}

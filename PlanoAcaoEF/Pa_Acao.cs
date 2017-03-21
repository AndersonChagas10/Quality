namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_Acao
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Required]
        public int? Unidade_Id { get; set; }

        public int? Departamento_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? QuandoInicio { get; set; }

        public int? DuracaoDias { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? QuandoFim { get; set; }

        public string ComoPontosimportantes { get; set; }

        public int? Predecessora_Id { get; set; }

        public string PraQue { get; set; }

        public decimal? QuantoCusta { get; set; }

        public int? Status { get; set; }

        public int? Panejamento_Id { get; set; }

        public int? Pa_IndicadorSgqAcao_Id { get; set; }

        public int? Pa_Problema_Desvio_Id { get; set; }
    }
}
